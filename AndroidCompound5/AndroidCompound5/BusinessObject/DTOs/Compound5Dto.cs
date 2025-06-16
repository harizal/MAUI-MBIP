

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class Compound5Dto
    {
        public string CarNum { get; set; } //[15] ;		    // Car number
        public string Category { get; set; }	            // Jenis Kenderaan
        public string CarType { get; set; } //[3] ;		    // Car type
        public string CarTypeDesc { get; set; } //[40] ;	// Car type description
        public string CarColor { get; set; } //[2] ;		// Car color
        public string CarColorDesc { get; set; } //[40] ;	// Car color description
        public string RoadTax { get; set; } //[10] ;		// Road tax
        public string RoadTaxDate { get; set; } //[10] ;		// Road tax
        public string CompAmt { get; set; } //[10] ;		// Compound amount	
        public string CompAmt2 { get; set; } //[10] ;		// Compound amount
        public string CompAmt3 { get; set; } //[10] ;		// Compound amount
        public string DeliveryCode { get; set; } //[2] ;	// Delivery code
        public string Muatan { get; set; } //[20] ;	        // Muatan
        public string CompDesc { get; set; } //[600] ;		// Compound description
        public string LockTime { get; set; } //[4] ;		// Lock time
        public string LockKey { get; set; } //[5] ;		    // Lock key
        public string UnlockAmt { get; set; } //[7] ;		// Unlock amount
        public string TowAmt { get; set; } //[7] ;			// Tow amount
    }
}
