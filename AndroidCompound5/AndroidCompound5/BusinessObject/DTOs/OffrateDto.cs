using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class OffrateDto : BaseDto
	{
        public string OfdCode { get; set; } //[10] ;		    // Offend code
        public string ActCode { get; set; } //[10] ;		// Act Code
        public string CarCategory { get; set; } //[1] ;		// Car Category Code
        public string Description { get; set; } = string.Empty; //[80] ;	// Short Description 
        public string OffendAmt { get; set; } //[10] ;		// Offence Amount
        public string OffendAmt2 { get; set; } //[10] ;		// Offence Amount
        public string OffendAmt3 { get; set; } //[10] ;		// Offence Amount

    }
}
