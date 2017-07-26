using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Commerce.Plugin.Tax;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Xcentium.Tax.Avalara.Pipelines.Block
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


            var language = context.CommerceContext.CurrentLanguage();
            var currencyCode = context.CommerceContext.CurrentCurrency();
            var globalTaxPolicy = context.GetPolicy<GlobalTaxPolicy>();
            var defaultTaxRate = globalTaxPolicy.DefaultCartTaxRate;
            var taxRate = defaultTaxRate;
            var globalPricingPolicy = context.GetPolicy<GlobalPricingPolicy>();
            var avalaraTaxPolicy = context.GetPolicy<AvalaraPolicy>();


            foreach (var cartLineComponent in list)
            {
                // If it is an electronically transmitted item such as e-book or subscription, charge no tax
                if (cartLineComponent.ChildComponents.OfType<FulfillmentComponent>().FirstOrDefault() is
                        ElectronicFulfillmentComponent)
                {
                    context.Logger.LogDebug(string.Format("{0} - Skipping Tax Calculation for Electronic Delivery", Name));
                }
                else
                {
                    // if there is a delivery address, then charge tax
                    if (cartLineComponent.ChildComponents.OfType<FulfillmentComponent>().FirstOrDefault() is
                        PhysicalFulfillmentComponent)
                    {
                        var cartComponent = cartLineComponent.GetComponent<PhysicalFulfillmentComponent>();
                        ShippingParty = cartComponent?.ShippingParty;

                        if (ShippingParty != null)
                        {

                            var lines = new[]
                            {
                                new
                                {
                                    amount =
                                    double.Parse(
                                        cartLineComponent.Totals.GrandTotal.Amount.ToString(CultureInfo.InvariantCulture)),
                                    description = "Item " + cartLineComponent.ItemId,
                                    itemCode = cartLineComponent.ItemId,
                                    quantity = Convert.ToInt32(Math.Ceiling(cartLineComponent.Quantity))
                                }

                            }.ToList();



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
                                currencyCode = currencyCode,
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

                    var subTotal = cartLineComponent.Adjustments.Where(a => a.IsTaxable)
                        .Aggregate(decimal.Zero, (current, adjustment) => current + adjustment.Adjustment.Amount);
                    var subTotalRound = new Money(currencyCode,
                        (cartLineComponent.Totals.SubTotal.Amount + subTotal) * taxRate);
                    if (globalPricingPolicy.ShouldRoundPriceCalc)
                        subTotalRound.Amount = decimal.Round(subTotalRound.Amount, globalPricingPolicy.RoundDigits,
                            globalPricingPolicy.MidPointRoundUp ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
                    var adjustments = cartLineComponent.Adjustments;
                    var awardedAdjustment = new CartLineLevelAwardedAdjustment
                    {
                        Name = Constants.Tax.TaxFee,
                        DisplayName = Constants.Tax.TaxFee,
                        Adjustment = subTotalRound
                    };

                    var tax = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax;
                    awardedAdjustment.AdjustmentType = tax;

                    awardedAdjustment.AwardingBlock = Name;
                    var taxableFlag = 0;
                    awardedAdjustment.IsTaxable = taxableFlag != 0;
                    var includeinGrandTotalFlag = 0;
                    awardedAdjustment.IncludeInGrandTotal = includeinGrandTotalFlag != 0;
                    adjustments.Add(awardedAdjustment);

                }
            }
            return await Task.FromResult(arg);
        }



    }
}
