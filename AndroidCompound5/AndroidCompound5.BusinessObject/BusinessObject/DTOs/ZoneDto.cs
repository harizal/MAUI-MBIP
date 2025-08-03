
using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class ZoneDto : BaseDto
	{
        public string Code { get; set; }//[4] ;			// Zone code
        public string Mukim { get; set; }//[2] ;			// Mukim code
        public string LongDesc { get; set; }//[40] ;		// Long description
        public string ShortDesc { get; set; }//[15] ;		// Short description
    }
}
