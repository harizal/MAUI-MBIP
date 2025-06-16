using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using System.IO;

namespace AndroidCompound5
{
    public static class MessageAccess
    {
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
    }
}