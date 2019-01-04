using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Commerce.Plugin.Avalara
{
    /// <summary>
    /// 
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// 
        /// </summary>
        public struct Tax
        {

            /// <summary>
            /// 
            /// </summary>
            public const string CartNullText = "The cart cannot be null.";

            /// <summary>
            /// 
            /// </summary>
            public const string TaxFee = "TaxFee";

            /// <summary>
            /// /
            /// </summary>
            public const string Lang = "en-US";


            /// <summary>
            /// 
            /// </summary>
            public const string Us = "us";


            /// <summary>
            /// 
            /// </summary>
            public const string ProductTax = "ProductTax";

            /// <summary>
            /// 
            /// </summary>
            public const string AllOthers = "All Others";
            /// <summary>
            /// 
            /// </summary>
            public const string CartLineNullText = "The cart lines cannot be null.";

            /// <summary>
            /// 
            /// </summary>
            public const string Price = "Shipping Price";

            /// <summary>
            /// 
            /// </summary>
            public const string ItemId = "ItemID";

            /// <summary>
            /// 
            /// </summary>
            public const string SitecoreItem = "SitecoreItem_{0}";

            /// <summary>
            /// 
            /// </summary>
            public const string ForwardSlash = "/";

            /// <summary>
            /// 
            /// </summary>
            public const string ConfigurationItemPath = "/sitecore/system/Modules/Avalara/Settings/Configuration";


            public const string Enabled = "Enabled";
            public const string InProduction = "InProduction";
            public const string CompanyCode = "CompanyCode";
            public const string TestUrl = "TestUrl";
            public const string ProdUrl = "ProdUrl";
            public const string AccountId = "AccountId";
            public const string UserName = "UserName";
            public const string Password = "Password";
            public const string AddressLine1 = "AddressLine1";
            public const string AddressLine2 = "AddressLine2";
            public const string AddressLine3 = "AddressLine3";
            public const string City = "City";
            public const string StateCode = "StateCode";
            public const string ZipCode = "ZipCode";
            public const string CountryCode = "CountryCode";
            public const string AppName = "AppName";
            public const string AppVersion = "AppVersion";
            public const string ConfigIsNull = "Configuration settings is null"; 
            public const string AvalaraDisabled = "Avalara is not enabled in Sitecore";

            /// <summary>
            /// 
            /// </summary>
            public const string DefaultTaxCode = "P0000000";
            public const string DefaultTaxExcemptCode = "NT";
            public const string DefaultFreightCode = "FR020100";

        }


        public struct View
        {

            /// <summary>
            /// 
            /// </summary>
            public const string AvalaraCategoryTaxSettingsView = "AvalaraCategoryTaxSettingsView";

            /// <summary>
            /// 
            /// </summary>
            public const string AvalaraProductTaxSettingsView = "AvalaraProductTaxSettingsView";

            /// <summary>
            /// 
            /// </summary>
            public const string AvalaraVariantTaxSettingsView = "AvalaraVariantTaxSettingsView";

            /// <summary>
            /// 
            /// </summary>
            public const string AvalaraCustomerTaxSettingsView = "AvalaraCustomerTaxSettingsView";


        }
    }
}

