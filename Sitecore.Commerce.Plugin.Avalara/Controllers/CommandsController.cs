using System;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Avalara.AvaTax.RestClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Entities;
using Sitecore.Commerce.Plugin.Avalara.Helpers;
using Sitecore.Commerce.Plugin.Management;

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
            // Get custom error messages from Sitecore
            var messagesItem = SitecoreItemHelper.GetAvaTaxMessages(CurrentContext.PipelineContext.ContextOptions.CommerceContext, _getItemByPathPipeline);

            // Get Entity

            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1", false), CurrentContext.PipelineContext.ContextOptions) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {
                return new ObjectResult(new { Success = false, Message = messagesItem.AvaTaxSettingsMissing });
            }

            if (avalaraTaxEntity.Enabled == false)
            {
                return new ObjectResult(new { Success = false, Message = messagesItem.AvaTaxIsDisabled });
            }


            var client = new AvaTaxClient(avalaraTaxEntity.AppName, avalaraTaxEntity.AppVersion, Environment.MachineName, avalaraTaxEntity.InProductionMode ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                .WithSecurity(avalaraTaxEntity.AccountId, avalaraTaxEntity.LicenseKey);

            try
            {

                // Verify that we can ping successfully
                var pingResult = client.Ping();

                if (pingResult.authenticated != null && (bool)pingResult.authenticated)
                {

                    return new ObjectResult(new { Success = true, Message = messagesItem.ConnectionSuccessful });
                }


                var result = new { Success = false, Message = messagesItem.ErrorConnectingInvalidCredentials };

                return new ObjectResult(result);


            }
            catch (Exception ex)
            {
                CurrentContext.PipelineContext.ContextOptions.CommerceContext.Logger.LogError(ex.Message, CurrentContext.PipelineContext.ContextOptions.CommerceContext);
                var result = new { Success = false, Message = ex.Message };
                return new ObjectResult(result);
            }
        }


        [HttpPost]
        [Route("GetAvalaraConfiguration()")]
        public async Task<IActionResult> GetAvalaraConfiguration([FromBody] ODataActionParameters value)
        {


            // Get Entity
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1", false), CurrentContext.PipelineContext.ContextOptions) as AvalaraTaxEntity;

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
            var avalaraTaxEntity = await _findEntity.Run(new FindEntityArgument(typeof(AvalaraTaxEntity), "Entity-AvalaraTaxEntity-1",false), CurrentContext.PipelineContext.ContextOptions) as AvalaraTaxEntity;

            if (avalaraTaxEntity == null)
            {

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

            var enabled = value["Enabled"]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.Enabled = !string.IsNullOrEmpty(enabled);

            var disableReporting = value["DisableReporting"]?.ToString().Trim() ?? string.Empty;
            avalaraTaxEntity.DisableReporting = !string.IsNullOrEmpty(disableReporting);

            var persistEntityArgument = await this._persistEntityPipeline.Run(new PersistEntityArgument((CommerceEntity)avalaraTaxEntity), this.CurrentContext.PipelineContext.ContextOptions);


            return new ObjectResult(avalaraTaxEntity);
        }

    }
}
