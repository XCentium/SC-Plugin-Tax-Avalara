using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Configuration.BooleanLogic;

namespace Sitecore.Commerce.Plugin.Avalara.Models
{
   public class AvalaraConfiguration
    {
        private readonly PropertiesModel _itemProp;

        public AvalaraConfiguration(Core.PropertiesModel itemProp)
        {
            _itemProp = itemProp;
        }


        /// <summary>
        /// 
        /// </summary>
        public string EmptyAddressResponseFromCommerce
        {
            get
            {


                if (!(_itemProp?.GetPropertyValue(Constants.Tax.EmptyAddressResponseFromCommerce) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorConnectingToCommerce
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.ErrorConnectingToCommerce) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string CustomerMessage
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.CustomerMessage) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string AvaTaxSettingsMissing
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AvaTaxSettingsMissing) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string AvaTaxIsDisabled
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AvaTaxIsDisabled) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionSuccessful
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.ConnectionSuccessful) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorConnectingInvalidCredentials
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.ErrorConnectingInvalidCredentials) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.Enabled) is string resp) || string.IsNullOrEmpty(resp))
                    return false;

                return !string.IsNullOrEmpty(resp);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public bool InProductionMode
        {

            get
            {

                if (!(_itemProp?.GetPropertyValue(Constants.Tax.InProduction) is string resp) || string.IsNullOrEmpty(resp))
                    return false;

                return !string.IsNullOrEmpty(resp);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        public string CompanyCode
        {
            get
            {
                if(!(_itemProp?.GetPropertyValue(Constants.Tax.CompanyCode) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp; 

            }

        }


        /// <summary>
        /// 
        /// </summary>
        public string TestUrl
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.TestUrl) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ProductionUrl
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.ProdUrl) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string AppName
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AppName) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AppVersion
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AppVersion) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountId
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AccountId) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.UserName) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.Password) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine1
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AddressLine1) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine2
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AddressLine2) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine3
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.AddressLine3) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromCity
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.City) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromStateOrProvinceCode
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.StateCode) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromPostalCode
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.ZipCode) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShipFromCountryCode
        {
            get
            {
                if (!(_itemProp?.GetPropertyValue(Constants.Tax.CountryCode) is string resp) || string.IsNullOrEmpty(resp)) return string.Empty;

                return resp;

            }
        }

    }
}
