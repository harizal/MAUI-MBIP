using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.PrintService
{
	public class PrinterWoosimBll : PrinterBaseBll
	{
		private const string ClassName = "WoosimBll";

		public PrinterWoosimBll()
		{

		}

		public override void PrintInitialise()
		{
			Byte[] FontNormal = new Byte[3] { 27, 33, 0 };       //48 columns                0        
			Byte[] InitializePrinter = new Byte[2] { 27, 64 };    //Esc @
			PrintChar(InitializePrinter);
			PrintChar(FontNormal);
		}

		public static implicit operator PrinterWoosimBll(PrinterPTPBll v)
		{
			throw new NotImplementedException();
		}
	}
}
