using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.PrintService
{
	public class PrinterPTPBll : PrinterBaseBll
	{
		private const string ClassName = "PrinterPTPBll";


		public PrinterPTPBll()
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
			//initialized printer status
			_printerMessage = "";
			_printerStatus = 0;

			//query PTP-III printer status
			//Decimal ：29 114 n  n= 1-49, Printer status is n = 2 & 3
			//PrintChar(new Byte[3] { 29, 114, 3});
			//ASCII ：DLE EOT n
			//Decimal ：16 4 n
			//default any response BITS 1 & 4 always on value = 18
			//n = 1, response = 26 -> Printer offline, BITS=1,3,4
			//n = 2, response = 18 -> useless. Cannot detect anything, BITS=1,4 always ON
			//n = 3, response = 82 -> Printhead temperature high BITS=1,4,6
			//n = 4, response = 114 ->paper out BITS=1,4,5,6 
			//Checking sequnce as follow;
			//1. n = 1 to determine printer is offline
			//2. if printer is offline, check the reason
			//   - n = 3, is it printhead overheat.
			//   - n = 4, Paper out either cover openned or physical paper out.
			//3. n = 1, return value 18 or bits 1 & 4 are ON. printer is normal, just return.

			PrintChar(new Byte[3] { 16, 4, 1 });
			int bytes = ReadChar(200);
			var resp = ReadCharData();
			if (bytes > 0)
			{
				//PTP-III printer response  
				if ((resp[0] & 26) == 26)       //BIT 1, 3, 4  are ON
				{
					_printerMessage = "Printer Offine";
					_printerStatus = 1;

					PrintChar(new Byte[3] { 16, 4, 3 });
					bytes = ReadChar(200);
					resp = ReadCharData();
					if (bytes > 0)
					{
						if ((resp[0] & 82) == 82)       //BIT 1, 4, 6  are ON
						{
							_printerMessage += "\nPrinthead Overheat";
						}
					}

					PrintChar(new Byte[3] { 16, 4, 4 });

					bytes = ReadChar(200);
					resp = ReadCharData();
					if (bytes > 0)
					{
						if ((resp[0] & 114) == 114)       //BITS 1, 4, 5, 6 are ON
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

		/**
                        if ((resp[0] & 0x01) == 0x01)       //BIT 0  = 2^0
                        {
                            _printerMessage = "0";
                            _printerStatus = 1;
                        }

                        if ((resp[0] & 0x01) == 0x01)       //BIT 0  = 2^0
                        {
                            _printerMessage = "0";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 0x02) == 0x02)       //BIT 1  = 2^1
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "1";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 0x04) == 0x4)        //BIT 2  = 2^2
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "2";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 0x08) == 0x8)        //BIT 3  = 2^3
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "3";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 16) == 16)        //BIT 4  = 2^4
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "4";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 32) == 32)        //BIT 5  = 2^5
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "5";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 64) == 64)        //BIT 6  = 2^6
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "6";
                            _printerStatus = 1;
                        }
                        if ((resp[0] & 128) == 128)        //BIT 7  = 2^7
                        {
                            if (_printerMessage.Length > 0)
                                _printerMessage += ",";
                            _printerMessage += "7";
                            _printerStatus = 1;
                        }
         * */
	}
}
