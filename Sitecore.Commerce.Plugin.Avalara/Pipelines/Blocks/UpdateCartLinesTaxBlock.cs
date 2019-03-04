using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalara.AvaTax.RestClient;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.EntityViews.Commands;
using Sitecore.Commerce.Plugin.Avalara.Components;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Avalara.Models;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Commerce.Plugin.Tax;
using Sitecore.Diagnostics;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    public class UpdateCartLinesTaxBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {

        public UpdateCartLinesTaxBlock(IGetItemByPathPipeline getItemByPathPipeline, GetSellableItemCommand getSellableItemCommand, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline, GetEntityViewCommand getEntityViewCommand)
        {
            _getItemByPathPipeline = getItemByPathPipeline;
            _getSellableItemCommand = getSellableItemCommand;

            _findEntity = findEntityPipeline;
            _persistEntityPipeline = persistEntityPipeline;
            _getEntityViewCommand = getEntityViewCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly GetSellableItemCommand _getSellableItemCommand;

        /// <summary>
        /// 
        /// </summary>
        private readonly IFindEntityPipeline _findEntity;

        /// <summary>
        /// 
        /// </summary>
        private readonly IGetItemByPathPipeline _getItemByPathPipeline;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        /// <summary>
        /// 
        /// </summary>
        private readonly GetEntityViewCommand _getEntityViewCommand;
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            await Task.Delay(1);
            return arg;

        }

    }
}
