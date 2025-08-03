namespace AndroidCompound5.AimforceUtils
{
	public static class GeneralUtils
	{
		public static string ValidateRecordByLine(string sLine, int lenStart, int lenEnd, ref int totalLen)
		{
			totalLen = lenStart + lenEnd;
			if (sLine.Length >= totalLen)
				return (sLine.Substring(lenStart, lenEnd)).Trim();

			return (" ".PadLeft(lenEnd)).Trim();

		}

		public static bool IsNumeric(string sValue)
		{
			try
			{
				Convert.ToDecimal(sValue);

				return true;
			}
			catch (Exception ex)
			{
				ex.ToString();
				return false;
			}

		}

		public static string SetLine(string sLine, string sValue, int iLen)
		{
			if (string.IsNullOrEmpty(sValue))
				sLine += " ".PadLeft(iLen);
			else if (sValue.Length < iLen)
				sLine += sValue + " ".PadLeft(iLen - sValue.Length);
			else if (sValue.Length > iLen)//cut if more than len
				sLine += sValue.Substring(0, iLen);
			else
				sLine += sValue;

			return sLine;
		}
	}
}