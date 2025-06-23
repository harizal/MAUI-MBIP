using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.PrintService
{
	public class PrinterSPRTBll : PrinterBaseBll
	{
		private const string ClassName = "PrinterSPRTBll";
		//PrinterInstance PrintInstance;

		public PrinterSPRTBll()
		{

		}

		public override void PrintInitialise()
		{
			Byte[] FontNormal = new Byte[3] { 27, 33, 0 };       //48 columns                0        
			Byte[] InitializePrinter = new Byte[2] { 27, 64 };    //Esc @
			PrintChar(InitializePrinter);
			PrintChar(FontNormal);
		}

		public override int PrinterQuery()
		{
#if PrintFile
            return 0;
#endif
			//#if DEBUG || DebugSM29 || KoelmesDebug
			//            return 0;
			//#endif
			//initialized printer status
			_printerMessage = "";
			_printerStatus = 0;

			//query SPRT printer status
			//ASCII ：DLE EOT n
			//Decimal ：16 4 n   1<= n <= 4

			//SPRT printer response  
			//BIT 1, 4  are always ON
			PrintChar(new Byte[3] { 16, 4, 1 });
			int bytes = ReadChar(200);
			var resp = ReadCharData();
			if (bytes > 0)
			{
				if ((resp[0] & 26) == 26)       //BIT 3 = ON
				{
					_printerMessage = "Printer Offine";
					_printerStatus = 1;

					PrintChar(new Byte[3] { 16, 4, 2 });
					bytes = ReadChar(200);
					resp = ReadCharData();
					if (bytes > 0)
					{
						if ((resp[0] & 4) == 4)              //BIT 2 = ON
							_printerMessage += "\nCover open";
						else if ((resp[0] & 8) == 8)         //BIT 3 = ON
							_printerMessage += "\nPaper Feed";
						else if ((resp[0] & 32) == 32)       //BIT 5 = ON
							_printerMessage += "\nPaper Out";
						else if ((resp[0] & 64) == 64)       //BIT 6 = ON   //unknown error
						{
							_printerMessage += "\nPrinter error";
							//Further cecking on printer error
							//PrintChar(new Byte[3] { 16, 4, 3 });
							//bytes = ReadChar(200);
							//resp = ReadCharData();
							//if (bytes > 0)
							//{
							//    if ((resp[0] & 8) == 8)       //BITS 3 = ON
							//    {
							//        _printerMessage += "\nPaper cutter error";
							//    }
							//    else if ((resp[0] & 32) == 32)    //BITS 5 = ON
							//    {
							//        _printerMessage += "\nUnrecoverable error";
							//    }
							//    else if ((resp[0] & 64) == 64)    //BITS 6 = ON
							//    {
							//        _printerMessage += "\nAuto-recoverable error";
							//    }
							//}
						}
					}


					//Paper sensors output, not applicable to this program control
					PrintChar(new Byte[3] { 16, 4, 4 });
					bytes = ReadChar(200);
					resp = ReadCharData();
					if (bytes > 0)
					{
						if ((resp[0] & 12) == 12)       //BITS 2, 3 are ON
						{
							_printerMessage += "\nPaper out soon";
						}
						else if ((resp[0] & 96) == 96)       //BITS 5, 6 are ON
						{
							_printerMessage += "\nCover Openned/Paper Out";
						}
					}
				}
				else if ((resp[0] & 18) == 18)       //BIT 1, 4  are ON  (Printer Normal)
				{
					_printerStatus = 0;
				}
				else
				{
					//undetect error
					_printerMessage = $"Unknown printer response ({resp[0].ToString()})";
					_printerStatus = 1;
				}

				if (_printerStatus == 0)
				{
					_printerMessage = "Printer OK";
				}
			}
			else
			{
				if (_printerMessage.Length > 0)
				{ _printerMessage += "\n"; }
				_printerMessage += "Printer no response";
				_printerStatus = -1;
			}
			return _printerStatus;
		}

	}
}