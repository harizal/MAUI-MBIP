using AndroidCompound5.BusinessObject.BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class Compound2Dto : BaseDto
	{
        public string CarNum { get; set; } //[15] ;		    // Car number
        public string Category { get; set; }	            // Jenis Kenderaan
        public string CarType { get; set; } //[3] ;		    // Car type
        public string CarTypeDesc { get; set; } //[40] ;	// Car type description
        public string CarColor { get; set; } //[2] ;		// Car color
        public string CarColorDesc { get; set; } //[40] ;	// Car color description
        public string RoadTax { get; set; } //[10] ;		// Road tax
        public string RoadTaxDate { get; set; } //[10] ;		// Road tax
        public string CompAmt { get; set; } //[10] ;		// Compound amount	
        public string CompAmt2 { get; set; } //[10] ;		// Compound amount
        public string CompAmt3 { get; set; } //[10] ;		// Compound amount
        public string DeliveryCode { get; set; } //[2] ;	// Delivery code
        public string Muatan { get; set; } //[20] ;	        // Muatan
        public string CompDesc { get; set; } //[600] ;		// Compound description
    }
}
