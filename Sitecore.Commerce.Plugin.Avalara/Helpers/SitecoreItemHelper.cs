using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Models;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Services.Core.Model;

namespace Sitecore.Commerce.Plugin.Avalara.Helpers
{
    public static class SitecoreItemHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal GetFirstDecimalFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0.00M;
            var decList = Regex.Split(str, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToList();
            var decimalVal = decList.Any() ? decList.FirstOrDefault() : string.Empty;

            if (string.IsNullOrEmpty(decimalVal)) return 0.00M;
            decimal.TryParse(decimalVal, out var decimalResult);

            return decimalResult;
        }



        public static PropertiesModel TranslateToProperties(ItemModel itemModel)
        {
            var propertiesModelInitial = new PropertiesModel();
            var str = string.Format(Constants.Tax.SitecoreItem, itemModel[Constants.Tax.ItemId] as string);
            propertiesModelInitial.Name = str;
            var propertiesModelTranslated = propertiesModelInitial;
            foreach (var keyValuePair in itemModel)
                if (keyValuePair.Value is string)
                    propertiesModelTranslated.SetPropertyValue(keyValuePair.Key, (string)keyValuePair.Value);
                else
                    propertiesModelTranslated.SetPropertyValue(keyValuePair.Key, keyValuePair.Value);
            return propertiesModelTranslated;
        }

        internal static AvalaraConfiguration GetAvaTaxMessages(CommerceContext context, IGetItemByPathPipeline getItemByPathPipeline)
        {

            var language = context.CurrentLanguage();

            var itemModelShippingPriceArgument =
                new ItemModelArgument(Constants.Tax.AvaMessagesItemPath)
                {
                    Language = language
                };

            var configurationItem = getItemByPathPipeline.Run(itemModelShippingPriceArgument, context.PipelineContext.ContextOptions).Result;

            if (configurationItem == null)
            {
              return  new AvalaraConfiguration(new PropertiesModel());
            }

            var itemProp = TranslateToProperties(configurationItem);

            return new AvalaraConfiguration(itemProp);

        }
    }
}
