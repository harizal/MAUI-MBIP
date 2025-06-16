using Android.Text;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public class TextFilter : Java.Lang.Object, IInputFilter
	{
		private string regex = "^[a-zA-Z0-9-]*$";
		private string value = "";
		public TextFilter(int type)
		{
			switch (type)
			{
				case 1:
					break;

				default:
					regex = "^[a-zA-Z0-9-]*$";
					break;
			}
		}

		public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(source.ToString(), regex))
			{
				return source;
			}
			return null;
		}
	}
}