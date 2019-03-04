using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalara.AvaTax.RestClient;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Avalara.Models;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    public class UpdateCartTaxBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        public UpdateCartTaxBlock(IGetItemByPathPipeline getItemByPathPipeline, GetSellableItemCommand getSellableItemCommand, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline, GetEntityViewCommand getEntityViewCommand)
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

        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            if (!arg.HasComponent<FulfillmentComponent>()) { return arg; }

            if (arg.GetComponent<FulfillmentComponent>() is SplitFulfillmentComponent){return arg;}

            // Flag to update tax calculation
            var updateTaxCalculation = false;

            // Check if tax adjustment has been applied
            var taxAdjustment = arg.Adjustments.FirstOrDefault(x => x.DisplayName == "TaxFee");

            //If no tax adjustment return
            if (taxAdjustment == null) { return arg; }

            // Get config from Entity
            // var config = SitecoreItemHelper.GetConfiguration(context.CommerceContext, _getItemByPathPipeline);
            var config = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), Constants.Tax.AvalaraTaxConfig, false), context) as AvalaraTaxEntity;

            // Abort if not found or Sitecore not available
            if (config == null)
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

            // Get AvalaraTaxCacheEntity
            var avalaraTaxCacheEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxCacheEntity), $"avalaraTaxCache{arg.Id}", false), context) as AvalaraTaxCacheEntity;

            // Remove tax adjustment
            arg.Adjustments.Remove(taxAdjustment);

            if (avalaraTaxCacheEntity == null)
            {
                avalaraTaxCacheEntity = new AvalaraTaxCacheEntity
                {
                    Id = $"avalaraTaxCache{arg.Id}",
                    FriendlyId = $"avalaraTaxCache{arg.Id}"
                };

                updateTaxCalculation = true;
            }
            else
            {
                // check if there has any changes since last created. 
                if (CartHasChanged(arg, avalaraTaxCacheEntity, shippingParty))
                {
                    updateTaxCalculation = true;
                }

                // If cashed for more than 1 hour 
                var lastCreated = (DateTime.Now - avalaraTaxCacheEntity.WhenCreated).TotalHours;
                if (lastCreated > 1)
                {
                    updateTaxCalculation = true;
                }

                if (!updateTaxCalculation)
                {



                    // Add new tax adjustment
                    var awardedAdjustment = new CartLevelAwardedAdjustment
                    {
                        Name = "TaxFee",
                        DisplayName = "TaxFee",
                        Adjustment = new Money(context.CommerceContext.CurrentCurrency(), (decimal)avalaraTaxCacheEntity.TaxTotal),
                        AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                        AwardingBlock = this.Name,
                        IsTaxable = false
                    };

                    arg.Adjustments.Add((AwardedAdjustment)awardedAdjustment);
                }

            }



            // If updateTaxCalculation = true , recalculate tax and cache result
            if (updateTaxCalculation)
            {

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



                var builder = new TransactionBuilder(client, config.CompanyCode, DocumentType.SalesOrder, string.IsNullOrEmpty(customer.CustomerId)? customer.Id: customer.CustomerId)
                    .WithAddress(TransactionAddressType.SingleLocation, shippingParty.Address1, shippingParty.Address2, null, shippingParty.City, shippingParty.StateCode,
                        shippingParty.ZipPostalCode, shippingParty.CountryCode);

                // Get all line
                var cartLines = arg.Lines.ToList();

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
                        taxCode, description, cartLine.ItemId, !string.IsNullOrEmpty(taxEntityUseCode)? taxEntityUseCode : null);

                    if (cartLevelDiscountAmount != 0)
                    {
                        builder.WithItemDiscount(cartLevelDiscountAmount != 0);
                    }



                }

                decimal? taxValue = 0m;

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


                    var transaction = builder.Create();

                    taxValue = transaction.totalTax ?? 0m;

                    // Update Avalara Cache Entity

                    avalaraTaxCacheEntity.LineCount = arg.Lines.Count;
                    avalaraTaxCacheEntity.TaxTotal = (decimal) taxValue;
                    avalaraTaxCacheEntity.LineTotal = arg.Totals.SubTotal.Amount;
                    avalaraTaxCacheEntity.LinesTax = new List<LineTaxRecord>();
                    avalaraTaxCacheEntity.WhenCreated = DateTime.Now;


                    foreach (var cartLine in cartLines)
                    {
                        var lineTaxRecord = new LineTaxRecord
                        {
                            LineItemId = cartLine.ItemId,
                            LineQuantity = cartLine.Quantity,
                            LineTotal = cartLine.Totals.SubTotal.Amount
                        };
                        avalaraTaxCacheEntity.LinesTax.Add(lineTaxRecord);

                    }

                    avalaraTaxCacheEntity.Address1 = shippingParty.Address1;
                    avalaraTaxCacheEntity.StateCode = shippingParty.StateCode;
                    avalaraTaxCacheEntity.ZipPostalCode = shippingParty.ZipPostalCode;

                    // Persist the entity
                    var persistEntityArgument =
                        await this._persistEntityPipeline.Run(
                            new PersistEntityArgument((CommerceEntity) avalaraTaxCacheEntity), context);



                }
                catch (Exception ex)
                {

                    context.Logger.LogError($"{this.Name}: Message={ex.Message}");
                    await context.CommerceContext.AddMessage("Error", "DoActionConfigure.Run.Exception", new Object[] { ex }, ex.Message);
                }


                // Add new tax adjustment
                var awardedAdjustment = new CartLevelAwardedAdjustment
                {
                    Name = "TaxFee",
                    DisplayName = "TaxFee",
                    Adjustment = new Money(context.CommerceContext.CurrentCurrency(), (decimal)taxValue),
                    AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                    AwardingBlock = this.Name,
                    IsTaxable = false
                };

                arg.Adjustments.Add((AwardedAdjustment)awardedAdjustment);



            }


            return arg;
        }

        private static bool CartHasChanged(Cart arg, AvalaraTaxCacheEntity avalaraTaxCacheEntity, Party shippingParty)
        {
            var carlLines = arg.Lines.ToList();

            
            // if shipping address has changed, return true
            if (shippingParty.Address1 != avalaraTaxCacheEntity.Address1) { return true; }
            if (shippingParty.StateCode != avalaraTaxCacheEntity.StateCode) { return true; }
            if (shippingParty.ZipPostalCode != avalaraTaxCacheEntity.ZipPostalCode) { return true; }

            // if cart line count has changed, return true
            if (carlLines.Count != avalaraTaxCacheEntity.LineCount) { return true; }

            // if cart total has changed, return true
            if (arg.Totals.SubTotal.Amount != avalaraTaxCacheEntity.LineTotal) { return true; }

            // loop through cartLines
            // check if cartline not found return true
            // if cartline quantity has change, return true
            // if cartline total has changed, return true
            var cachedLines = avalaraTaxCacheEntity.LinesTax.ToList();
            foreach (var carlLine in carlLines)
            {
                var cachedLine = cachedLines.FirstOrDefault(x => x.LineItemId == carlLine.ItemId);

                if (cachedLine == null) { return true; }

                if (cachedLine.LineQuantity != carlLine.Quantity) { return true; }

                if (carlLine.Totals.SubTotal.Amount != cachedLine.LineTotal) { return true; }
            }



            return false;
        }

    }
}
