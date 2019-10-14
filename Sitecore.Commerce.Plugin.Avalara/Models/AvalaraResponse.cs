using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Commerce.Plugin.Avalara.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public object id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int transactionLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int transactionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int addressId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string stateFIPS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemptAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemptReasonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool inState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jurisCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jurisName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int jurisdictionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string signatureCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string stateAssignedNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jurisType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int nonTaxableAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int nonTaxableRuleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string nonTaxableType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int rateRuleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int rateSourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string serCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sourcing { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double tax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxableAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxAuthorityTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxRegionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double taxCalculated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxOverride { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rateType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rateTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxableUnits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int nonTaxableUnits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemptUnits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string unitOfBasis { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LineLocationType
    {
        /// <summary>
        /// 
        /// </summary>
        public int documentLineLocationTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int documentLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int documentAddressId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string locationTypeCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Parameters
    {
    }


    /// <summary>
    /// 
    /// </summary>
    public class ResponseLine
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int transactionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string lineNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int boundaryOverrideId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerUsageType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int destinationAddressId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int originAddressId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int discountAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemptAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemptCertId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string exemptNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isItemTaxable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isSSTP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string itemCode { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public int lineAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ref1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ref2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string reportingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string revAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sourcing { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double tax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxableAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double taxCalculated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxCodeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxEngine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxOverrideType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string businessIdentificationNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int taxOverrideAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxOverrideReason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool taxIncluded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Detail> details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<LineLocationType> lineLocationTypes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Parameters parameters { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int transactionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string boundaryLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string line2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string line3 { get; set; }

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
        public string postalCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public int taxRegionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string longitude { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class LocationType
    {
        /// <summary>
        /// /
        /// </summary>
        public int documentLocationTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int documentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int documentAddressId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string locationTypeCode { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Summary
    {
        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public string jurisType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jurisCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string jurisName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxAuthorityType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string stateAssignedNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string taxName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rateType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double tax { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public double taxCalculated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int nonTaxable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exemption { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Parameters2
    {
    }


    /// <summary>
    /// 
    /// </summary>
    public class AvalaraResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int companyId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string paymentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string batchCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string currencyCode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string customerUsageType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string customerVendorCode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string exemptNo { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool reconciled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string purchaseOrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string referenceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string salespersonCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxOverrideType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int taxOverrideAmount { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string taxOverrideReason { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int totalAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int totalExempt { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public double totalTax { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int totalTaxable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double totalTaxCalculated { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string adjustmentReason { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string adjustmentDescription { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool locked { get; set; }


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
        public int version { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string softwareVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int originAddressId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int destinationAddressId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string exchangeRateEffectiveDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int exchangeRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isSellerImporterOfRecord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string businessIdentificationNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string modifiedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int modifiedUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string taxDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ResponseLine> lines { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Address> addresses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<LocationType> locationTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Summary> summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Parameters2 parameters { get; set; }
    }

}
