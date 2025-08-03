
using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class CompDescDto : BaseDto
	{
        public string ActCode { get; set; } //[10];		//Act Code
        public string OfdCode { get; set; } //[10];			//Offend Code
        public string ButirCode { get; set; } //[2];		// 'butir kesalahan' Code
        public string ButirDesc { get; set; } //[600];		// 'butir kesalahan' Description, same as CompDesc
    }
}
