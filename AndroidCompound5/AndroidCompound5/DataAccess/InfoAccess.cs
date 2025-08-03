using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;

namespace AndroidCompound5
{
    /// <summary>
    /// InfoDat = DH01.DAT
    /// </summary>
    public static class InfoAccess
    {
		[Obsolete("This method is deprecated. Please use NewMethod() instead.")]
		public static InfoDto GetInfoAccess(string strFullFileName)
		{
			var objInfo = new StreamReader(strFullFileName);

			var info = new InfoDto();

			try
			{
				string sLine;
				int len = 0;

				while ((sLine = objInfo.ReadLine()) != null)
				{
					//Console.WriteLine(sLine);
					if (sLine.Length > 0)
					{
						info.DolphinId = GeneralUtils.ValidateRecordByLine(sLine, 0, 2, ref len);
						info.Council = GeneralUtils.ValidateRecordByLine(sLine, len, 10, ref len);
						info.AssignZone = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
						info.BroadMsg = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len);
						info.StartCmp = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
						info.StartSita = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len);
						info.LogDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
						info.LogTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len);
						info.CurrMukim = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
						info.CurrZone = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.CurrDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);

						string sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
						info.CurrComp = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
						info.CurrSita = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.CompCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.SitaCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.NoticeCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.PhotoCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.NoteSize = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.NoteCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
						info.CurrRcpNum = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

						sTemp = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
						info.RcpCnt = GeneralUtils.IsNumeric(sTemp) ? Convert.ToInt32(sTemp) : 0;

					}//end if
				}//end while
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			finally
			{

				objInfo.Close();
				objInfo.Dispose();
			}

			return info;
		}

		public static InfoDto? GetInfoAccess()
        {
            return DbContextProvider.Instance?.Infos?.FirstOrDefault() ?? null;
        }

        public static void UpdateInfoAccess(InfoDto infoDto, Enums.FormName formName)
        {
            DbContextProvider.Instance.Infos.Update(infoDto);
            LogFile.WriteLogFile("FormName : " + formName.ToString());
        }
    }
}