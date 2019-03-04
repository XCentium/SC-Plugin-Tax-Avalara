using System;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Avalara.AvaTax.RestClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Management;

namespace Sitecore.Commerce.Plugin.Avalara.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment, IFindEntityPipeline findEntityPipeline, IPersistEntityPipeline persistEntityPipeline) 
            : base(serviceProvider, globalEnvironment)
        {
            _findEntity = findEntityPipeline;
            _persistEntityPipeline = persistEntityPipeline;
        }

        private readonly IFindEntityPipeline _findEntity;
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

            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), Constants.Tax.AvalaraTaxConfig, false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {
                return new ObjectResult(new { Success = false, Result = Constants.Tax.AvalaraSettingsMissing });
            }

            if (avalaraTaxEntity.Enabled == false)
            {
                return new ObjectResult(new { Success = false, Result = Constants.Tax.AvalaraDisabled });
            }


            var client = new AvaTaxClient(avalaraTaxEntity.AppName, avalaraTaxEntity.AppVersion, Environment.MachineName, avalaraTaxEntity.InProductionMode ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                .WithSecurity(avalaraTaxEntity.AccountId, avalaraTaxEntity.LicenseKey);

            // Verify that we can ping successfully
            var pingResult = client.Ping();


            if (pingResult.authenticated != null && (bool)pingResult.authenticated)
            {

                return new ObjectResult(new { Success = true, Result = Constants.Tax.AvalaraConnectionSuccessful });
            }

            var result = new {Success = false, Result = Constants.Tax.AvalaraConnectionError };

            return new ObjectResult(result);
        }


        [HttpPost]
        [Route("GetAvalaraConfiguration()")]
        public async Task<IActionResult> GetAvalaraConfiguration([FromBody] ODataActionParameters value)
        {


            // Get Entity
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), Constants.Tax.AvalaraTaxConfig, false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null) { return (IActionResult)new BadRequestObjectResult((object)value); }

            var output = JsonConvert.SerializeObject(avalaraTaxEntity);

            dynamic jsonResponse = JsonConvert.DeserializeObject(output);

            return new ObjectResult(jsonResponse);

        }


        [HttpPost]
        [Route("SaveAvalaraConfiguration()")]
        public async Task<IActionResult> SaveAvalaraConfiguration([FromBody] ODataActionParameters value)
        {

            if (!value.ContainsKey(Constants.Tax.CompanyCode)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.InProductionMode)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.Enabled)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.AccountId)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.LicenseKey)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.ShipFromAddressLine1)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.ShipFromCity)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.ShipFromStateOrProvinceCode)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.ShipFromPostalCode)) return (IActionResult)new BadRequestObjectResult((object)value);
            if (!value.ContainsKey(Constants.Tax.ShipFromCountryCode)) return (IActionResult)new BadRequestObjectResult((object)value);



            // Get Entity
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), Constants.Tax.AvalaraTaxConfig,false), CurrentContext.GetPipelineContextOptions()) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {
                avalaraTaxEntity = new AvalaraTaxEntity
                {
                    Id = Constants.Tax.AvalaraTaxConfig,
                    FriendlyId = Constants.Tax.AvalaraTaxConfig
                };
            }

            avalaraTaxEntity.Id = Constants.Tax.AvalaraTaxConfig;
            avalaraTaxEntity.FriendlyId = Constants.Tax.AvalaraTaxConfig;

            avalaraTaxEntity.AccountId =int.Parse(value[Constants.Tax.AccountId].ToString().Trim());
            avalaraTaxEntity.LicenseKey = value[Constants.Tax.LicenseKey].ToString().Trim(); 
            avalaraTaxEntity.AppName = value[Constants.Tax.AppName].ToString().Trim();
            avalaraTaxEntity.AppVersion = value[Constants.Tax.AppVersion].ToString().Trim();
            avalaraTaxEntity.CompanyCode = value[Constants.Tax.CompanyCode].ToString().Trim();
            avalaraTaxEntity.FreightCode = value[Constants.Tax.FreightCode].ToString().Trim();


            var inProductionMode = value[Constants.Tax.InProductionMode]?.ToString().Trim() ?? string.Empty;
               
            avalaraTaxEntity.InProductionMode = !string.IsNullOrEmpty(inProductionMode);

            avalaraTaxEntity.ShipFromAddressLine1 = value[Constants.Tax.ShipFromAddressLine1].ToString().Trim();
            avalaraTaxEntity.ShipFromAddressLine2 = value[Constants.Tax.ShipFromAddressLine2].ToString().Trim();
            avalaraTaxEntity.ShipFromAddressLine3 = value[Constants.Tax.ShipFromAddressLine3].ToString().Trim();
            avalaraTaxEntity.ShipFromCity = value[Constants.Tax.ShipFromCity].ToString().Trim();
            avalaraTaxEntity.ShipFromCountryCode = value[Constants.Tax.ShipFromCountryCode].ToString().Trim();
            avalaraTaxEntity.ShipFromName = value[Constants.Tax.ShipFromName].ToString().Trim();
            avalaraTaxEntity.ShipFromPostalCode = value[Constants.Tax.ShipFromPostalCode].ToString().Trim();
            avalaraTaxEntity.ShipFromStateOrProvinceCode = value[Constants.Tax.ShipFromStateOrProvinceCode].ToString().Trim();

            var enabled = value[Constants.Tax.Enabled]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.Enabled = !string.IsNullOrEmpty(enabled);

            var disableReporting = value[Constants.Tax.DisableReporting]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.DisableReporting = !string.IsNullOrEmpty(disableReporting);

            var persistEntityArgument = await this._persistEntityPipeline.Run(new PersistEntityArgument((CommerceEntity)avalaraTaxEntity), CurrentContext.GetPipelineContextOptions());


            return new ObjectResult(avalaraTaxEntity);
        }

    }
}
