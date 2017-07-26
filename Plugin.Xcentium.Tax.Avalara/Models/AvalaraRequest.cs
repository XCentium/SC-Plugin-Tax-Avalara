using System.Collections.Generic;

namespace Plugin.Xcentium.Tax.Avalara.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class RequestLine
    {
        /// <summary>
        /// 
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string itemCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ShipFrom
    {
        /// <summary>
        /// 
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string postalCode { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ShipTo
    {
        /// <summary>
        /// 
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string postalCode { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PointOfOrderOrigin
    {
        /// <summary>
        /// 
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string postalCode { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PointOfOrderAcceptance
    {
        /// <summary>
        /// 
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string postalCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Addresses
    {
        /// <summary>
        /// 
        /// </summary>
        public ShipFrom shipFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ShipTo shipTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PointOfOrderOrigin pointOfOrderOrigin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PointOfOrderAcceptance pointOfOrderAcceptance { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AvalaraRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public List<RequestLine> lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string purchaseOrderNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Addresses addresses { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool commit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string currencyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
    }

}
