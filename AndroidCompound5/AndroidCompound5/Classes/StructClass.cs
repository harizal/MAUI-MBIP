using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	static class StructClass
	{
		/*******************************************************
         Name of Data Structure : COMPOUND		Date: 06/05/2001
        ********************************************************/
		public struct type1_t
		{
			public string CarNum;//[15] ;		// Car number
			public string Category; // [2] Jenis Kenderaan
			public string CarType;//[3] ;		// Car type
			public string CarTypeDesc;//[40] ;	// Car type description
			public string CarColor;//[2] ;		// Car color
			public string CarColorDesc;//[40] ;	// Car color description
			public string ParkingLot;//[15] ;		// Road tax
			public string RoadTax;//[10] ;		// Road tax
			public string CompAmt;//[10] ;		// Compound amount
			public string CompAmt2;//[10] ;		// Compound amount
			public string CompAmt3;//[10] ;		// Compound amount
			public string CompAmt4;//[10] ;		// Compound amount
			public string CompDesc;//[1000] ;		// Compound description

		} //482

		public struct type2_t
		{
			public string CarNum;//[15] ;		// Car number
			public string Category;     // [2] Jenis Kenderaan
			public string CarType;//[3] ;		// Car type
			public string CarTypeDesc;//[40] ;	// Car type description
			public string CarColor;//[2] ;		// Car color
			public string CarColorDesc;//[40] ;	// Car color description
			public string RoadTax;//[10] ;		// Road tax
			public string CompAmt;//[10] ;		// Compound amount
			public string CompAmt2;//[10] ;		// Compound amount
			public string CompAmt3;//[10] ;		// Compound amount
			public string CompAmt4;//[10] ;		// Compound amount
			public string DeliveryCode;//[2] ;	// Delivery code
			public string CompDesc;//[1000] ;		// Compound description

		}//434


		public struct type3_t
		{
			public string Kategori;//[1] ;		// Kategori
			public string Rujukan;//[15] ;		// Rujukan MPS
			public string Company;//[20] ;		// Company number
			public string CompanyName;//[60] ;	// Company name
			public string OffenderIc;//[16] ;	// Offender IC
			public string OffenderName;//[60] ;	// Offender name
			public string Alamat1;//[80] ;	
			public string Alamat2;//[80] ;	
			public string Alamat3;//[170] ;	

			/***********   New Address format  *****************/
			//public string No;//[15] ;
			//public string Building;//[30] ;
			//public string Jalan;//[30] ;
			//public string Taman;//[30] ;
			//public string PostCode;//[5] ;
			//public string City;//[20] ;
			//public string State;//[20] ;
			/***************************************************/

			public string CompAmt;//[10] ;		// Compound amount
			public string CompAmt2;//[10] ;		// Compound amount
			public string CompAmt3;//[10] ;		// Compound amount
			public string CompAmt4;//[10] ;		// Compound amount
			public string DeliveryCode;//[2] ;	// Delivery code
			public string CompDesc;//[1000] ;		// Compound description

		}//644

		public struct type4_t
		{
			public string Kategori;//[1] ;
			public string Rujukan;//[15] ;		// Rujukan MPS
			public string OffenderIc;//[16] ;	// Offender IC
			public string OffenderName;//[60] ;	// Offender name
			public string Alamat1;//[80] ;	
			public string Alamat2;//[80] ;	
			public string Alamat3;//[170] ;	

			/***********   New Address format  *****************/
			//public string No;//[15] ;
			//public string Building;//[30] ;
			//public string Jalan;//[30] ;
			//public string Taman;//[30] ;
			//public string PostCode;//[5] ;
			//public string City;//[20] ;
			//public string State;//[20] ;
			/***************************************************/

			public string CompAmt;//[10] ;		// Compound amount
			public string CompAmt2;//[10] ;		// Compound amount
			public string CompAmt3;//[10] ;		// Compound amount
			public string CompAmt4;//[10] ;		// Compound amount
			public string StorageAmt;//[6] ;		// Storage amount
			public string TransportAmt;//[6] ;	// Transportation amount
			public string Remark;//[60] ;		// Remark

		}//334

		public struct type5_t
		{
			public string CarNum;//[15] ;		// Car number
			public string Category;     // [2] Jenis Kenderaan
			public string CarType;//[3] ;		// Car type
			public string CarTypeDesc;//[40] ;	// Car type description
			public string CarColor;//[2] ;		// Car color
			public string CarColorDesc;//[40] ;	// Car color description
			public string RoadTax;//[10] ;		// Road tax
			public string LockTime;//[4] ;		// Lock time
			public string LockKey;//[5] ;		// Lock key
			public string UnlockAmt;//[7] ;		// Unlock amount
			public string TowAmt;//[7] ;			// Tow amount

		}//134

		public struct compound_t
		{
			public string Deleted;          // Deletion mark (space/*)
			public string CompNum;//[16] ;		// K001170426000001 (Prefix “K” + HH ID + YYMMDD + running no) 
			public string CompType;         // Compound type
			public string ActCode;//[10] ;		// Act code
			public string OfdCode;//[10] ;		// Offend code
			public string Mukim;//[6] ;			// Mukim code
			public string Zone;//[10] ;			// Zone code
			public string StreetCode;//[10] ;		// Street code
			public string StreetDesc;//[100] ;	// Street description
			public string CompDate;//[8] ;		// Compound date
			public string CompTime;//[4] ;		// Compound time
			public string EnforcerId;//[4] ;		// Enforcer ID
			public string WitnessId1;//[4] ;		// Witness ID
			public string WitnessId2;//[4] ;		// Witness ID
			public string WitnessId3;//[4] ;		// Witness ID
			public string WitnessId4;//[4] ;		// Witness ID
			public string WitnessId5;//[4] ;		// Witness ID
			public string PubWitness;//[150] ;	// Public Witness name
			public string PrintCnt; //1     // Printed counter
			public string Tempatjadi;//[300] ;	// Tempat Kejadian
			public string IssueDate;//8
			public string IssueTime;//4
			public string Kadar;
			public string SecondSalah;//[1] ;

			public string NamaPenerima;//[80] ;	//tbkompaun.namapenerima
			public string IcPenerima;//[16] ;	//tbkompaun.icpenerima
			public string GpsX;//[15] ;	
			public string GpsY;//[15] ;	

			public type1_t Type1;
			public type2_t Type2;
			public type3_t Type3;
			public type4_t Type4;
			public type5_t Type5;

			//temp for note
			public string NoteDes;//[60] 

		}//401+2


		public struct item_t
		{
			public string CompNum;//[16] ;		// K001170426000001 (Prefix “K” + HH ID + YYMMDD + running no)
			public string Seq;//[2] ;			// Sequence
			public string Flag;             // Senang/not senang musnah (S/N)
			public string Desc;//[100] ;			// Description

		}

		/*******************************************************
         Name of Data Structure : NOTE			Date: 06/05/2001
        ********************************************************/
		public struct note_t
		{
			public string Deleted;          // Deletion mark (space/*)
			public string NoteCode;//[2] ;		// Note sequence
			public string CompNum;//[16] ;		// K001170426000001 (Prefix “K” + HH ID + YYMMDD + running no)
			public string NoteDate;//[8] ;		// Note date YYYYMMDD
			public string NoteTime;//[4] ;		// Note time HHMM
			public string EnforcerId;//[4] ;		// Enforcer ID
			public string NoteDesc;//[60] ;		// Note description

		}


		/****************************************************************
        Name of Data Structure : INFO (DH01.DAT)		Date: 06/05/2001
        *****************************************************************/
		public struct info_t
		{
			public string DolphinId;//[2] ;		// Dolphin ID number
			public string Council;//[10] ;		// Council name
			public string AssignZone;//[20] ;	// Zone assigned(max 5)
			public string BroadMsg;//[60] ;		// Broadcast message
			public string StartCmp;//[16] ;		// Start Compound number//exclude
			public string StartSita;//[10] ;		// Start Sita number
			public string LogDate;//[8] ;		// Login date//exclude
			public string LogTime;//[4] ;		// Login time//exclude
			public string CurrDate;//[8] ;		// Current date//exclude
			public string EnforcerId;//[4] ;		// Enforcer ID		//exclude
			public string CurrMukim;//[2] ;		// Enforcer ID		//exclude
			public string CurrZone;//[4] ;		// Current zone//exclude
			public long CurrComp;           // Current compound number
			public long CurrSita;           // Current sita number / Notice number
			public int CompCnt;         // Compound issue count
			public int SitaCnt;         // Sitaan issue count
			public int NoticeCnt;           // Notice issue count
			public int PhotoCnt;            // Photo count
			public int NoteSize;            // Note count in bytes (max.42KB)
			public int NoteCnt;         // Note raises since login
			public long CurrRcpNum;     // Current Receipt Number
			public int RcpCnt;          // Receipt Count
		}

		/****************************************************************
         Name of Data Structure : ENFORCER (DH05.FIL)	Date: 06/05/2001
        *****************************************************************/
		public struct enforcer_t
		{
			public string EnforcerId;//[4] ;		// Enforcer ID
			public string EnforcerName;//[60] ;	// Enforcer name
			public string EnforcerIc;//[20] ;	// Enforcer IC
			public string Password;//[4] ;		// Enforcer password
			public string Level;                // Enforcer level
			public string IncomeCode;//[8] ;   // Income Code

		}


		/****************************************************************
         Name of Data Structure : PONDOK (DH09.FIL)	Date: 06/05/2001
        *****************************************************************/
		public struct pondok_t
		{
			public string PondokId;//[4] ;		// Pondok ID
			public string PondokName;//[60] ;	// Pondok name
		}

		/****************************************************************
         Name of Data Structure : HANDHELD (DH10.FIL)	Date: 06/05/2001
        *****************************************************************/
		public struct handheld_t
		{
			public string HandheldId;//[2] ;		// handheld ID
			public string EnfId;//[6] ;             // Enforcer ID

		}


		#region CARCATEGORY (DH04.FIL)

		/****************************************************************
         Name of Data Structure : CARCATEGORY (DH04.FIL)	Date: 25/10/2002
        *****************************************************************/
		public struct carcategory_t
		{
			public string Carcategory;      // [1] Car Category code
			public string ShortDesc;//[40] ;		// Short description

		}

		public struct cartype_t             // Car type
		{
			public string Code;//[3] ;			// Car code
			public string Carcategory;          // [1] Car type
			public string LongDesc;//[40] ;		// Long description
			public string ShortDesc;//[17] ;		// Short description

		}

		public struct carcolor_t                    // Color code
		{
			public string Code;//[2] ;			// Color code
			public string LongDesc;//[40] ;		// Long description
			public string ShortDesc;//[17] ;		// Short description

		}

		public struct offend_t                  // Offence Code 
		{
			public string ActCode;//[10] ;		// Act Code
			public string OfdCode;//[10] ;		// Offend code
			public string IncomeCode;//[8] ;		// Revenue Code
			public string ShortDesc;//[300] ;		// Short Description	//tbkesalahan.prgndol -change from 15 to 300
			public string LongDesc;//[600] ;		// Long Description
			public string OffendAmt;//[10] ;		// Offence Amount	//tbkesalahan.harga
			public string OffendAmt2;//[10] ;		// Offence Amount	//tbkesalahan.harga28
			public string OffendAmt3;//[10] ;		// Offence Amount	//tbkesalahan.harga30
			public string OffendAmt4;//[10] ;		// Offence Amount	//tbkesalahan.harga4
			public string PrnDesc;//[15] ;		// Print Description
			public string CompType;         // Compound type
			public string NoticeTitle1;//[43] ;	// Notice Title 1
			public string NoticeTitle2;//[43] ;	// Notice Title 2
			public string NoticeDenda;//[650] ;	// Notice Denda
			public string Action;//[350] ;		// Notice Action
		}

		public struct zone_t                    // Zone code
		{
			public string Code;//[4] ;			// Zone code
			public string Mukim;//[2] ;			// Mukim code
			public string LongDesc;//[40] ;		// Long description
			public string ShortDesc;//[15] ;		// Short description

		}

		public struct act_t
		{
			public string Code;//[10] ;			// Act Code
			public string ShortDesc;//[40] ;		// Short Description
			public string LongDesc;//[255] ;		// Long Description

		}

		public struct delivery_t
		{
			public string Code;//[2] ;			// Delivery code
			public string ShortDesc;//[40] ;		// Description

		}

		public struct barangsita_t
		{
			public string Code;//[4] ;			// 'Sitaan' Code
			public string Flag;             // 'Sitaan' type ; 'S' = 'Senang musnah', 'N' = 'Tidak Senang musnah'
			public string Desc;//[40] ;			// 'Sitaan' Desc

		}

		public struct mukim_t
		{
			public string Code;//[2] ;			// Mukim code
			public string LongDesc;//[40] ;		// Long description	

		}

		public struct tempatjadi_t
		{
			public string Code;//[4] ;
			public string Desc1;//[150] ;	//tbtempatjadi.prgn //length change from 100 to 150

		}

		#endregion


		/****************************************************************
         Name of Data Structure : TABLE	(DH06.FIL)		Date: 06/05/2001
        *****************************************************************/
		public struct street_t                  // Street Code
		{
			public string Code;//[4] ;			// Street Code
			public string Zone;//[4] ;			// Mukim Code + Zone Code ? ask in master 4
			public string Mukim;//[2] ;
			public string LongDesc;//[40] ;		// Long description
			public string ShortDesc;//[15] ;		// Short description

		}

		/*************************************************************
        Name of Data Structure : COMPDESC (DH07.FIL)	Date: 29/01/2002
        ********************************************************/
		public struct compdesc_t
		{
			public string ActCode;//[10];		//Act Code
			public string OfdCode;//[10];			//Offend Code
			public string ButirCode;//[2];		// 'butir kesalahan' Code
			public string ButirDesc;//[1000];		// 'butir kesalahan' Description, same as CompDesc
		}

		/*****************************************************************
        Name of Data Structure : ACTTITLE (DH08.FIL)	Date: 06/02/2002
        *****************************************************************/
		public struct acttitle_t
		{
			public string ActCode;//[10];		//Hand Held Act Code
			public string Title1;//[55];			//Act Title1
			public string Title2;//[55];			//Act Title2
		}


		/********************************************************************
        Name of Data Structure : NOTICETYPE	Date: 04/07/2002 (dd/mm/yyyy)
       *********************************************************************/
		public struct Noticetype2_t
		{
			public string CarNum;//[15] ;
			public string CarCategory;  //[2]
			public string CarType;//[3] ;
			public string CarTypeDesc;//[40] ;
			public string CarColor;//[2] ;
			public string CarColorDesc;//[40] ;
			public string RoadTax;//[10] ;

		}

		public struct Noticetype3_t
		{
			public string Rujukan;//[15] ;			// Rujukan MPS
			public string Company;//[20] ;			// Company number
			public string CompanyName;//[60] ;		// Company name
			public string OffenderIc;//[16] ;		// Offender IC
			public string OffenderName;//[60] ;		// Offender name
			public string Alamat1;//[60] ;	
			public string Alamat2;//[60] ;	
			public string Alamat3;//[60] ;	

			/***********   New Address format  *****************/
			//public string No;//[15] ;
			//public string Building;//[30] ;
			//public string Jalan;//[30] ;
			//public string Taman;//[30] ;
			//public string PostCode;//[5] ;
			//public string City;//[20] ;
			//public string State;//[20] ;
			/***************************************************/

		}

		/**************************************************************************
        Name of Data Structure : NOTICE (NOTICE.FIL)	Date: 04/07/2002 (dd/mm/yyyy)
        *****************************************************************************/
		public struct notice_t
		{
			public string Deleted;              // Deletion mark (space/*)
			public string NoticeNum;//[16] ;			// Dolphin ID + compound number
			public string NoticeType;           // Notice type
			public string ActCode;//[10] ;			// Act code
			public string OfdCode;//[10] ;			// Offend code
			public string Mukim;//[6] ;				// Mukim code
			public string Zone;//[10] ;				// Zone code
			public string StreetCode;//[10] ;			// Street code
			public string StreetDesc;//[100] ;		// Street description
			public string NoticeDate;//[8] ;			// Compound date
			public string NoticeTime;//[4] ;			// Compound time
			public string EnforcerId;//[4] ;			// Enforcer ID
			public string WitnessId;//[4] ;			// Witness ID
			public string PubWitness;//[150] ;		// Public Witness name
			public string PrintCnt;             // Printed counter	

			public string DeliveryCode;//[2] ;		// Delivery Code
			public string ButirKesalahan;//[1000] ;	// 'Butir Kesalahan' Desc
			public string NoticeAction;//[350] ;		// Notice Action
			public string Tempoh;//[2] ;				// Action Duration
			public string Tempatjadi;//[300] ;		// Tempat Kejadian
			public string IssueDate;//[8] ;
			public string IssueTime;//[4] ;

			public Noticetype2_t Type2;
			public Noticetype3_t Type3;


		}



	}
}
