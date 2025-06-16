
namespace AndroidCompound5.BusinessObject.DTOs
{
    public class Compound1Dto
    {
        public string CarNum { get; set; } //[15] ;		    // Car number
        public string Category { get; set; }	            // Jenis Kenderaan
        public string CarType { get; set; } //[3] ;		    // Car type
        public string CarTypeDesc { get; set; } //[40] ;	// Car type description
        public string CarColor { get; set; } //[2] ;		// Car color
        public string CarColorDesc { get; set; } //[40] ;	// Car color description
        public string LotNo { get; set; } //[15] ;		    // LotNo
        public string RoadTax { get; set; } //[10] ;		// Road tax
        public string RoadTaxDate { get; set; } //[8] ;		// Road taxDate
        public string CompAmt { get; set; } //[10] ;		// Compound amount
        public string CompAmt2 { get; set; } //[10] ;		// Compound amount
        public string CompAmt3 { get; set; } //[10] ;		// Compound amount
        public string CouponNumber { get; set; } //[15] ;	// CouponNumber
        public string CouponDate { get; set; } //[8] ;		// CouponDate
        public string CouponTime { get; set; } //[4] ;		// CouponTime
        public string DeliveryCode { get; set; } //[2] ;		// DeliveryCode
        public string CompDesc { get; set; } //[600] ;		// Compound description
        public string ScanDateTime { get; set; } //[600] ;		// Smart Parking Checking Date Time yyyy-mm-dd hh:mm:ss


    }
}
