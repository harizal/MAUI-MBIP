namespace AndroidCompound5.BusinessObject.DTOs.Responses
{
    public class ZoneDto
    {
        public string Zone_name { get; set; }
    }

    public class DataDto
    {
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public ZoneDto Parking_zone { get; set; }
    }

    public class ResponseDto
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public DataDto Data { get; set; }
    }
}