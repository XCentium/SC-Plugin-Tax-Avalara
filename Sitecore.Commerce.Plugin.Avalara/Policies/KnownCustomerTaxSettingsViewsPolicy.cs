using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Policies
{
    public class KnownCustomerTaxSettingsViewsPolicy : Policy
    {
        public string CustomerSettings { get; set; } = "CustomerAvalaraTaxSettings";
    }
}
