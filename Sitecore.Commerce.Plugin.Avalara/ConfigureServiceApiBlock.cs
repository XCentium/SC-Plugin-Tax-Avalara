using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Avalara
{
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull($"{this.Name}: The argument cannot be null.");


            // Add unbound actions
            var configuration4 = modelBuilder.Action("TestAvalaraConnection");
            configuration4.ReturnsFromEntitySet<CommerceCommand>("Commands");


            // Add unbound actions
            var configuration5 = modelBuilder.Action("GetAvalaraConfiguration");
            configuration5.ReturnsFromEntitySet<CommerceCommand>("Commands");



            // Add unbound actions
            var configuration2 = modelBuilder.Action("SaveAvalaraConfiguration");
            configuration2.Parameter<string>("CompanyCode");
            configuration2.Parameter<bool>("InProductionMode");
            configuration2.Parameter<string>("AccountId");
            configuration2.Parameter<string>("AppName");
            configuration2.Parameter<string>("AppVersion");
            configuration2.Parameter<string>("LicenseKey");
            configuration2.Parameter<string>("ShipFromName");
            configuration2.Parameter<string>("ShipFromAddressLine1");
            configuration2.Parameter<string>("ShipFromAddressLine2");
            configuration2.Parameter<string>("ShipFromAddressLine3");
            configuration2.Parameter<string>("ShipFromCity");
            configuration2.Parameter<string>("ShipFromStateOrProvinceCode");
            configuration2.Parameter<string>("ShipFromPostalCode");
            configuration2.Parameter<string>("ShipFromCountryCode");
            configuration2.Parameter<bool>("Enabled");
            configuration2.Parameter<bool>("DisableReporting");
            configuration2.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(modelBuilder);

        }
    }
}
