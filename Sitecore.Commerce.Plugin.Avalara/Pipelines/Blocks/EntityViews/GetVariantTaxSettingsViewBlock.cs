using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.EntityViews
{
    public class GetVariantTaxSettingsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public GetVariantTaxSettingsViewBlock(ViewCommander viewCommander)
        {
            _viewCommander = viewCommander;
        }

        private readonly ViewCommander _viewCommander;


        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: {Sitecore.Commerce.Plugin.Avalara.Constants.Tax.ArgumentNullText}");

            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            try

            {

                var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);
                var isMasterView = arg.Name.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);

                var isVariationView = request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
                var isSellableItemView = request.ViewName.Equals(catalogViewsPolicy.SellableItem, StringComparison.OrdinalIgnoreCase);

                var targetView = arg;


                // Make sure that we target the correct views
                if (string.IsNullOrEmpty(request.ViewName) ||
                   // !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(Constants.View.AvalaraVariantTaxSettingsView, StringComparison.OrdinalIgnoreCase) &&
                    !isConnectView)
                {
                    return Task.FromResult(arg);
                }


                // Only proceed if the current entity is a sellable item
                var sellableItem = request.Entity as SellableItem;
                if (sellableItem == null)
                {
                    return Task.FromResult(arg);
                }

                var variationId = arg.ItemId;


                if (!sellableItem.HasComponent<ItemVariationsComponent>(variationId))
                {
                    return Task.FromResult(arg);
                }

                var variantItem = sellableItem.GetComponent<ItemVariationsComponent>(variationId);




                // Check if the edit action was requested
                var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(Constants.View.AvalaraVariantTaxSettingsView, StringComparison.OrdinalIgnoreCase);
                if (!isEditView)
                {
                    // Create a new view and add it to the current entity view.
                    var view = new EntityView
                    {
                        Name = Constants.View.AvalaraVariantTaxSettingsView,
                        DisplayName = "Avalara Variant Tax Settings",
                        EntityId = arg.EntityId,
                        ItemId = arg.ItemId
                    };

                    arg.ChildViews.Add(view);

                    targetView = view;
                }



                if (!variantItem.HasComponent<ProductTaxSettingsComponent>())
                {
                    var variantComponent = new ProductTaxSettingsComponent
                    {
                        TaxCode = string.Empty
                    };

                    variantItem.SetComponent((Component) variantComponent);

                }


                if (isConnectView || isEditView || isVariationView)
                {
                    var component = variantItem.GetComponent<ProductTaxSettingsComponent>();
                    AddPropertiesToView(targetView, component, !isEditView);
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, $"GetVariantTaxSettingsViewBlock: {ex.Message}");
                throw;
            }

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
