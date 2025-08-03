
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class ActiveParkingDto
    {
        public Enums.ParkingStatus ReturnResponse { get; set; }
        public string CarNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ScanDate { get; set; }
    }
}
