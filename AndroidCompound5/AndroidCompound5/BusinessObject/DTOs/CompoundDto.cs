namespace AndroidCompound5.BusinessObject.DTOs
{
    public class CompoundDto
    {
        public CompoundDto()
        {
            Compound1Type = new Compound1Dto();
            Compound2Type = new Compound2Dto();
            Compound3Type = new Compound3Dto();
            Compound4Type = new Compound4Dto();
            Compound5Type = new Compound5Dto();

        }
        public string Deleted { get; set; } //[1];			// Deletion mark (space/*)
        public string CompNum { get; set; } //[20] ;		// Dolphin ID + compound number
        public string CompType { get; set; } //;			// Compound type
        public string ActCode { get; set; } //[10] ;		// Act code
        public string OfdCode { get; set; } //[10] ;		    // Offend code
        public string Mukim { get; set; } //[6] ;			// Mukim code
        public string Zone { get; set; } //[10] ;			// Zone code
        public string StreetCode { get; set; } //[10] ;		// Street code
        public string StreetDesc { get; set; } //[100] ;	// Street description
        public string CompDate { get; set; } //[8] ;		// Compound date
        public string CompTime { get; set; } //[4] ;		// Compound time
        public string EnforcerId { get; set; } //[7] ;		// Enforcer ID
        public string WitnessId { get; set; } //[4] ;		// Witness ID
        public string PubWitness { get; set; } //[60] ;	    // Public Witness name
        public string PrintCnt { get; set; } //;			// Printed counter
        public string Tempatjadi { get; set; } //[150] ;	// Tempat Kejadian
        public string Tujuan { get; set; } //[100] ;
        public string Perniagaan { get; set; } //[100] ;
        public string Kadar { get; set; } //[1] ;
        public string GpsX { get; set; }	//[15] ;
        public string GpsY { get; set; }    //[15] ;
        public string TempohDate { get; set; }	//[8] ;

        public Compound1Dto Compound1Type { get; set; }
        public Compound2Dto Compound2Type { get; set; }
        public Compound3Dto Compound3Type { get; set; }
        public Compound4Dto Compound4Type { get; set; }
        public Compound5Dto Compound5Type { get; set; }


        public int TotalPhoto { get; set; }
        public string OffendDesc { get; set; }//use in view compound
        public string ZoneDesc { get; set; }
        public string NoteDesc { get; set; }

    }
}
