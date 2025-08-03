using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class StreetDto : BaseDto
	{
        public string Code { get; set; }//[4] ;			// Street Code
        public string Zone { get; set; }//[4] ;			// Zone Code
        public string LongDesc { get; set; }//[40] ;	// Long description
        public string ShortDesc { get; set; }//[15] ;	// Short description
        public string Mukim { get; set; }//[2] ;	// Mukim Code
    }
}
