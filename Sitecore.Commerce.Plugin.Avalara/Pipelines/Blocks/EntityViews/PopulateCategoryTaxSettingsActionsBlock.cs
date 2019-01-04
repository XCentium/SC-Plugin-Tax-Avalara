﻿using System;
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
    public class PopulateCategoryTaxSettingsActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");


            try
            {

                if (string.IsNullOrEmpty(arg?.Name) ||
                    !arg.Name.Equals(Constants.View.AvalaraCategoryTaxSettingsView, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(arg);
                }

                var actionPolicy = arg.GetPolicy<ActionsPolicy>();

                actionPolicy.Actions.Add(
                    new EntityActionView
                    {
                        Name = Constants.View.AvalaraCategoryTaxSettingsView,
                        DisplayName = "Edit Avalara Tax Group",
                        Description = "Edit Avalara Tax Group",
                        IsEnabled = true,
                        EntityView = arg.Name,
                        Icon = "edit"
                    });

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"PopulateCategoryTaxSettings: {ex.Message}");
                throw;
            }



            return Task.FromResult(arg);
        }
    }
}