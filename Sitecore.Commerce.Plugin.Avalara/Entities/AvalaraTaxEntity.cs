using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Entities
{
    public class AvalaraTaxEntity : CommerceEntity
    {

        /// <summary>
        /// Avalara Company Code
        /// </summary>
        public string CompanyCode { get; set; }
        public string FreightCode { get; set; }


        /// <summary>
        /// Is this in production or test environment? Test connects to Sandbox
        /// </summary>
        public bool InProductionMode { get; set; }

        public bool Enabled { get; set; }
        public bool DisableReporting { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccountId { get; set; }
        public string LicenseKey { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }

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
