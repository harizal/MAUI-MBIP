using System;

namespace AndroidCompound5.BusinessObject.DTOs.Responses
{
    public class CompoundDetailDto
    {
        public string councilid { get; set; }
        public string searchtype { get; set; }
        public string searchvalue { get; set; }
        public string nokompaun { get; set; }
        public string nokenderaan { get; set; }
        public string noic { get; set; }
        public DateTime tarikhkmp { get; set; }
        public string kesalahan { get; set; }
        public string kodhasil { get; set; }
        public string amnperlubyr { get; set; }
        public string statuskmp { get; set; }
        public string lokasikesalahan { get; set; }
        public string nosyarikat { get; set; }
        public string namasyarikat { get; set; }
        public string namapesalah { get; set; }

        public string ERROR { get; set; }
    }
}