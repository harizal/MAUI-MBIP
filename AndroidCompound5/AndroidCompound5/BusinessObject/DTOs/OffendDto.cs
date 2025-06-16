namespace AndroidCompound5.BusinessObject.DTOs
{
    public class OffendDto
    {
        public string ActCode { get; set; } //[10] ;		// Act Code
        public string OfdCode { get; set; } //[10] ;		// Offend code
        public string IncomeCode { get; set; } //[8] ;		// Revenue Code
        public string ShortDesc { get; set; } //[15] ;		// Short Description
        public string LongDesc { get; set; } //[1000] ;		// Long Description
        public string OffendAmt { get; set; } //[10] ;		// Offence Amount
        public string OffendAmt2 { get; set; } //[10] ;		// Offence Amount
        public string OffendAmt3 { get; set; } //[10] ;		// Offence Amount
        public string PrnDesc { get; set; } //[20] ;		// Print Description 
        public string CompType { get; set; } // ;			// Compound type
        public string NoticeTitle1 { get; set; } //[43] ;	// Notice Title 1
        public string NoticeTitle2 { get; set; } //[43] ;	// Notice Title 2
        public string NoticeDenda { get; set; } //[650] ;	// Notice Denda
        public string Action { get; set; } //[350] ;		// Notice Action
        public string PrintFlag { get; set; } //[1] ;		// Print Note Flag

    }
}
