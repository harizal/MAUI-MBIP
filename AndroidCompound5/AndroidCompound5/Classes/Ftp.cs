using AndroidCompound5.AimforceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCompound5.Classes
{
	public class Ftp
	{
		private string host = null;
		private string user = null;
		private string pass = null;
		private FtpWebRequest ftpRequest = null;
		private FtpWebResponse ftpResponse = null;
		private Stream ftpStream = null;
		//private int bufferSize = 2048;
		private int bufferSize = 4096;


		public Ftp(string hostIP, string userName, string password)
		{
			host = hostIP;
			user = userName;
			pass = password;
		}

		public string Upload(string remoteFile, string localFile)
		{
			string message = "";
			try
			{
				ftpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + host + "/" + remoteFile);
				/* Log in to the FTP Server with the User Name and Password Provided */
				ftpRequest.Credentials = new NetworkCredential(user, pass);
				/* When in doubt, use these options */
				ftpRequest.UseBinary = true;
				ftpRequest.KeepAlive = false;// true;
				ftpRequest.UsePassive = false;
				ftpRequest.Timeout = 3600000;

				/* Specify the Type of FTP Request */
				ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

				/* Establish Return Communication with the FTP Server */
				ftpStream = ftpRequest.GetRequestStream();

				/* Open a File Stream to Read the File for Upload */
				FileStream localFileStream = System.IO.File.OpenRead(localFile);//new FileStream(localFile, FileMode.Create);

				/* Buffer for the Downloaded Data */
				byte[] byteBuffer = new byte[bufferSize];
				int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
				/* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
				try
				{
					while (bytesSent != 0)
					{
						ftpStream.Write(byteBuffer, 0, bytesSent);
						bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
					}
				}
				catch (Exception ex)
				{
					LogFile.WriteLogFile(ex.Message + "\r" + "\r" + localFile, Enums.LogType.Error);
					message = "Error Uploading " + localFile;
					LogFile.WriteLogFile("Error Uploading " + localFile);
					LogFile.WriteLogFile(ex.Message);
				}
				/* Resource Cleanup */
				localFileStream.Close();
				ftpStream.Close();
				ftpRequest = null;
				LogFile.WriteLogFile("Done Uploading " + localFile, Enums.LogType.Info);
			}
			catch (Exception ex)
			{
				ftpRequest = null;
				message = "Error Uploading " + localFile;
				LogFile.WriteLogFile("upload " + localFile + " : " + ex.Message, Enums.LogType.Error);

			}
			System.Threading.Thread.Sleep(1000);
			return message;
		}


		public string DownloadFile(string remoteFile, string localFile)
		{
			string message = "";
			FtpWebResponse response = null;
			try
			{
				int bytesRead = 0;
				//byte[] buffer = new byte[2048];
				byte[] buffer = new byte[4096];
				ftpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + host + "/" + remoteFile);
				ftpRequest.Credentials = new NetworkCredential(user, pass);
				ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
				ftpRequest.Timeout = 3600000;
				ftpRequest.KeepAlive = true;

				using (response = (FtpWebResponse)ftpRequest.GetResponse())
				{
					Stream reader = response.GetResponseStream();
					var fileStream = new FileStream(localFile, FileMode.OpenOrCreate,
						FileAccess.ReadWrite,
						FileShare.None);

					while (true)
					{
						bytesRead = reader.Read(buffer, 0, buffer.Length);

						if (bytesRead == 0)
							break;

						fileStream.Write(buffer, 0, bytesRead);
					}
					reader.Close();
					fileStream.Close();
					fileStream.Dispose();
					ftpRequest = null;
				}


			}
			catch (Exception ex)
			{
				if (response != null)
				{
					response.Close();
					response.Dispose();
				}

				ftpRequest = null;
				LogFile.WriteLogFile(ex.Message, Enums.LogType.Error);
				message = "error Downloading : " + localFile;

			}
			System.Threading.Thread.Sleep(1000);
			return message;
		}

		public string[] DirectoryList(string remotePath)
		{
			string[] directoryList;
			try
			{
				/* Create an FTP Request */
				ftpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + host + "/" + remotePath);
				/* Log in to the FTP Server with the User Name and Password Provided */
				ftpRequest.Credentials = new NetworkCredential(user, pass);
				/* When in doubt, use these options */
				ftpRequest.UseBinary = true;
				ftpRequest.UsePassive = false;
				ftpRequest.KeepAlive = false;
				/* Specify the Type of FTP Request */
				ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
				/* Establish Return Communication with the FTP Server */
				ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
				/* Establish Return Communication with the FTP Server */
				ftpStream = ftpResponse.GetResponseStream();
				/* Get the FTP Server's Response Stream */
				StreamReader ftpReader = new StreamReader(ftpStream);
				/* Store the Raw Response */
				string directoryRaw = null;
				/* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
				try
				{
					while (ftpReader.Peek() != -1)
					{
						directoryRaw += ftpReader.ReadLine() + "|";
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return null;
				}
				/* Resource Cleanup */
				ftpReader.Close();
				ftpStream.Close();
				ftpResponse.Close();
				ftpRequest = null;
				/* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
				try
				{
					directoryList = directoryRaw.Split("|".ToCharArray());
					return directoryList;
				}
				catch (Exception ex)
				{
					LogFile.WriteLogFile("Error - " + ex.Message, Enums.LogType.Error);
					directoryList = null;
					return directoryList;
				}

			}
			catch (Exception ex)
			{
				ftpRequest = null;
				LogFile.WriteLogFile("Error - " + ex.Message, Enums.LogType.Error);
				directoryList = null;
				return directoryList;
			}

		}
	}
}