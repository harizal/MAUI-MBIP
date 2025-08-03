using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class CarColorDto : BaseDto
	{
        public string Code { get; set; }//[2];		// Car Category code
        public string LongDesc { get; set; }//[40] ;		// Short description
        public string ShortDesc { get; set; }//[17] ;		// Short description
    }
}
