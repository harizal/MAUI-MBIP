using AndroidCompound5.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.AimforceUtils
{
	public static class LogFile
	{
		public static void WriteLogFile(string log)
		{
			var dtLocalTime = DateTime.Now;

			string strFile = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
							 Constants.LogPath + string.Format("Log_{0}.txt", dtLocalTime.ToString("yyyyMMdd"));

			string sLine = log;
			var fileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.None);
			var objWrite = new StreamWriter(fileStream);
			sLine = dtLocalTime.ToString("yyyy-MM-dd HH:mm:ss : ") + sLine + "\r";//Char(13);
			objWrite.WriteLine(sLine);
			objWrite.Flush();

			fileStream.Flush(true);
			fileStream.Close();
		}

		public static void WriteLogFile(string log, Enums.LogType logType)
		{
			log = logType + " : " + log;
			WriteLogFile(log);
		}

		public static void WriteLogFile(string className, string message, Enums.LogType logType)
		{
			string log = logType + " : " + className;
			log += "\r";
			log += logType + " : " + message;
			WriteLogFile(log);
		}

		public static void WriteLogFile(string className, string functionName, string message, Enums.LogType logType)
		{
			string log = logType + " : " + className;
			log += "\r";
			log += "Function Name : " + functionName;
			log += "\r";
			log += "message : " + message;
			WriteLogFile(log);
		}

		public static void WriteLogFile(string log, Enums.LogType logType, Enums.FileLogType fileLogType)
		{
			log = logType + " : " + log;
			WriteLogFile(log, fileLogType);
		}

		public static void WriteLogFile(string log, Enums.FileLogType fileLogType)
		{
			var dtLocalTime = DateTime.Now;

			string path = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath + Constants.LogPath;

			string strFile = string.Format("{0}Log{1}_{2}.txt", path, dtLocalTime.ToString("yyyyMMdd"), fileLogType.ToString());

			string sLine = log;
			var fileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.None);
			var objWrite = new StreamWriter(fileStream);
			sLine = dtLocalTime.ToString("yyyy-MM-dd HH:mm:ss : ") + sLine + "\r";//Char(13);
			objWrite.WriteLine(sLine);
			objWrite.Flush();
			fileStream.Flush(true);
			fileStream.Close();
		}
		public static void WriteMemoryLogFile(string functionname, string log)
		{
			var dtLocalTime = DateTime.Now;

			string strFile = GeneralAndroidClass.GetExternalStorageDirectory() + Constants.ProgramPath +
							 Constants.LogPath + string.Format("MemoryLog_{0}.txt", dtLocalTime.ToString("yyyyMMdd"));

			string sLine = "(" + functionname + ")" + log;

			var fileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.None);
			var objWrite = new StreamWriter(fileStream);
			sLine = dtLocalTime.ToString("yyyy-MM-dd HH:mm:ss : ") + sLine + "\r";//Char(13);
			objWrite.WriteLine(sLine);
			objWrite.Flush();
			fileStream.Flush(true);
			fileStream.Close();
		}

	}
}