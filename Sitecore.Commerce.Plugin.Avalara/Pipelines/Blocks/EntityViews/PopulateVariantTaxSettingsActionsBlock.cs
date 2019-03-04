using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.EntityViews
{
    public class PopulateVariantTaxSettingsActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: {Sitecore.Commerce.Plugin.Avalara.Constants.Tax.ArgumentNullText}");

            try
            {

                if (string.IsNullOrEmpty(arg?.Name) ||
                    !arg.Name.Equals(Constants.View.AvalaraVariantTaxSettingsView, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(arg);
                }

                var actionPolicy = arg.GetPolicy<ActionsPolicy>();

                actionPolicy.Actions.Add(
                    new EntityActionView
                    {
                        Name = Constants.View.AvalaraVariantTaxSettingsView,
                        DisplayName = "Edit Variant Item Avalara Tax Settings",
                        Description = "Edits the variant item Avalara tax settings",
                        IsEnabled = true,
                        EntityView = arg.Name,
                        Icon = "edit"
                    });

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"PopulateVariantTaxSettingsActionsBlock: {ex.Message}");
                throw;
            }

            return Task.FromResult(arg);
        }
    }
}
