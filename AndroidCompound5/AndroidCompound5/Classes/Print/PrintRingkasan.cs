using Android.Graphics;

namespace AndroidCompound5.Classes.Print
{
	public class PrintRingkasan : PrintBase
	{
		public PrintRingkasan(ContentPage contentPage) : base(contentPage)
		{
		}

		public override Bitmap GetBitmap()
		{
			return new PrintImageBll().CreateTestBitmap();
		}
	}
}
