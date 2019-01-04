using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Policies;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.EntityViews
{
    public class GetProductTaxSettingsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        public GetProductTaxSettingsViewBlock(ViewCommander viewCommander)
        {
            _viewCommander = viewCommander;
        }

        private readonly ViewCommander _viewCommander;


        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            try

            {

                var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);
                var isMasterView = arg.Name.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);

                var isVariationView = request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
                var isSellableItemView = request.ViewName.Equals(catalogViewsPolicy.SellableItem, StringComparison.OrdinalIgnoreCase);

                //var ismasterView = arg.Name.Equals(catalogViewsPolicy.SellableItem, StringComparison.OrdinalIgnoreCase) && request.EntityId.ToLower().Contains("sellableitem");


                var targetView = arg;


                // Make sure that we target the correct views
                if (string.IsNullOrEmpty(request.ViewName) ||
                    !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.SellableItem, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(Constants.View.AvalaraProductTaxSettingsView, StringComparison.OrdinalIgnoreCase) &&
                    !isConnectView)
                {
                    return Task.FromResult(arg);
                }


                // Only proceed if the current entity is a sellable item
                var sellableItem = request.Entity as SellableItem;
                if (sellableItem==null)
                {
                    return Task.FromResult(arg);
                }

                var variationId = string.Empty;



                // Check if the edit action was requested
                var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(Constants.View.AvalaraProductTaxSettingsView, StringComparison.OrdinalIgnoreCase);
                if (!isEditView)
                {
                    // Create a new view and add it to the current entity view.
                    var view = new EntityView
                    {
                        Name = Constants.View.AvalaraProductTaxSettingsView,
                        DisplayName = "Avalara Product Tax Settings",
                        EntityId = arg.EntityId,
                        ItemId = string.Empty,
                        EntityVersion = sellableItem.EntityVersion
                    };

                    arg.ChildViews.Add(view);

                    targetView = view;
                }

                if (!sellableItem.HasComponent<ProductTaxSettingsComponent>(variationId))
                {
                    var productComponent = new ProductTaxSettingsComponent
                    {
                        TaxCode = string.Empty
                    };

                    sellableItem.SetComponent((Component)productComponent);

                }


               // (sellableItem.HasComponent<ProductTaxSettingsComponent>(variationId);

                if (isConnectView || isEditView || isMasterView)
                {
                    var component = sellableItem.GetComponent<ProductTaxSettingsComponent>(variationId);
                    AddPropertiesToView(targetView, component, !isEditView);
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, $"GetProductTaxSettingsViewBlock: {ex.Message}");
                throw;
            }




/*

            //================================================

            var viewsPolicy = context.GetPolicy<KnownProductTaxSettingsViewsPolicy>();
            var actionsPolicy = context.GetPolicy<KnownProductsTaxActionsPolicy>();



            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(viewsPolicy.ProductSettings, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }



            // See if we are dealing with the base sellable item or one of its variations.
            //var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
            }


            // Check if the edit action was requested
            var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(actionsPolicy.EditProductTaxSettings, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<KnownProductTaxSettingsViewsPolicy>().ProductSettings,
                    DisplayName = "Avalara Tax Settings",
                    EntityId = arg.EntityId,
                    ItemId = variationId
                };

                arg.ChildViews.Add(view);

                targetView = view;
            }

            if (sellableItem != null && (sellableItem.HasComponent<ProductTaxSettingsComponent>(variationId) || isConnectView || isEditView || isVariationView || ismasterView) )
            {
                var component = sellableItem.GetComponent<ProductTaxSettingsComponent>(variationId);
                AddPropertiesToView(targetView, component, !isEditView);
            }
*/
            return Task.FromResult(arg);
        }

        private void AddPropertiesToView(EntityView entityView, ProductTaxSettingsComponent component, bool isReadOnly)
        {
            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(component.TaxCode),
                    DisplayName = "Tax Code",
                    RawValue = component.TaxCode,
                    IsReadOnly = isReadOnly,
                    IsRequired = false
                });

        }
    }
}
