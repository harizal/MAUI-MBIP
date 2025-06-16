namespace AndroidCompound5.BusinessObject.DTOs
{
    public class InfoDto
    {
        public string DolphinId { get; set; }//;//[2] ;		// Dolphin ID number
        public string Council { get; set; }//;//[10] ;		// Council name
        public string AssignZone { get; set; }//;//[20] ;	// Zone assigned(max 5)
        public string BroadMsg { get; set; }//;//[60] ;		// Broadcast message
        public string StartCmp { get; set; }//;//[20] ;		// Start Compound number//exclude
        public string StartSita { get; set; }//;//[20] ;		// Start Sita number
        public string LogDate { get; set; }//;//[8] ;		// Login date//exclude
        public string LogTime { get; set; }//;//[4] ;		// Login time//exclude
        public string EnforcerId { get; set; }//;//[7] ;		// Enforcer ID		//exclude
        public string CurrMukim { get; set; }//;//[2] ;		// Current Mukim//exclude
        public string CurrZone { get; set; }//;//[4] ;		// Current zone//exclude
        public string CurrDate { get; set; }//;//[8] ;		// Current date//exclude
        public long CurrComp { get; set; }//;           // Current compound number
        public long CurrSita { get; set; }//;           // Current sita number / Notice number
        public int CompCnt { get; set; }//;         // Compound issue count
        public int SitaCnt { get; set; }//;         // Sitaan issue count
        public int NoticeCnt { get; set; }//;           // Notice issue count
        public int PhotoCnt { get; set; }//;            // Photo count
        public int NoteSize { get; set; }//;            // Note count in bytes (max.42KB)
        public int NoteCnt { get; set; }//;         // Note raises since login
        public long CurrRcpNum { get; set; }//;     // Current Receipt Number
        public int RcpCnt { get; set; }//;			// Receipt Count

    }
}
