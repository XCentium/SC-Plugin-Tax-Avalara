using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Avalara.AvaTax.RestClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Avalara.Helpers;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara.Controllers
{
    public class CommandsController : CommerceController
    {


        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment, IGetItemByPathPipeline getItemByPathPipeline, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline) 
            : base(serviceProvider, globalEnvironment)
        {
            _getItemByPathPipeline = getItemByPathPipeline;
            _findEntity = findEntityPipeline;
            _persistEntityPipeline = persistEntityPipeline;
        }

        private readonly IFindEntityPipeline _findEntity;
        private readonly IGetItemByPathPipeline _getItemByPathPipeline;
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TestAvalaraConnection()")]
        public async Task<IActionResult> TestAvalaraConnection([FromBody] ODataActionParameters value)
        {
            // Get Entity

            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1", false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {
                return new ObjectResult(new { Success = false, Result = "Avalara Settings Missing" });
            }

            if (avalaraTaxEntity.Enabled == false)
            {
                return new ObjectResult(new { Success = false, Result = "Avalara Is Disabled" });
            }

            #region Comment1

            //// Get config from Sitecore
            //var config = SitecoreItemHelper.GetConfiguration(this.CurrentContext.GetPipelineContextOptions().CommerceContext, _getItemByPathPipeline);

            //if (config == null)
            //{
            //    return new ObjectResult(new { Success = false, Result = Constants.Tax.ConfigIsNull });
            //}

            //if (config.Enabled == false)
            //{
            //    return new ObjectResult(new { Success = false, Result = Constants.Tax.AvalaraDisabled, Config = config });
            //}

            //var client = new AvaTaxClient(config.AppName, config.AppVersion, Environment.MachineName, config.InProductionMode? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
            //    .WithSecurity(config.UserName, config.Password);


            #endregion



            var client = new AvaTaxClient(avalaraTaxEntity.AppName, avalaraTaxEntity.AppVersion, Environment.MachineName, avalaraTaxEntity.InProductionMode ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                .WithSecurity(avalaraTaxEntity.UserName, avalaraTaxEntity.Password);

            // Verify that we can ping successfully
            var pingResult = client.Ping();

            #region Comment2
                //// Create base transaction.
                //var builder = new TransactionBuilder(client, avalaraTaxEntity.CompanyCode, DocumentType.SalesInvoice,"TaxOverrideCustomerCode")
                //    .WithAddress(TransactionAddressType.SingleLocation, avalaraTaxEntity.ShipFromAddressLine1, null, null, avalaraTaxEntity.ShipFromCity, avalaraTaxEntity.ShipFromStateOrProvinceCode,
                //        avalaraTaxEntity.ShipFromStateOrProvinceCode, avalaraTaxEntity.ShipFromCountryCode)
                //    .WithLine(100.0m, 1, "P0000000")
                //    .WithLine(200m);

                //var transaction = builder.Create();
            #endregion



            if (pingResult.authenticated != null && (bool)pingResult.authenticated)
            {

                return new ObjectResult(new { Success = true, Result = "Connection Successful!" });
            }

            var result = new {Success = false, Result = "Error Connecting to Avalara. Invalid Credentials." };

            return new ObjectResult(result);
        }


        [HttpPost]
        [Route("GetAvalaraConfiguration()")]
        public async Task<IActionResult> GetAvalaraConfiguration([FromBody] ODataActionParameters value)
        {


            // Get Entity
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1", false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null) { return (IActionResult)new BadRequestObjectResult((object)value); }

            var output = JsonConvert.SerializeObject(avalaraTaxEntity);

            dynamic jsonResponse = JsonConvert.DeserializeObject(output);

            return new ObjectResult(jsonResponse);

        }


        [HttpPost]
        [Route("SaveAvalaraConfiguration()")]
        public async Task<IActionResult> SaveAvalaraConfiguration([FromBody] ODataActionParameters value)
        {

            if (!value.ContainsKey("CompanyCode")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("InProductionMode")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("Enabled")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("AccountId")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("LicenseKey")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("ShipFromAddressLine1")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("ShipFromCity")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("ShipFromStateOrProvinceCode")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("ShipFromPostalCode")) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey("ShipFromCountryCode")) return (IActionResult)new BadRequestObjectResult((object)value);



            // Get Entity
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1",false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {
                // 2777d5a8ef464ee8bd5dc29b1443cff5

                avalaraTaxEntity = new AvalaraTaxEntity
                {
                    Id = "Entity-AvalaraTaxEntity-1",
                    FriendlyId = "Entity-AvalaraTaxEntity-1"
                };
            }

            avalaraTaxEntity.Id = "Entity-AvalaraTaxEntity-1";
            avalaraTaxEntity.FriendlyId = "Entity-AvalaraTaxEntity-1";

            avalaraTaxEntity.AccountId =int.Parse(value["AccountId"].ToString().Trim());// LicenseKey
            avalaraTaxEntity.LicenseKey = value["LicenseKey"].ToString().Trim();// 
            avalaraTaxEntity.AppName = value["AppName"].ToString().Trim();
            avalaraTaxEntity.AppVersion = value["AppVersion"].ToString().Trim();
            avalaraTaxEntity.CompanyCode = value["CompanyCode"].ToString().Trim();
            avalaraTaxEntity.FreightCode = value["FreightCode"].ToString().Trim();


            var inProductionMode = value["InProductionMode"]?.ToString().Trim() ?? string.Empty;
               
            avalaraTaxEntity.InProductionMode = !string.IsNullOrEmpty(inProductionMode);

            avalaraTaxEntity.ShipFromAddressLine1 = value["ShipFromAddressLine1"].ToString().Trim();
            avalaraTaxEntity.ShipFromAddressLine2 = value["ShipFromAddressLine2"].ToString().Trim();
            avalaraTaxEntity.ShipFromAddressLine3 = value["ShipFromAddressLine3"].ToString().Trim();
            avalaraTaxEntity.ShipFromCity = value["ShipFromCity"].ToString().Trim();
            avalaraTaxEntity.ShipFromCountryCode = value["ShipFromCountryCode"].ToString().Trim();
            avalaraTaxEntity.ShipFromName = value["ShipFromName"].ToString().Trim();
            avalaraTaxEntity.ShipFromPostalCode = value["ShipFromPostalCode"].ToString().Trim();
            avalaraTaxEntity.ShipFromStateOrProvinceCode = value["ShipFromStateOrProvinceCode"].ToString().Trim();

            // bool.TryParse(value["Enabled"].ToString().Trim(), out var enabled);

            var enabled = value["Enabled"]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.Enabled = !string.IsNullOrEmpty(enabled);

            var disableReporting = value["DisableReporting"]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.DisableReporting = !string.IsNullOrEmpty(disableReporting);

            var persistEntityArgument = await this._persistEntityPipeline.Run(new PersistEntityArgument((CommerceEntity)avalaraTaxEntity), this.CurrentContext.GetPipelineContext());


            return new ObjectResult(avalaraTaxEntity);
        }

    }
}
