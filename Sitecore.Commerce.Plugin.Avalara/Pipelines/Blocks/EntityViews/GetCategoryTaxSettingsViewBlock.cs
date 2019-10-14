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
    public class GetCategoryTaxSettingsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        public GetCategoryTaxSettingsViewBlock(ViewCommander viewCommander)
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

                var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectCategory, StringComparison.OrdinalIgnoreCase);
                var isMasterView = arg.Name.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);

                var targetView = arg;

                // Make sure that we target the correct views
                if (string.IsNullOrEmpty(request.ViewName) ||
                    !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(catalogViewsPolicy.Categories, StringComparison.OrdinalIgnoreCase) &&
                    !request.ViewName.Equals(Constants.View.AvalaraCategoryTaxSettingsView, StringComparison.OrdinalIgnoreCase) &&
                    !isConnectView)
                {
                    return Task.FromResult(arg);
                }

                // Only proceed if the current entity is a Category
                var catalogItem = request.Entity as Category;
                if (catalogItem == null)
                {
                    return Task.FromResult(arg);
                }



                // Check if the edit action was requested
                var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(Constants.View.AvalaraCategoryTaxSettingsView, StringComparison.OrdinalIgnoreCase);
                if (!isEditView)
                {
                    // Create a new view and add it to the current entity view.
                    var view = new EntityView
                    {
                        Name = Constants.View.AvalaraCategoryTaxSettingsView,
                        DisplayName = "Avalara Tax Group",
                        EntityId = arg.EntityId,
                        ItemId = string.Empty,
                        EntityVersion = catalogItem.EntityVersion
                    };

                    arg.ChildViews.Add(view);

                    targetView = view;
                }

                if (!catalogItem.HasComponent<CategoryTaxSettingsComponent>())
                {
                    var catalogComponent = new CategoryTaxSettingsComponent
                    {
                        TaxGroup = string.Empty
                    };

                    catalogItem.SetComponent((Component)catalogComponent);

                }

                if (isConnectView || isEditView || isMasterView)
                {
                    var component = catalogItem.GetComponent<CategoryTaxSettingsComponent>();
                    AddPropertiesToView(targetView, component, !isEditView);
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, $"GetCategoryTaxSettingsViewBlock: {ex.Message}");
                throw;
            }


            return Task.FromResult(arg);
        }

        private void AddPropertiesToView(EntityView entityView, CategoryTaxSettingsComponent component, bool isReadOnly)
        {
            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(component.TaxGroup),
                    DisplayName = "Tax Group",
                    RawValue = component?.TaxGroup,
                    IsReadOnly = isReadOnly,
                    IsRequired = false
                });


        }
    }
}
