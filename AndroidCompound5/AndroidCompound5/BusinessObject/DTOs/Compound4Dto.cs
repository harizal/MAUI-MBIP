
namespace AndroidCompound5.BusinessObject.DTOs
{
    public class Compound4Dto
    {
       
        public string Rujukan { get; set; } //[15] ;		// Rujukan MPS
        public string OffenderIc { get; set; } //[16] ;	    // Offender IC
        public string OffenderName { get; set; } //[60] ;	// Offender name

        /***********   New Address format  *****************/
        public string No { get; set; } //[15] ;
        public string Building { get; set; } //[30] ;
        public string Jalan { get; set; } //[30] ;
        public string Taman { get; set; } //[30] ;
        public string PostCode { get; set; } //[5] ;
        public string City { get; set; } //[20] ;
        public string State { get; set; } //[20] ;
        /***************************************************/

        public string CompAmt { get; set; } //[10] ;		// Compound amount
        public string CompAmt2 { get; set; } //[10] ;		// Compound amount
        public string CompAmt3 { get; set; } //[10] ;		// Compound amount

        public string StorageAmt { get; set; } //[6] ;		// Storage amount
        public string TransportAmt { get; set; } //[6] ;	// Transportation amount
        public string Remark { get; set; } //[60] ;		    // Remark

       

        ////put in the last
        //public string Address1 { get; set; } //[80] ;
        //public string Address2 { get; set; } //[80] ;
        //public string Address3 { get; set; } //[170] ;

    }
}
