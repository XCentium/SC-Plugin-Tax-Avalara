using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Policies
{
    public class KnownProductTaxSettingsViewsPolicy : Policy
    {
        public string ProductSettings { get; set; } = "SellableItemProductSettings";
    }
}
