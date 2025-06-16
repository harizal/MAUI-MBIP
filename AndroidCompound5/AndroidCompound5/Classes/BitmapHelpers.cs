using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public static class BitmapHelpers
	{
		/// <summary>
		/// This method will recyle the memory help by a bitmap in an ImageView
		/// </summary>
		/// <param name="imageView">Image view.</param>
		public static void RecycleBitmap(this ImageView imageView)
		{
			if (imageView == null)
			{
				return;
			}

			Drawable toRecycle = imageView.Drawable;
			if (toRecycle != null)
			{
				((BitmapDrawable)toRecycle).Bitmap.Recycle();
			}
		}

		//static public Bitmap ScaleImage(this string fileName, int maxWidth, int maxHeight)
		//{
		//    // First we get the the dimensions of the file on disk
		//    BitmapFactory.Options options = new BitmapFactory.Options
		//    {
		//        InPurgeable = true,
		//        InJustDecodeBounds = true
		//    };
		//    BitmapFactory.DecodeFile(fileName, options);


		//    var ratioX = (double)maxWidth / image.Width;

		//    var ratioY = (double)maxHeight / image.Height;

		//    var ratio = Math.Min(ratioX, ratioY);



		//    var newWidth = (int)(image.Width * ratio);

		//    var newHeight = (int)(image.Height * ratio);



		//    var newImage = new Bitmap(newWidth, newHeight);

		//    Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

		//    Bitmap bmp = new Bitmap(newImage);



		//    return bmp;

		//}


		/// <summary>
		/// Load the image from the device, and resize it to the specified dimensions.
		/// </summary>
		/// <returns>The and resize bitmap.</returns>
		/// <param name="fileName">File name.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options
			{
				InPurgeable = true,
				InJustDecodeBounds = true
			};
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
								   ? outHeight / height
								   : outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}

		#region New LoadAndReizeImage
		internal static async void LoadImage(ImageView imageView, string pathImage)
		{
			BitmapFactory.Options options = await BitmapHelpers.GetBitmapOptionsOfImageAsync(pathImage);
			Bitmap bitmapToDisplay = await BitmapHelpers.LoadScaledDownBitmapForDisplayAsync(pathImage, options, 150, 150);
			imageView.SetImageBitmap(bitmapToDisplay);
		}

		internal static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(string pathImage)
		{
			BitmapFactory.Options options = new BitmapFactory.Options
			{
				InJustDecodeBounds = true
			};

			// The result will be null because InJustDecodeBounds == true.
			//Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.samoyed, options);
			Bitmap result = await BitmapFactory.DecodeFileAsync(pathImage, options);

			int imageHeight = options.OutHeight;
			int imageWidth = options.OutWidth;

			//_originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

			return options;
		}

		internal static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string pathImage, BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			// Calculate inSampleSize
			options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;

			return await BitmapFactory.DecodeFileAsync(pathImage, options);
		}

		public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			// Raw height and width of image
			float height = options.OutHeight;
			float width = options.OutWidth;
			double inSampleSize = 1D;

			if (height > reqHeight || width > reqWidth)
			{
				int halfHeight = (int)(height / 2);
				int halfWidth = (int)(width / 2);

				// Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
				while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
				{
					inSampleSize *= 2;
				}
			}

			return (int)inSampleSize;
		}

		#endregion
	}
}