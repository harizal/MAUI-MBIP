using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class DeliveryDto : BaseDto
	{
        public string Code { get; set; }//[2] ;			// Delivery code
        public string ShortDesc { get; set; }//[50] ;		// Description
    }
}
