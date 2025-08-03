
using AndroidCompound5.AimforceUtils;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class ActiveReserveParkingDto
    {
        public Enums.ParkingStatus ReturnResponse { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Amt { get; set; }
        public string Lotstatus { get; set; }
        public string Note { get; set; }

        public string status { get; set; }
    }
}