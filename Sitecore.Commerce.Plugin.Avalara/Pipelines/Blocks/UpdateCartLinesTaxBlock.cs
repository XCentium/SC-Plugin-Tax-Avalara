using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalara.AvaTax.RestClient;
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
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Commerce.Plugin.Tax;
using Sitecore.Diagnostics;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    public class UpdateCartLinesTaxBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {

        public UpdateCartLinesTaxBlock(IGetItemByPathPipeline getItemByPathPipeline, GetSellableItemCommand getSellableItemCommand, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline, GetEntityViewCommand getEntityViewCommand)
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
            await Task.Delay(1);
            return arg;

            //if (!arg.HasComponent<FulfillmentComponent>()) { return arg; }

            //if ((arg.GetComponent<FulfillmentComponent>() is SplitFulfillmentComponent)) { return arg; }


            ////if (!(arg.GetComponent<FulfillmentComponent>() is SplitFulfillmentComponent)) { return arg; }

            //// Get config from Entity
            //// var config = SitecoreItemHelper.GetConfiguration(context.CommerceContext, _getItemByPathPipeline);
            //var config = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1", false), context) as AvalaraTaxEntity;

            //// Abort if not found or Sitecore not available
            //if (config == null)
            //{
            //    return arg;
            //}

            //// If Avalara set to false/turned off, abort
            //if (config.Enabled == false)
            //{
            //   return arg;
            //}

            //var carlLines = arg.Lines.ToList();

            //// Flag to update tax calculation
            //var updateTaxCalculation = false;



            //// Get AvalaraTaxCacheEntity
            //var avalaraTaxCacheEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxCacheEntity), $"avalaraTaxCache{arg.Id}", false), context) as AvalaraTaxCacheEntity;

            //if (avalaraTaxCacheEntity == null)
            //{
            //    avalaraTaxCacheEntity = new AvalaraTaxCacheEntity
            //    {
            //        Id = $"avalaraTaxCache{arg.Id}",
            //        FriendlyId = $"avalaraTaxCache{arg.Id}"
            //    };

            //    updateTaxCalculation = true;
            //}
            //else
            //{
            //    // check if there has any changes since last created. 
            //    if (CartHasChanged(arg, avalaraTaxCacheEntity))
            //    {
            //        updateTaxCalculation = true;
            //    }

            //    // If cashed for more than 1 hour 
            //    var lastCreated = (DateTime.Now - avalaraTaxCacheEntity.WhenCreated).TotalHours;
            //    if (lastCreated > 1)
            //    {
            //        updateTaxCalculation = true;
            //    }

            //    if (!updateTaxCalculation)
            //    {
            //        var cachedLines = avalaraTaxCacheEntity.LinesTax;

            //        // loop through cart, if it has awarded adjustment, remove it and add a new one with cashed tax price
            //        foreach (var carlLine in carlLines)
            //        {

            //            // if cartline line has awarded
            //            // Check if tax adjustment has been applied
            //            var taxAdjustment = carlLine.Adjustments.FirstOrDefault(x => x.DisplayName == "TaxFee");
            //            if (taxAdjustment != null)
            //            {
            //                carlLine.Adjustments.Remove(taxAdjustment);

            //                // Get cachedLineTax from avalaraTaxCacheEntity
                           
            //                var cachedLine = cachedLines.FirstOrDefault(x=>x.LineItemId== carlLine.ItemId);
            //                var cachedLineTax = cachedLine?.LineTaxAmount ?? 0m;

            //                // Add new tax adjustment
            //                var awardedAdjustment = new CartLevelAwardedAdjustment
            //                {
            //                    Name = "TaxFee",
            //                    DisplayName = "TaxFee",
            //                    Adjustment = new Money(context.CommerceContext.CurrentCurrency(), cachedLineTax),
            //                    AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
            //                    AwardingBlock = this.Name,
            //                    IsTaxable = false
            //                };

            //                carlLine.Adjustments.Add((AwardedAdjustment)awardedAdjustment);

            //            }


            //        }

            //    }

            //    // If updateTaxCalculation = true , recalculate tax and cache result
            //    if (updateTaxCalculation)
            //    {

            //        var customer = arg.GetComponent<ContactComponent>();

            //        var globalPolicy = context.GetPolicy<GlobalTaxPolicy>();

            //        var taxExcemptionNumber = string.Empty;
            //        var taxEntityUseCode = string.Empty;

            //        if (customer.IsRegistered)
            //        {
            //            // Get the customer master entity view
            //            var entityView = await _getEntityViewCommand.Process(context.CommerceContext, customer.ShopperId, new int?(arg.EntityVersion), context.GetPolicy<KnownCustomerViewsPolicy>().Master, "", "");

            //            if (entityView != null && entityView.ChildViews.Any())
            //            {
            //                // if no discount field return arg
            //                var avalaraCustomerTaxSettingsView = entityView.ChildViews.FirstOrDefault(x => x.Name == Constants.View.AvalaraCustomerTaxSettingsView) as EntityView;
            //                if (avalaraCustomerTaxSettingsView != null)
            //                {
            //                    // if discount field is blank, return arg
            //                    var properties = avalaraCustomerTaxSettingsView.Properties;
            //                    var entityUseCode = properties.FirstOrDefault(x => x.Name == "EntityUseCode");
            //                    if (entityUseCode != null)
            //                    {
            //                        taxEntityUseCode = entityUseCode.Value;
            //                    }
            //                    var excemptionNumber = properties.FirstOrDefault(x => x.Name == "ExemptionNumber");
            //                    if (excemptionNumber != null)
            //                    {
            //                        taxExcemptionNumber = excemptionNumber.Value;
            //                    }
            //                }
            //            }
            //        }


            //        // Build the client
            //        var client = new AvaTaxClient(config.AppName, config.AppVersion, Environment.MachineName, config.InProductionMode ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
            //            .WithSecurity(config.UserName, config.Password);



            //        var builder = new TransactionBuilder(client, config.CompanyCode, DocumentType.SalesOrder,
            //            string.IsNullOrEmpty(customer.CustomerId) ? customer.Id : customer.CustomerId);



            //        // Get all line
            //        var cartLines = arg.Lines.ToList();

            //        // Loop through lines and prepare request for Avalara
            //        foreach (var cartLine in cartLines)
            //        {


            //            var taxCode = Constants.Tax.DefaultTaxCode;

            //            var cartProductComponent = cartLine.GetComponent<CartProductComponent>();


            //            var description = cartProductComponent.DisplayName;

            //            // Get the sellableItem entity
            //            var sellableItem = await _getSellableItemCommand.Process(context.CommerceContext, cartLine.ItemId, false);
            //            //var c = sellableItem.Components.FirstOrDefault(x => x.Name == "ProductTaxSettingsComponent");
            //            if (sellableItem != null)
            //            {
            //                // Get SellableItem Tax Code
            //                if (sellableItem.HasComponent<ProductTaxSettingsComponent>())
            //                {
            //                    var productTaxSettingsComponent = sellableItem.GetComponent<ProductTaxSettingsComponent>();

            //                    // Get the taxcode and overide default with it.
            //                    if (!string.IsNullOrEmpty(productTaxSettingsComponent.TaxCode))
            //                    {
            //                        taxCode = productTaxSettingsComponent.TaxCode.Trim();
            //                    }


            //                }

            //                // if the sellable item has variant, get the variant
            //                if (sellableItem.HasComponent<ItemVariationsComponent>())
            //                {
            //                    var selectedVariantComponent = cartLine.GetComponent<ItemVariationSelectedComponent>();

            //                    var variantItem =
            //                        sellableItem.GetComponent<ItemVariationsComponent>(selectedVariantComponent
            //                            .VariationId);

            //                    // Get variant Tax Code
            //                    if (variantItem.HasComponent<ProductTaxSettingsComponent>())
            //                    {
            //                        var productTaxSettingsComponent =
            //                            variantItem.GetComponent<ProductTaxSettingsComponent>();

            //                        // Get the taxcode and overide default with it.
            //                        if (!string.IsNullOrEmpty(productTaxSettingsComponent.TaxCode))
            //                        {
            //                            taxCode = productTaxSettingsComponent.TaxCode.Trim();
            //                        }

            //                    }
            //                }

            //            }


            //            if (globalPolicy.TaxExemptTagsEnabled)
            //            {
            //                var exemptag = cartProductComponent.Tags.FirstOrDefault(x => x.Name == globalPolicy.TaxExemptTag);
            //                if (exemptag != null)
            //                {
            //                    taxCode = Constants.Tax.DefaultTaxExcemptCode;
            //                }
            //            }



            //            if (cartLine.GetComponent<FulfillmentComponent>() is PhysicalFulfillmentComponent)
            //            {
            //                var fulfillmentComponent = cartLine.GetComponent<PhysicalFulfillmentComponent>();

            //                var shippingParty = fulfillmentComponent?.ShippingParty;

            //                if (shippingParty == null) { return arg; }

            //                builder.WithLine(
            //                    cartLine.Totals.GrandTotal.Amount,
            //                    cartLine.Quantity,
            //                    taxCode, description, cartLine.ItemId, !string.IsNullOrEmpty(taxEntityUseCode) ? taxEntityUseCode : null);

            //                builder.WithLineAddress(TransactionAddressType.ShipTo, shippingParty.Address1,
            //                    shippingParty.Address2, null, shippingParty.City, shippingParty.StateCode,
            //                    shippingParty.ZipPostalCode, shippingParty.CountryCode);

            //                if (cartLine.Totals.AdjustmentsTotal.Amount != 0)
            //                {
            //                    builder.WithItemDiscount(cartLine.Totals.AdjustmentsTotal.Amount != 0);
            //                    var discountAmount = cartLine.Totals.AdjustmentsTotal.Amount * -1;
            //                    builder.WithDiscountAmount(discountAmount);
            //                }

            //                //builder.WithLine()
            //            }
            //            else
            //            {
            //                if (cartLine.GetComponent<FulfillmentComponent>() is ElectronicFulfillmentComponent)
            //                {
            //                    var fulfillmentComponent = cartLine.GetComponent<ElectronicFulfillmentComponent>();

            //                    builder.WithLine(
            //                        cartLine.Totals.GrandTotal.Amount,
            //                        cartLine.Quantity,
            //                        taxCode, description, cartLine.ItemId, !string.IsNullOrEmpty(taxEntityUseCode) ? taxEntityUseCode : null);

            //                    builder.WithEmail(fulfillmentComponent.EmailAddress);

            //                }
            //                else
            //                {
            //                    builder.WithLine(
            //                        cartLine.Totals.GrandTotal.Amount,
            //                        cartLine.Quantity,
            //                        taxCode, description, cartLine.ItemId, !string.IsNullOrEmpty(taxEntityUseCode) ? taxEntityUseCode : null);

            //                }

            //                if (cartLine.Totals.AdjustmentsTotal.Amount != 0)
            //                {
            //                    builder.WithItemDiscount(cartLine.Totals.AdjustmentsTotal.Amount != 0);
            //                    var discountAmount = cartLine.Totals.AdjustmentsTotal.Amount * -1;
            //                    builder.WithDiscountAmount(discountAmount);
            //                }

            //            }

            //        }


            //        try
            //        {

            //            // Get shipping if tax 
            //            var shippingAdjustment = arg.Adjustments.FirstOrDefault(x => x.DisplayName == "FulfillmentFee");

            //            if (shippingAdjustment != null)
            //            {
            //                builder.WithLine(shippingAdjustment.Adjustment.Amount, 1m,
            //                    string.IsNullOrEmpty(config.FreightCode)
            //                        ? Constants.Tax.DefaultFreightCode
            //                        : config.FreightCode);
            //            }

            //            // If there is an exemption number, include it.
            //            if (!string.IsNullOrEmpty(taxExcemptionNumber))
            //            {
            //                builder.WithExemptionNumber(taxExcemptionNumber);
            //            }


            //            var transaction = builder.Create();

            //            var taxValue = transaction.totalTax ?? 0m;

            //            // cache the response
            //            // Update Avalara Cache Entity

            //            avalaraTaxCacheEntity.LineCount = arg.Lines.Count;
            //            avalaraTaxCacheEntity.TaxTotal = (decimal)taxValue;
            //            avalaraTaxCacheEntity.LineTotal = arg.Totals.SubTotal.Amount;
            //            avalaraTaxCacheEntity.LinesTax = new List<LineTaxRecord>();
            //            avalaraTaxCacheEntity.WhenCreated = DateTime.Now;


            //            var responseLines = transaction.lines.ToList();

            //            foreach (var cartLine in cartLines)
            //            {
            //                var responseLine = responseLines.FirstOrDefault(x => x.itemCode == cartLine.ItemId);
            //                if (responseLine != null)
            //                {


            //                    var lineTaxRecord = new LineTaxRecord
            //                    {
            //                        LineItemId = cartLine.ItemId,
            //                        LineQuantity = cartLine.Quantity,
            //                        LineTotal = cartLine.Totals.SubTotal.Amount,
            //                        LineTaxAmount = responseLine?.tax ?? 0m
            //                    };

            //                    avalaraTaxCacheEntity.LinesTax.Add(lineTaxRecord);

            //                    // if cartline line has awarded
            //                    // Check if tax adjustment has been applied
            //                    var taxAdjustment = cartLine.Adjustments.FirstOrDefault(x => x.DisplayName == "TaxFee");
            //                    if (taxAdjustment != null)
            //                    {
            //                        cartLine.Adjustments.Remove(taxAdjustment);


            //                        // Add new tax adjustment
            //                        var awardedAdjustment = new CartLevelAwardedAdjustment
            //                        {
            //                            Name = "TaxFee",
            //                            DisplayName = "TaxFee",
            //                            Adjustment = new Money(context.CommerceContext.CurrentCurrency(), (decimal)responseLine.tax),
            //                            AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
            //                            AwardingBlock = this.Name,
            //                            IsTaxable = false
            //                        };

            //                        cartLine.Adjustments.Add((AwardedAdjustment)awardedAdjustment);

            //                    }
            //                }

                           

            //            }

            //            // Persist the entity
            //            var persistEntityArgument =
            //                await this._persistEntityPipeline.Run(
            //                    new PersistEntityArgument((CommerceEntity)avalaraTaxCacheEntity), context);



            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex.Message, this);
            //        }


            //    }

            //}



            //return arg;
        }


        private static bool CartHasChanged(Cart arg, AvalaraTaxCacheEntity avalaraTaxCacheEntity)
        {
            var carlLines = arg.Lines.ToList();

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
