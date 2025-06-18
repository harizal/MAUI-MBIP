using Android.Graphics;
namespace AndroidCompound5.AimforceUtils
{
	public static class BarcodeImage
	{
		private static int _defaultSize = 400;
		private static int small_size = 20;

		public static Bitmap CreateBitmapBarcode2(string sValue, int size = 0, int height = 50, int margin = 0)
		{
			if (size == 0) size = _defaultSize;


			//First barcode is used to test the width of the actual barcode generated
			var barcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.CODE_128,
				//Format = ZXing.BarcodeFormat.CODE_39,
				Options = new EncodingOptions
				{
					Width = 0,
					Height = 0,
					PureBarcode = true
				}
			};
			barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			var bitmap = barcodeWriter.Write(sValue);

			//Used the actual barcode width to generate the actual barcode image but not wider than given size
			if ((bitmap.Width * 2) < size)
				size = bitmap.Width * 2;

			var actualBarcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.CODE_128,
				Options = new EncodingOptions
				{
					Width = size,
					Height = height,
					PureBarcode = true
				}
			};
			actualBarcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			bitmap = actualBarcodeWriter.Write(sValue);

			return bitmap;
		}

		public static Bitmap CreateBitmapBarcodePDF417(string sValue, int size = 0, int height = 50, int margin = 0)
		{
			if (size == 0) size = _defaultSize;

			//First barcode is used to test the width of the actual barcode generated
			var barcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.PDF_417,
				Options = new EncodingOptions
				{
					//Width = size,
					//Height = height,
					Width = 0,
					Height = 0,
					//Margin = 5  
					PureBarcode = true
				}
			};
			barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			var bitmap = barcodeWriter.Write(sValue);

			//Used the actual barcode width to generate the actual barcode image but not wider than given size
			if ((bitmap.Width * 2) < size)
				size = bitmap.Width * 2;

			var actualBarcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.PDF_417,
				Options = new EncodingOptions
				{
					Width = size,
					Height = height,
					PureBarcode = true
				}
			};
			actualBarcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			bitmap = actualBarcodeWriter.Write(sValue);

			return bitmap;
		}

		public static Bitmap CreateBitmapBarcodeQRCode(string sValue, int width = 200, int height = 200, int margin = 0)
		{
			if (width == 0) width = _defaultSize;
			if (height == 0) height = _defaultSize;

			//First barcode is used to test the width of the actual barcode generated
			var barcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new EncodingOptions
				{
					Width = width,
					Height = height,
					Margin = margin,
					//Width = 0,
					//Height = 0,
					//Margin = 5  
					PureBarcode = true
				}
			};


			barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			var bitmap = barcodeWriter.Write(sValue);

			//Used the actual barcode width to generate the actual barcode image but not wider than given size
			if ((bitmap.Width * 2) < width)
				width = bitmap.Width * 2;

			var actualBarcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new EncodingOptions
				{
					Width = width,
					Height = height,
					PureBarcode = true
				}
			};
			actualBarcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
			bitmap = actualBarcodeWriter.Write(sValue);

			return bitmap;

		}
	}
}
