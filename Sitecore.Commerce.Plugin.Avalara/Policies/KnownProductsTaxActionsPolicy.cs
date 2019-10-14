using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Avalara.Policies
{
    public class KnownProductsTaxActionsPolicy : Policy
    {
        public string EditProductTaxSettings { get; set; } = nameof(EditProductTaxSettings);
    }
}
