using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    /****************************************************************
    Name of Data Structure : PASSBULAN (DH11.FIL)	Date: 06/05/2001
    *****************************************************************/

    public class PassBulanDto : BaseDto
	{
        public string PassType { get; set; } //[1] ;		// B = Monthly Pass ; V = VIP
        public string SerialNum { get; set; } //[8] ;	    // Pass serial Number
        public string CarNum { get; set; } //[15] ;	        // car Number
        public string StartDate { get; set; } //[8] ;		// Start Date
        public string EndDate { get; set; }   // [8]        // End Date
    }
}
