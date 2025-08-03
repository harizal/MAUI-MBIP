using AndroidCompound5.BusinessObject.BusinessObject.DTOs;

namespace AndroidCompound5.BusinessObject.DTOs
{
    /****************************************************************
    Name of Data Structure : ENFORCER (DH05.FIL)	Date: 06/05/2001
    *****************************************************************/

    public class EnforcerDto : BaseDto
	{
        public string EnforcerId { get; set; } //[7] ;		// Enforcer ID
        public string EnforcerName { get; set; } //[60] ;	// Enforcer name
        public string EnforcerIc { get; set; } //[14] ;	    // Enforcer IC
        public string Password { get; set; } //[10] ;		// Enforcer password
        public string Level { get; set; }                   // Enforcer level
        public string EnforcerUnit { get; set; } //[50] ;   // EnforcerUnit
        public string KodJabatan { get; set; } //[3] ;      // Kod Jabatan
        public string Jabatan { get; set; } //[50] ;      // Jabatan
        public string KodKaunter { get; set; } //[8] ;      // Kod Kaunter
        public string KodCetak { get; set; } //[10] ;      // Kod Kaunter
    }
}
