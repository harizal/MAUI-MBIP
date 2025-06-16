using System;
using System.Collections.Generic;

namespace AndroidCompound5.BusinessObject.DTOs
{
    public class PrintDataDto
    {
        //public string Text { get; set; }
        //public Byte[] Byte { get; set; }

        //public bool IsByte { get; set; }

        ////for second print copy 
        //public bool IsReplace { get; set; }
        //public string ReplaceData { get; set; }
        //public string ReplaceBy { get; set; }
        //public int PaddingLeft { get; set; }

        public List<PrintDataItem> DataItems { get; set; } = new List<PrintDataItem>();
        public List<PrintFreqItem> FreqItems { get; set; } = new List<PrintFreqItem>();
      
    }

    public class PrintDataItem
    {
        public string Text { get; set; }
        public Byte[] Byte { get; set; }

        public bool IsByte { get; set; }
    }

    public class PrintFreqItem
    {
        public bool UpdateStat { get; set; }
        public string TableName { get; set; }
        public int PrintFreqValue { get; set; }
        public string DocumentNumber { get; set; }
        public int SeqNo { get; set; }
    }
}