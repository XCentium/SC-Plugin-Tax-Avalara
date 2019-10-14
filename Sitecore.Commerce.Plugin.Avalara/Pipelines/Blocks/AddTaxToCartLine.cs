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
            Condition.Requires<CartLineArgument>(arg).IsNotNull<CartLineArgument>($"{(object) (this.Name)}: The argument cannot be null.");
            // ISSUE: explicit non-virtual call
            Condition.Requires<Cart>(arg.Cart).IsNotNull<Cart>($"{(object) (this.Name)}: The cart cannot be null.");
            // ISSUE: explicit non-virtual call
            Condition.Requires<CartLineComponent>(arg.Line).IsNotNull<CartLineComponent>($"{(object) (this.Name)}: The line to add cannot be null.");

            var cart = arg.Cart;
            var line = arg.Line;

            // get the 

            return Task.FromResult<Cart>(cart);

        }
    }
}
