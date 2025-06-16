

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class UPSBParkingDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Vpl_Number { get; set; }
        public string Record_Id { get; set; }
        public string In_Time { get; set; }
        public string Out_Time { get; set; }
        public string Duration { get; set; }
        public string Amount_paid { get; set; }
    }
    public class UPSBInput
    {
        public string CompanyId { get; set; }
        public string ParkCode { get; set; }
        public string VplNumber { get; set; }
        public string Sign { get; set; }
        public string AppId { get; set; }
    }

    public class CompoundExistDto
    {
        public string CompoundNumber { get; set; }
        public string CompDate { get; set; }
        public string Lokasi { get; set; }
        public string Discname { get; set; }
        public string vStartDate { get; set; }
        public string vEndDate { get; set; }
    }

    public class ResultCompoundExistDto
    {
        public string ReturnStatus { get; set; }
        public string CompoundNumber { get; set; }
        public string CompDate { get; set; }
        public string Lokasi { get; set; }
        public string Discname { get; set; }
        public string vStartDate { get; set; }
        public string vEndDate { get; set; }
    }
}
