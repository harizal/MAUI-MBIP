

using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class UPSBActiveParkingDto
    {
        public Enums.ParkingStatus ReturnResponse { get; set; }
        public string ScanDate { get; set; }
        public string Carnum { get; set; }
        public string Start_Time{ get; set; }
        public string End_Time { get; set; }
        public string Duration { get; set; }
        public string Amount_paid { get; set; }
    }
}
