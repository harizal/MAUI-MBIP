namespace AndroidCompound5.PrintService
{
	public class PrinterZebraBll : PrinterBaseBll
	{
		private const string ClassName = "ZebraBll";

		public PrinterZebraBll()
		{

		}

		public override void PrintInitialise()
		{
			//Byte[] FontNormal = new Byte[3] { 27, 33, 0 };       //48 columns                0        
			//Byte[] InitializePrinter = new Byte[2] { 27, 64 };    //Esc @
			//PrintChar(InitializePrinter);
			//PrintChar(FontNormal);
		}

		public static implicit operator PrinterZebraBll(PrinterPTPBll v)
		{
			throw new NotImplementedException();
		}
	}
}
