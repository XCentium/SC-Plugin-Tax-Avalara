using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Policies;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.EntityViews
{
    public class GetCustomerTaxSettingsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetCustomerTaxSettingsViewBlock(ViewCommander viewCommander)
        {
            _viewCommander = viewCommander;
        }
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {

            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            var customerViewsPolicy = context.GetPolicy<KnownCustomerViewsPolicy>();

            var viewsPolicy = context.GetPolicy<KnownCustomerTaxSettingsViewsPolicy>();
            var actionsPolicy = context.GetPolicy<KnownCustomerTaxActionsPolicy>();

            var isSummaryView = arg.Name.Equals(customerViewsPolicy.Summary, StringComparison.OrdinalIgnoreCase);
            var isDetailtView = arg.Name.Equals(customerViewsPolicy.Details, StringComparison.OrdinalIgnoreCase);
            var isMasterView = arg.Name.Equals(customerViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);
           // var isCustomerSettingView = arg.Name.Equals("CustomerAvalaraTaxSettings", StringComparison.OrdinalIgnoreCase);
            //"CustomerAvalaraTaxSettings"

            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(customerViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(customerViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(Constants.View.AvalaraCustomerTaxSettingsView, StringComparison.OrdinalIgnoreCase)

                )
            {
                return Task.FromResult(arg);
            }

            // Only proceed if the current entity is a sellable item
            if (!(request.Entity is Customer))
            {
                return Task.FromResult(arg);
            }

            var customer = (Customer)request.Entity;

            var targetView = arg;

            // Check if the edit action was requested
            var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(Constants.View.AvalaraCustomerTaxSettingsView, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = Constants.View.AvalaraCustomerTaxSettingsView,
                    DisplayName = "Avalara Customer Tax Settings",
                    EntityId = arg.EntityId,
                    ItemId = arg.ItemId,
                    EntityVersion = customer.EntityVersion
                };

                arg.ChildViews.Add(view);

                targetView = view;
            }

            if (!customer.HasComponent<CustomerDetailsComponent>())
            {
                return Task.FromResult(arg);
            }

            var customerDetailsComponent = customer.GetComponent<CustomerDetailsComponent>();

            if (!customerDetailsComponent.HasComponent<CustomerTaxSettingComponent>())
            {
                var component = new CustomerTaxSettingComponent
                {
                    EntityUseCode = string.Empty,
                    ExemptionNumber = string.Empty
                    
                };

                customerDetailsComponent.SetComponent((Component)component);

            }


            if (isSummaryView || isMasterView || isDetailtView || isEditView)
            {
                var component = customerDetailsComponent.GetComponent<CustomerTaxSettingComponent>();
                AddPropertiesToView(targetView, component, !isEditView);
            }

            return Task.FromResult(arg);
        }


        private void AddPropertiesToView(EntityView entityView, CustomerTaxSettingComponent component, bool isReadOnly)
        {
            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(component.EntityUseCode),
                    DisplayName = "Entity Use Code",
                    RawValue = component?.EntityUseCode,
                    IsReadOnly = isReadOnly,
                    IsRequired = false
                });

            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(component.ExemptionNumber),
                    DisplayName = "Exemption Number",
                    RawValue = component?.ExemptionNumber,
                    IsReadOnly = isReadOnly,
                    IsRequired = false
                });


        }

    }
}
