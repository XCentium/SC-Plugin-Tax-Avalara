using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Rules;

namespace Sitecore.Commerce.Plugin.Avalara.Pipelines.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateCalculateCartTaxBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {

        private readonly IRuleCreator _ruleCreator;
        private readonly IRuleEngine _ruleEngine;

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
        /// <param name="ruleEngine"></param>
        /// <param name="ruleCreator"></param>
        public UpdateCalculateCartTaxBlock(IRuleEngine ruleEngine, IRuleCreator ruleCreator)
            : base(null)
        {
            _ruleEngine = ruleEngine;
            _ruleCreator = ruleCreator;
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
            if (!arg.HasComponent<FulfillmentComponent>()) { return await Task.FromResult(arg); }



            return await Task.FromResult(arg);
        }



    }
}

