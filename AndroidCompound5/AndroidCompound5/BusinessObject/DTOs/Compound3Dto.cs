
namespace AndroidCompound5.BusinessObject.DTOs
{
    public class Compound3Dto
    {
        public string Rujukan { get; set; } //[15] ;		// Rujukan MPS
        public string Company { get; set; } //[20] ;		// Company number
        public string CompanyName { get; set; } //[60] ;	// Company name
        public string OffenderIc { get; set; } //[16] ;	    // Offender IC
        public string OffenderName { get; set; } //[60] ;	// Offender name

        /***********   New Address format  *****************/
        //public string No { get; set; }                      //[15] ;
        //public string Building { get; set; }                //[30] ;
        //public string Jalan { get; set; }                   //[30] ;

        //public string Taman { get; set; }                   //[30] ;
        //public string PostCode { get; set; }                //[5] ;
        //public string City { get; set; }                    //[20] ;
        //public string State { get; set; }                   //[20] ;
        

        public string Address1 { get; set; } //[80] ;
        public string Address2 { get; set; } //[80] ;
        public string Address3 { get; set; } //[170] ;

        public string CompAmt { get; set; } //[10] ;		// Compound amount
        public string CompAmt2 { get; set; } //[10] ;		// Compound amount
        public string CompAmt3 { get; set; } //[10] ;		// Compound amount

        public string DeliveryCode { get; set; } //[2] ;	// Delivery code
        public string CompDesc { get; set; } //[600] ;		// Compound description
    }
}
