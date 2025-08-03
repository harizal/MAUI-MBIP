using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;


namespace AndroidCompound5
{
    /// <summary>
    /// HandheldFil = DH19.FIL
    /// </summary>
    public static class HandheldAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static List<HandheldDto> GetHandheldAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var listHandheld = new List<HandheldDto>();

			string sLine = "";
			int len = 0;
			while ((sLine = objStream.ReadLine()) != null)
			{
				if (sLine.Length > 0)
				{
					var handheld = new HandheldDto();

					handheld.HandheldID = GeneralUtils.ValidateRecordByLine(sLine, 0, 2, ref len);
					handheld.EnfID = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
					listHandheld.Add(handheld);
				}
			}

			objStream.Close();
			objStream.Dispose();

			return listHandheld;
		}
		public static List<HandheldDto> GetHandheldAccess()
        {
            return [.. DbContextProvider.Instance.Handhelds];
        }
    }
}