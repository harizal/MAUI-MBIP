namespace AndroidCompound5.BusinessObject.DTOs
{
    public class CarTypeDto
    {
        public string Code { get; set; }//[3] ;			// Car code
        public string CarcategoryCode { get; set; }			// Car type
        public string LongDescCode { get; set; }//[40] ;		// Long description
        public string ShortDescCode { get; set; }//[17] ;		// Short description
    }
}
