
using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class ActDto : BaseDto
	{
        public string Code { get; set; }//[10] ;			// Act Code
        public string ShortDesc { get; set; }//[40] ;		// Short Description
        public string LongDesc { get; set; }//[255] ;		// Long Description
        public string Title1{ get; set; }   //[70] ;		    // Title 1}
        public string Title2 { get; set; }   //[70] ;		    // Title 1}
        public string Title3 { get; set; }   //[70] ;		    // Title 1}
        public string Title4 { get; set; }   //[70] ;		    // Title 1}
    }
}
