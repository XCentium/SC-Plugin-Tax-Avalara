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
            public const string ArgumentNullText = "The argument cannot be null.";

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
            public const string InProductionMode = "InProductionMode";
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
            
            public const string LicenseKey = "LicenseKey";
            public const string FreightCode = "FreightCode";
            public const string DisableReporting = "DisableReporting";

            public const string ShipFromName = "ShipFromName";
            public const string ShipFromAddressLine1 = "ShipFromAddressLine1";
            public const string ShipFromAddressLine2 = "ShipFromAddressLine2";
            public const string ShipFromAddressLine3 = "ShipFromAddressLine3";
            public const string ShipFromCity = "ShipFromCity";
            public const string ShipFromStateOrProvinceCode = "ShipFromStateOrProvinceCode";
            public const string ShipFromPostalCode = "ShipFromPostalCode";
            public const string ShipFromCountryCode = "ShipFromCountryCode";

            /// <summary>
            /// 
            /// </summary>
            public const string DefaultTaxCode = "P0000000";
            public const string DefaultTaxExcemptCode = "NT";
            public const string DefaultFreightCode = "FR020100";




            public const string AvalaraTaxConfig = "Entity-AvalaraTaxEntity-1";
            public const string AvalaraSettingsMissing = "Avalara Settings Missing";
            public const string AvalaraDisabled = "Avalara Is Disabled";
            public const string AvalaraConnectionSuccessful = "Connection Successful!";
            public const string AvalaraConnectionError = "Error Connecting to Avalara. Invalid Credentials.";

            public const string SaveAvalaraConfiguration = "SaveAvalaraConfiguration";
            public const string GetAvalaraConfiguration = "GetAvalaraConfiguration";
            public const string TestAvalaraConnection = "TestAvalaraConnection";
            public const string Commands = "Commands";



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

