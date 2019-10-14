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

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.DoActions
{
    public class DoActionEditCustomerTaxSettingsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionEditCustomerTaxSettingsBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var actionsPolicy = context.GetPolicy<KnownCustomerTaxActionsPolicy>();


            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(Constants.View.AvalaraCustomerTaxSettingsView, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }


            // Get the customer entity from the context
            var entity = context.CommerceContext.GetObject<Customer>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            var customerDetailsComponent = entity.GetComponent<CustomerDetailsComponent>();

            // Get the customertaxsettings component from the customer item
            var component = customerDetailsComponent.GetComponent<CustomerTaxSettingComponent>();

            // Map entity view properties to component
            component.EntityUseCode =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(component.EntityUseCode), StringComparison.OrdinalIgnoreCase))?.Value;

            component.ExemptionNumber =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(component.ExemptionNumber), StringComparison.OrdinalIgnoreCase))?.Value;

            // Persist changes
            this._commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
