using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Components
{
   public class ProductTaxSettingsComponent : Component
   {
       public string TaxCode { get; set; } = string.Empty;
    }
}
