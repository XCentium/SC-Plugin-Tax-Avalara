using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog.Core;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Policies;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Commerce.Plugin.Tax;
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


/*
            var taxAdjustments = arg.Adjustments.Where(a =>
                {
                    if (!string.IsNullOrEmpty(a.Name) && a.Name.Equals(Constants.Tax.TaxFee, StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrEmpty(a.AdjustmentType))
                        return a.AdjustmentType.Equals(context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                            StringComparison.OrdinalIgnoreCase);
                    return false;
                }).ToList();

            if (taxAdjustments.Any())
            {
                // remove adjustments
                arg.Adjustments.Where(a =>
                {
                    if (!string.IsNullOrEmpty(a.Name) && a.Name.Equals(Constants.Tax.TaxFee, StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrEmpty(a.AdjustmentType))
                        return a.AdjustmentType.Equals(context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                            StringComparison.OrdinalIgnoreCase);
                    return false;
                }).ToList().ForEach(a => arg.Adjustments.Remove(a));


                var language = context.CommerceContext.CurrentLanguage();
                var currency = context.CommerceContext.CurrentCurrency();
                var globalTaxPolicy = context.GetPolicy<GlobalTaxPolicy>();
                var defaultTaxRate = globalTaxPolicy.DefaultCartTaxRate;
                var taxRate = defaultTaxRate;
                var avalaraTaxPolicy = context.GetPolicy<AvalaraPolicy>();


                if (arg.Lines.Any() && arg.HasComponent<PhysicalFulfillmentComponent>())
                {
                    var cartComponent = arg.GetComponent<PhysicalFulfillmentComponent>();
                    ShippingParty = cartComponent?.ShippingParty;
                    if (ShippingParty != null)
                    {

                        var lines = new[]
                        {
                            new
                            {
                                amount = default(double),
                                description = default(string),
                                itemCode = default(string),
                                quantity = default(int),
                            }

                        }.ToList();

                        lines.RemoveAt(0);

                        foreach (var cartLineComponent in arg.Lines)
                        {

                            lines.Add(new
                            {
                                amount = double.Parse(cartLineComponent.Totals.GrandTotal.Amount.ToString(CultureInfo.InvariantCulture)),
                                description = "Item " + cartLineComponent.ItemId,
                                itemCode = cartLineComponent.ItemId,
                                quantity = Convert.ToInt32(Math.Ceiling(cartLineComponent.Quantity))

                            });
                        }


                        var shipfrom = new
                        {
                            line1 = avalaraTaxPolicy.ShipFromAddressLine1 + " " + avalaraTaxPolicy.ShipFromAddressLine2,
                            city = avalaraTaxPolicy.ShipFromCity,
                            region = avalaraTaxPolicy.ShipFromStateOrProvinceCode,
                            country = avalaraTaxPolicy.ShipFromCountryCode,
                            postalCode = avalaraTaxPolicy.ShipFromPostalCode
                        };

                        var pointOfOrderOrigin = new
                        {
                            line1 = avalaraTaxPolicy.ShipFromAddressLine1 + " " + avalaraTaxPolicy.ShipFromAddressLine2,
                            city = avalaraTaxPolicy.ShipFromCity,
                            region = avalaraTaxPolicy.ShipFromStateOrProvinceCode,
                            country = avalaraTaxPolicy.ShipFromCountryCode,
                            postalCode = avalaraTaxPolicy.ShipFromPostalCode
                        };

                        var shipTo = new
                        {
                            line1 = ShippingParty.Address1,
                            city = ShippingParty.City,
                            region = !string.IsNullOrEmpty(ShippingParty.StateCode) ? ShippingParty.StateCode : ShippingParty.State,
                            country = ShippingParty.CountryCode,
                            postalCode = ShippingParty.ZipPostalCode
                        };


                        var pointOfOrderAcceptance = new
                        {
                            line1 = ShippingParty.Address1,
                            city = ShippingParty.City,
                            region = !string.IsNullOrEmpty(ShippingParty.StateCode) ? ShippingParty.StateCode : ShippingParty.State,
                            country = ShippingParty.CountryCode,
                            postalCode = ShippingParty.ZipPostalCode
                        };

                        var addresses = new
                        {
                            shipFrom = shipfrom,
                            pointOfOrderOrigin = pointOfOrderOrigin,
                            shipTo = shipTo,
                            pointOfOrderAcceptance = pointOfOrderAcceptance
                        };


                        var avalaraRequest = new
                        {
                            lines = lines,
                            type = "SalesInvoice",
                            date = DateTime.UtcNow.ToLongDateString(),
                            companyCode = avalaraTaxPolicy.CompanyCode,
                            customerCode = arg.Id.ToLower().Replace("default", String.Empty).Replace(arg.ShopName.ToLower(), String.Empty),
                            purchaseOrderNo = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                            addresses = addresses,
                            commit = false,
                            currencyCode = currency,
                            description = arg.Id + "Cart for user" + arg.Id + " Date: " + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                        };

                        var url = avalaraTaxPolicy.InProductionMode
                            ? avalaraTaxPolicy.ProductionUrl
                            : avalaraTaxPolicy.TestUrl;



                        var credentials = "Basic " + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", avalaraTaxPolicy.UserName, avalaraTaxPolicy.Password)));
                        var appName = "MyTestApp";
                        var appVersion = "1.0";
                        var api_version = "17.6.0-89";

                        var clientHeader = String.Format("{0}; {1}; {2}; {3}; {4}", appName, appVersion, "CSharpRestClient", api_version, "SANDBOX");
                        var client = new HttpClient();
                        // Setup the request
                        using (var request = new HttpRequestMessage())
                        {
                            request.Method = new HttpMethod("POST");
                            request.RequestUri = new Uri(url);

                            // Add credentials and client header
                            request.Headers.Add("Authorization", credentials);

                            request.Headers.Add("X-Avalara-Client", clientHeader);


                            // Add payload
                            var json = JsonConvert.SerializeObject(avalaraRequest, SerializerSettings);
                            request.Content = new StringContent(json, Encoding.UTF8, "application/json");


                            // Send
                            var response = client.SendAsync(request).Result;


                            if (response != null && response.IsSuccessStatusCode)
                            {

                                dynamic contentData = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                                var totalTax = contentData.totalTax.ToString();

                                decimal.TryParse(totalTax.ToString(), out taxRate);

                                response.Dispose();

                            }


                            client.Dispose();


                        }

                    }
                }


                //var adjustmentTotal = arg.Adjustments.Where(p => p.IsTaxable)
                //    .Aggregate(decimal.Zero, (current, adjustment) => current + adjustment.Adjustment.Amount);

                //_ruleEngine.Run(new IRule[1]
                //{
                //    _ruleCreator.Create()
                //        .When((Action<IRuleProperties<TaxCalculationEnabled>>) null)
                //        .Then((Action<IRuleProperties<AddCartTaxAdjustment>>) (p =>
                //        {
                //            var ruleProperties = p;
                //            Expression<Func<AddCartTaxAdjustment, IRuleValue<CartLevelAwardedAdjustment>>> propertySelector
                //                = a => a.Adjustment;
                //            var literalRuleValue = new LiteralRuleValue<CartLevelAwardedAdjustment>();
                //            var awardedAdjustment = new CartLevelAwardedAdjustment
                //            {
                //                Name = Constants.Tax.TaxFee,
                //                DisplayName = Constants.Tax.TaxFee,
                //                Adjustment = new Money(currency,
                //                    (arg.Totals.SubTotal.Amount + adjustmentTotal) * taxRate),
                //                AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                //                AwardingBlock = Name
                //            };
                //            var num = 0;
                //            awardedAdjustment.IsTaxable = num != 0;
                //            literalRuleValue.Value = awardedAdjustment;
                //            ruleProperties.Set(propertySelector, literalRuleValue);
                //        })).ToRule()
                //}, spec => spec.Fact(context.CommerceContext, "").Fact(arg, ""));

            }
*/
            return await Task.FromResult(arg);
        }



    }
}

