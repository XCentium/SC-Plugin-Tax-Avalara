using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Avalara.Models;

namespace Sitecore.Commerce.Plugin.Avalara.Entities
{
    public class AvalaraTaxCacheEntity : CommerceEntity
    {
        public int LineCount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal TaxTotal { get; set; }

        public DateTime WhenCreated { get; set; }

        public List<LineTaxRecord> LinesTax { get; set; }

        public string Address1 { get; set; }
        public string StateCode { get; set; }
        public string ZipPostalCode { get; set; }

    }
}
