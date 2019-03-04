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
            Condition.Requires(modelBuilder).IsNotNull($"{this.Name}: {Sitecore.Commerce.Plugin.Avalara.Constants.Tax.ArgumentNullText}");


            // Add unbound actions
            var configuration4 = modelBuilder.Action(Constants.Tax.TestAvalaraConnection);
            configuration4.ReturnsFromEntitySet<CommerceCommand>(Constants.Tax.Commands);


            // Add unbound actions
            var configuration5 = modelBuilder.Action(Constants.Tax.GetAvalaraConfiguration);
            configuration5.ReturnsFromEntitySet<CommerceCommand>(Constants.Tax.Commands);



            // Add unbound actions
            var configuration2 = modelBuilder.Action(Constants.Tax.SaveAvalaraConfiguration);
            configuration2.Parameter<string>(Constants.Tax.CompanyCode);
            configuration2.Parameter<bool>(Constants.Tax.InProductionMode);
            configuration2.Parameter<string>(Constants.Tax.AccountId);
            configuration2.Parameter<string>(Constants.Tax.AppName);
            configuration2.Parameter<string>(Constants.Tax.AppVersion);
            configuration2.Parameter<string>(Constants.Tax.LicenseKey);
            configuration2.Parameter<string>(Constants.Tax.ShipFromName);
            configuration2.Parameter<string>(Constants.Tax.ShipFromAddressLine1);
            configuration2.Parameter<string>(Constants.Tax.ShipFromAddressLine2);
            configuration2.Parameter<string>(Constants.Tax.ShipFromAddressLine3);
            configuration2.Parameter<string>(Constants.Tax.ShipFromCity);
            configuration2.Parameter<string>(Constants.Tax.ShipFromStateOrProvinceCode);
            configuration2.Parameter<string>(Constants.Tax.ShipFromPostalCode);
            configuration2.Parameter<string>(Constants.Tax.ShipFromCountryCode);
            configuration2.Parameter<bool>(Constants.Tax.Enabled);
            configuration2.Parameter<bool>(Constants.Tax.DisableReporting);
            configuration2.ReturnsFromEntitySet<CommerceCommand>(Constants.Tax.Commands);

            return Task.FromResult(modelBuilder);

        }
    }
}
