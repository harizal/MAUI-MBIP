using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;

namespace AndroidCompound5
{
    public static class MessageAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static MessageDto GetMessageAccess(string strFullFileName)
		{
			var objStream = new StreamReader(strFullFileName);

			var Message = new MessageDto();

			string sLine = "";
			int len = 0;


			sLine = objStream.ReadLine();
			if (sLine.Length > 0)
			{
				Message.TelNo = GeneralUtils.ValidateRecordByLine(sLine, 0, 50, ref len).Trim();
				Message.Desc = GeneralUtils.ValidateRecordByLine(sLine, len, 300, ref len).Trim();
			}

			objStream.Close();
			objStream.Dispose();

			return Message;
		}
		public static MessageDto? GetMessageAccess()
        {
            return DbContextProvider.Instance?.Messages?.FirstOrDefault() ?? null;
        }
    }
}