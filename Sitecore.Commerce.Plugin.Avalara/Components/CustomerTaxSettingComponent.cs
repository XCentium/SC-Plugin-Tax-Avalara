using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Components
{
    public class CustomerTaxSettingComponent : Component
    {
        public string EntityUseCode { get; set; } = string.Empty;
        public string ExemptionNumber { get; set; } = string.Empty;

    }
}
