using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Policies;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.DoActions
{
    public class DoActionEditProductTaxSettingsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        public DoActionEditProductTaxSettingsBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;

        }

        private readonly CommerceCommander _commerceCommander;


        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");


            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(Constants.View.AvalaraProductTaxSettingsView, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }


            // Get the sellable item from the context

            var entity = context.CommerceContext.GetObject<SellableItem>(x => x.Id.Equals(arg.EntityId));

            //var sellableItem = request.Entity as SellableItem;
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            // Get the productsettings component from the sellable item or its variation
            var component = entity.GetComponent<ProductTaxSettingsComponent>();
            //var component = entity.GetComponent<ProductTaxSettingsComponent>(arg.ItemId);

            // Map entity view properties to component
            component.TaxCode =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(component.TaxCode), StringComparison.OrdinalIgnoreCase))?.Value;

            // Persist changes
            _commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);


            return Task.FromResult(arg);
        }
    }
}
