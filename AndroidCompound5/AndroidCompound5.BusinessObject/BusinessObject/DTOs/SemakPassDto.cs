using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class SemakPassDto : BaseDto
	{
        public string SemakNo { get; set; } //[12] ;
        public string Zone { get; set; } //[6] ;
        public string Street { get; set; } //[10] ;
        public string StreetDesc { get; set; } //[100] ;
        public string NoPetak { get; set; } //[6] ;
        public string NamaPemohon { get; set; } //[50] ;
        public string StartDate { get; set; } //[8] ;
        public string EndDate { get; set; } //[8] ;
        public string Remark { get; set; } //[50] ;
        public string Date { get; set; } //[8] ;
        public string Time { get; set; } //[4] ;
        public string Pic1 { get; set; } //[20] ;
        public string EnfId { get; set; } //[4] ;
    }
}