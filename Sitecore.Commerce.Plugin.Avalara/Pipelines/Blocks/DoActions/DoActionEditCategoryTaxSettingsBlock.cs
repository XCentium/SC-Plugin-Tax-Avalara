using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.DoActions
{
    public class DoActionEditCategoryTaxSettingsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        public DoActionEditCategoryTaxSettingsBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        private readonly CommerceCommander _commerceCommander;


        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {

            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(Constants.View.AvalaraCategoryTaxSettingsView, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the category item from the context
            var entity = context.CommerceContext.GetObject<Category>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            // Get the component from the entity
            var component = entity.GetComponent<CategoryTaxSettingsComponent>();

            // Map entity view properties to component
            component.TaxGroup =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(CategoryTaxSettingsComponent.TaxGroup), StringComparison.OrdinalIgnoreCase))?.Value;

            // Persist changes
            _commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
