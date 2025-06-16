
namespace AndroidCompound5.BusinessObject.DTOs
{
    public class NoteDto
    {
        public string Deleted { get; set; } //;			// Deletion mark (space/*)
        public string NoteCode { get; set; } //[2] ;		// Note sequence
        public string CompNum { get; set; } //[20] ;		// Dolphin ID + compound number
        public string NoteDate { get; set; } //[8] ;		// Note date YYYYMMDD
        public string NoteTime { get; set; } //[4] ;		// Note time HHMM
        public string EnforcerId { get; set; } //[7] ;		// Enforcer ID
        public string NoteDesc { get; set; } //[60] ;		// Note description
        //char    eol[2] ;
    }
}
