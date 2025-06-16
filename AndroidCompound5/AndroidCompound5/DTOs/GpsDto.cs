namespace AndroidCompound5.DTOs
{
    public class GpsDto
    {
        public string Issend { get; set; } // 'N' or 'S'
        public string ActivityDate { get; set; } //[8] ;	// Activity Date - yyyymmdd
        public string ActivityTime { get; set; } //[4] ;	// Activity Time -HHMM
        public string GpsX { get; set; } //[15] ;
        public string GpsY { get; set; } //[15] ;
        public string Kodpguatkuasa { get; set; } //[6];	
        public string BatteryLife { get; set; } //[3] ;    // Battery Percentage in %
        public string DhId; //[3] ;
      
    }
}