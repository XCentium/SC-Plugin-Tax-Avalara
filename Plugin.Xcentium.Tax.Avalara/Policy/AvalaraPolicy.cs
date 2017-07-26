using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Tax.Avalara
{

    /// <summary>
    /// 
    /// </summary>
    public class AvalaraPolicy: Policy
    {
        /// <summary>
        /// 
        /// </summary>
        public AvalaraPolicy()
        {
            AccountId = "2000196050";
            UserName = "ola.owolawi@xcentium.com";
            Password = "5A42755F8A";
            CompanyCode = "ZEAL99";
            InProductionMode = false;
            TestUrl = "https://sandbox-rest.avatax.com/api/v2/transactions/create";
            ProductionUrl = "https://rest.avatax.com/api/v2/transactions/create";

            // ship from settings
            this.ShipFromAddressLine1 = "7600 bancroft Cir";
            this.ShipFromAddressLine2 = string.Empty;
            this.ShipFromAddressLine3 = "";
            this.ShipFromCity = "Fort Worth";
            this.ShipFromStateOrProvinceCode = "TX";
            this.ShipFromPostalCode = "76120";
            this.ShipFromCountryCode = "US";

        }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TestUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductionUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool InProductionMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        // ship from address
        /// <summary>
        /// 
        /// </summary>
        public string ShipFromName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromAddressLine3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromCity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromStateOrProvinceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipFromPostalCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipFromCountryCode { get; set; }


    }
}
