// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks;
using Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.DoActions;
using Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks.EntityViews;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Tax;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Sitecore.Commerce.Plugin.Avalara
{
    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<ICalculateCartLinesPipeline>(
                    d => { d.Add<UpdateCartLinesTaxBlock>().After<CalculateCartLinesTaxBlock>(); })
                .ConfigurePipeline<ICalculateCartPipeline>(
                    d => { d.Add<UpdateCartTaxBlock>().After<CalculateCartTaxBlock>(); })
                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c.Add<GetCategoryTaxSettingsViewBlock>().After<GetCategoryDetailsViewBlock>();
                    c.Add<GetProductTaxSettingsViewBlock>().After<GetSellableItemDetailsViewBlock>();
                    c.Add<GetVariantTaxSettingsViewBlock>().After<GetSellableItemVariantDetailsViewBlock>();
                    c.Add<GetCustomerTaxSettingsViewBlock>().After<GetCustomerDetailsViewBlock>();
                })
                .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                {
                    c.Add<PopulateCategoryTaxSettingsActionsBlock>().After<InitializeEntityViewActionsBlock>();
                    c.Add<PopulateProductTaxSettingsActionsBlock>().After<InitializeEntityViewActionsBlock>();
                    c.Add<PopulateVariantTaxSettingsActionsBlock>().After<InitializeEntityViewActionsBlock>();
                    c.Add<PopulateCustomerTaxSettingsActionsBlock>().After<InitializeEntityViewActionsBlock>();
                })
                .ConfigurePipeline<IDoActionPipeline>(c =>
                {
                    c.Add<DoActionEditCategoryTaxSettingsBlock>().After<ValidateEntityVersionBlock>();
                    c.Add<DoActionEditProductTaxSettingsBlock>().After<ValidateEntityVersionBlock>();
                    c.Add<DoActionEditVariantTaxSettingsBlock>().After<ValidateEntityVersionBlock>();
                    c.Add<DoActionEditCustomerTaxSettingsBlock>().After<ValidateEntityVersionBlock>();
                })
                .ConfigurePipeline<IPersistOrderPipeline>(
                    d => { d.Add<CommitAvalaraTaxBlock>().Before<PersistOrderBlock>(); })
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>())
            );

            services.RegisterAllCommands(assembly);
        }
    }
}