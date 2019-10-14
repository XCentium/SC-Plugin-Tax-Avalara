namespace Sitecore.Commerce.Plugin.Avalara.Models
{
    public class LineTaxRecord
    {
        public string LineItemId { get; set; }
        public decimal LineQuantity { get; set; }
        public string TaxId { get; set; }
        public string LineTaxCode { get; set; }

        public decimal LineTotal { get; set; }
        public decimal LineTaxAmount { get; set; }
        public double LineTax { get; set; }
        public double LineDiscount { get; set; }


    }
}