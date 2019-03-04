using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalara.AvaTax.RestClient;
using Microsoft.Extensions.Logging;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.EntityViews.Commands;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Avalara.Models;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    public class CommitAvalaraTaxBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {

        public CommitAvalaraTaxBlock(IGetItemByPathPipeline getItemByPathPipeline, GetSellableItemCommand getSellableItemCommand, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline, GetEntityViewCommand getEntityViewCommand)
        {
            _getItemByPathPipeline = getItemByPathPipeline;
            _getSellableItemCommand = getSellableItemCommand;

            _findEntity = findEntityPipeline;
            _persistEntityPipeline = persistEntityPipeline;
            _getEntityViewCommand = getEntityViewCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly GetSellableItemCommand _getSellableItemCommand;

        /// <summary>
        /// 
        /// </summary>
        private readonly IFindEntityPipeline _findEntity;

        /// <summary>
        /// 
        /// </summary>
        private readonly IGetItemByPathPipeline _getItemByPathPipeline;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        /// <summary>
        /// 
        /// </summary>
        private readonly GetEntityViewCommand _getEntityViewCommand;


        public override async Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            // We need to ensure the order object is not null.
            Condition.Requires<Order>(arg).IsNotNull<Order>("The order can not be null");


            // Get config from Entity
            // var config = SitecoreItemHelper.GetConfiguration(context.CommerceContext, _getItemByPathPipeline);
            var config = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), Constants.Tax.AvalaraTaxConfig, false), context) as AvalaraTaxEntity;


            // Abort if not found or Sitecore not available
            if (config == null)
            {
                return arg;
            }

            // If reporting is disabled no need to proceed
            if (config.DisableReporting)
            {
                return arg;
            }

            // If Avalara set to false/turned off, abort
            if (config.Enabled == false)
            {
                return arg;
            }

            var fulfillmentComponent = arg.GetComponent<PhysicalFulfillmentComponent>();

            var shippingParty = fulfillmentComponent?.ShippingParty;

            if (shippingParty == null) { return arg; }

            var customer = arg.GetComponent<ContactComponent>();


            var taxExcemptionNumber = string.Empty;
            var taxEntityUseCode = string.Empty;

            if (customer.IsRegistered)
            {
                // Get the customer master entity view
                var entityView = await _getEntityViewCommand.Process(context.CommerceContext, customer.ShopperId, context.GetPolicy<KnownCustomerViewsPolicy>().Master, "", "");
 
                if (entityView != null && entityView.ChildViews.Any())
                {
                    // if no discount field return arg
                    var avalaraCustomerTaxSettingsView = entityView.ChildViews.FirstOrDefault(x => x.Name == Constants.View.AvalaraCustomerTaxSettingsView) as EntityView;
                    if (avalaraCustomerTaxSettingsView != null)
                    {
                        // if discount field is blank, return arg
                        var properties = avalaraCustomerTaxSettingsView.Properties;
                        var entityUseCode = properties.FirstOrDefault(x => x.Name == "EntityUseCode");
                        if (entityUseCode != null)
                        {
                            taxEntityUseCode = entityUseCode.Value;
                        }
                        var excemptionNumber = properties.FirstOrDefault(x => x.Name == "ExemptionNumber");
                        if (excemptionNumber != null)
                        {
                            taxExcemptionNumber = excemptionNumber.Value;
                        }
                    }
                }
            }

            // Build the client
            var client = new AvaTaxClient(config.AppName, config.AppVersion, Environment.MachineName, config.InProductionMode ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                .WithSecurity(config.AccountId, config.LicenseKey);



            var builder = new TransactionBuilder(client, config.CompanyCode, DocumentType.SalesInvoice,customer.CustomerId)
                .WithAddress(TransactionAddressType.SingleLocation, shippingParty.Address1, shippingParty.Address2, null, shippingParty.City, shippingParty.StateCode,
                    shippingParty.ZipPostalCode, shippingParty.CountryCode);

            // Get all line
            var cartLines = arg.Lines.ToList();

            var cartLevelDiscountAmount = 0m;
            var ShippingDiscountAmount = 0m;

            var discounts = arg.Adjustments.Where(x => x.AdjustmentType == "Discount").ToList();

            if (discounts.Any())
            {
                var shippingDiscount = discounts.FirstOrDefault(x => x.Name == "Free Shipping");
                if (shippingDiscount != null)
                {
                    ShippingDiscountAmount = shippingDiscount.Adjustment.Amount;
                }

                var otherDiscounts = discounts.Where(x => x.Name != "Free Shipping").ToList();
                if (otherDiscounts.Any())
                {
                    foreach (var otherDiscount in otherDiscounts)
                    {
                        cartLevelDiscountAmount += otherDiscount.Adjustment.Amount;
                    }
                    
                }

            }

            // Loop through lines and prepare request for Avalara
            foreach (var cartLine in cartLines)
            {


                var taxCode = Constants.Tax.DefaultTaxCode;

                var cartProductComponent = cartLine.GetComponent<CartProductComponent>();


                var description = cartProductComponent.DisplayName;

                // Get the sellableItem entity
                var sellableItem = await _getSellableItemCommand.Process(context.CommerceContext, cartLine.ItemId, false);
                //var c = sellableItem.Components.FirstOrDefault(x => x.Name == "ProductTaxSettingsComponent");
                if (sellableItem != null)
                {
                    // Get SellableItem Tax Code
                    if (sellableItem.HasComponent<ProductTaxSettingsComponent>())
                    {
                        var productTaxSettingsComponent = sellableItem.GetComponent<ProductTaxSettingsComponent>();

                        // Get the taxcode and overide default with it.
                        if (!string.IsNullOrEmpty(productTaxSettingsComponent.TaxCode))
                        {
                            taxCode = productTaxSettingsComponent.TaxCode.Trim();
                        }

                    }

                    // if the sellable item has variant, get the variant
                    if (sellableItem.HasComponent<ItemVariationsComponent>())
                    {
                        var selectedVariantComponent = cartLine.GetComponent<ItemVariationSelectedComponent>();

                        var variantItem =
                            sellableItem.GetComponent<ItemVariationsComponent>(selectedVariantComponent
                                .VariationId);

                        // Get variant Tax Code
                        if (variantItem.HasComponent<ProductTaxSettingsComponent>())
                        {
                            var productTaxSettingsComponent =
                                variantItem.GetComponent<ProductTaxSettingsComponent>();

                            // Get the taxcode and overide default with it.
                            if (!string.IsNullOrEmpty(productTaxSettingsComponent.TaxCode))
                            {
                                taxCode = productTaxSettingsComponent.TaxCode.Trim();
                            }


                        }
                    }

                }

                var lineTotal = cartLine.Totals.SubTotal.Amount;

                if (cartLine.Totals.AdjustmentsTotal.Amount != 0)
                {
                    lineTotal = lineTotal + cartLine.Totals.AdjustmentsTotal.Amount;
                }

                builder.WithLine(
                    lineTotal,
                    cartLine.Quantity,
                    taxCode, description, cartLine.ItemId, !string.IsNullOrEmpty(taxEntityUseCode) ? taxEntityUseCode : null);

                if (cartLevelDiscountAmount != 0)
                {
                    builder.WithItemDiscount(cartLevelDiscountAmount != 0);
                }


            }



            try
            {

                // Get shipping if tax 
                var shippingAdjustment = arg.Adjustments.FirstOrDefault(x => x.DisplayName == "FulfillmentFee");

                if (shippingAdjustment != null)
                {
                    builder.WithLine(shippingAdjustment.Adjustment.Amount + ShippingDiscountAmount, 1m,
                        string.IsNullOrEmpty(config.FreightCode)
                            ? Constants.Tax.DefaultFreightCode
                            : config.FreightCode, "Shipping", "Shipping");
                }

                if (cartLevelDiscountAmount != 0)
                {
                    var discountAmount = cartLevelDiscountAmount * -1;
                    builder.WithDiscountAmount(discountAmount);
                }

                // If there is an exemption number, include it.
                if (!string.IsNullOrEmpty(taxExcemptionNumber))
                {
                    builder.WithExemptionNumber(taxExcemptionNumber);
                }

                //builder.WithPurchaseOrderNumber(arg.OrderConfirmationId);

                //builder.WithReferenceCode(arg.OrderConfirmationId);

                builder.WithTransactionCode(arg.OrderConfirmationId);



                var transaction = builder.Create();

                // Now commit that transaction
                var commitResult = client.CommitTransaction(config.CompanyCode, transaction.code, null, new CommitTransactionModel() { commit = true });

                if (commitResult != null && commitResult.status == DocumentStatus.Committed)
                {
                    Log.Error("Transaction Logged", this);

                    // Get AvalaraTaxCacheEntity and clear it from storage
                    var avalaraTaxCacheEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxCacheEntity), $"avalaraTaxCache{arg.Id}", false), context) as AvalaraTaxCacheEntity;


                }
                else
                {
                    Log.Error("Transaction Not Logged", this);
                    context.Logger.LogError($"{this.Name}: Message=Transaction Not Logged");
                    await context.CommerceContext.AddMessage("Error", "DoActionConfigure.Run.Exception", new Object[] { this }, "Transaction Not Logged");
                }

            }
            catch (Exception ex)
            {
                context.Logger.LogError($"{this.Name}: Message={ex.Message}");
                await context.CommerceContext.AddMessage("Error", "DoActionConfigure.Run.Exception", new Object[] { ex }, ex.Message);
            }



            // return the new order so that it can be sent to the entities databse
            return arg;

        }
    }
}
