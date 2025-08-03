
using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class LogServerDto : BaseDto
	{
        public string CarNo { get; set; } //[15] ;
        public string Date { get; set; } //[8] ;
        public string Time { get; set; } //[6] ;
        public string DolphinId { get; set; } //[3] ;
        public string EnforcerId { get; set; } //[7] ;
        public string Zone { get; set; } //[6] ;
        public string Sector { get; set; } //[10] ;
        public string Street { get; set; } //[10] ;
        public string Status { get; set; } //[1] ;
        public string End { get; set; } //[2] ;?
        public string Latitude { get; set; } //[15] ;?
        public string Longitude { get; set; } //[15] ;?
    }
}