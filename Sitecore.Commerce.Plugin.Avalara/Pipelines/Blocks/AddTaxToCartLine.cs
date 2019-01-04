using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    public class AddTaxToCartLine : PipelineBlock<CartLineArgument, Cart, CommercePipelineExecutionContext>
    {
        public override Task<Cart> Run(CartLineArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires<CartLineArgument>(arg).IsNotNull<CartLineArgument>(string.Format("{0}: The argument cannot be null.", (object)(this.Name)));
            // ISSUE: explicit non-virtual call
            Condition.Requires<Cart>(arg.Cart).IsNotNull<Cart>(string.Format("{0}: The cart cannot be null.", (object)(this.Name)));
            // ISSUE: explicit non-virtual call
            Condition.Requires<CartLineComponent>(arg.Line).IsNotNull<CartLineComponent>(string.Format("{0}: The line to add cannot be null.", (object)(this.Name)));

            var cart = arg.Cart;
            var line = arg.Line;

            // get the 

            return Task.FromResult<Cart>(cart);

        }
    }
}
