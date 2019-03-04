using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateCalculateCartLinesTaxBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {

        private JsonSerializerSettings _serializerSettings = null;
        private JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (_serializerSettings != null) return _serializerSettings;
                lock (this)
                {
                    _serializerSettings = new JsonSerializerSettings();
                    _serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    _serializerSettings.Converters.Add(new StringEnumConverter());
                }
                return _serializerSettings;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UpdateCalculateCartLinesTaxBlock()
            : base(null)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public static Party ShippingParty { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {

            Condition.Requires(arg).IsNotNull(string.Format("{0}: {1}", Name, Constants.Tax.CartNullText));
            Condition.Requires(arg.Lines).IsNotNull(string.Format("{0}: {1}", Name, Constants.Tax.CartLineNullText));

            // get all lines that have fulfillment methods applied
            var list = arg.Lines.Where(line =>
            {
                if (line != null)
                    return line.HasComponent<FulfillmentComponent>();
                return false;
            }).Select(l => l).ToList();
            if (!list.Any())
            {
                return await Task.FromResult(arg);
            }

            return await Task.FromResult(arg);
        }



    }
}
