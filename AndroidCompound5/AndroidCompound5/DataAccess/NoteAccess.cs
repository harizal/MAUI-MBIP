
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AndroidCompound5
{
    public static class NoteAccess
    {
        public static void UpdateNoteAccess(string strFullFileName, NoteDto noteDto)
        {
            string sLineNote = "";
            bool blUpdate = false;

            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.Deleted, 1);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteCode, 2);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.CompNum, 20);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteDate, 8);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteTime, 4);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.EnforcerId, 7);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteDesc, 60);

            long length = new System.IO.FileInfo(strFullFileName).Length;
            LogFile.WriteLogFile("Length Note fil " + length);
            if (length == 0)
            {
                var fileStream = new FileStream(strFullFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                var objInfo = new StreamWriter(fileStream);
                objInfo.Write(sLineNote);
                objInfo.Flush();
                fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
                fileStream.Close();
            }
            else
            {
                StreamReader objNote = new StreamReader(strFullFileName);
                var sUpdate = new StringBuilder();
                string sLine = "";

                while ((sLine = objNote.ReadLine()) != null)
                {
                    if (sLine.Length > 0)
                    {

                        if (sLine.Substring(1, 2) == noteDto.NoteCode &&
                            sLine.Substring(3, 23).Trim() == noteDto.CompNum)
                        {
                            blUpdate = true;
                            //update with new record
                            sUpdate.Append(sLineNote + Constants.NewLine);
                        }
                        else
                        {
                            sUpdate.Append(sLine + Constants.NewLine);
                        }
                    }
                }
                objNote.Close();
                objNote.Dispose();

                //add new note record
                if (!blUpdate)
                    sUpdate.Append(sLineNote + Constants.NewLine);

                File.Delete(strFullFileName);

                var fileStream = new FileStream(strFullFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                var objWrite = new StreamWriter(fileStream);
                objWrite.Write(sUpdate);
                objWrite.Flush();
                fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
                fileStream.Close();
            }


        }

        public static void AddNoteAccess(string strFullFileName, NoteDto noteDto)
        {
            string sLineNote = "";

            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.Deleted, 1);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteCode, 2);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.CompNum, 20);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteDate, 8);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteTime, 4);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.EnforcerId, 7);
            sLineNote = GeneralUtils.SetLine(sLineNote, noteDto.NoteDesc, 60);
            sLineNote += Constants.NewLine;

            var fileStream = new FileStream(strFullFileName, FileMode.Append, FileAccess.Write, FileShare.None);
            var objNote = new StreamWriter(fileStream);
            objNote.Write(sLineNote);
            objNote.Flush();
            fileStream.Flush(true); // true ensures the OS buffer is flushed (requires .NET Core or newer Xamarin)
            fileStream.Close();

        }

        public static List<NoteDto> GetNoteAccess(string strFullFileName)
        {
            var objStream = new StreamReader(strFullFileName);

            var listNote = new List<NoteDto>();

            string sLine = "";
            int len = 0;
            while ((sLine = objStream.ReadLine()) != null)
            {
                if (sLine.Length > 0)
                {
                    var note = new NoteDto();

                    note.Deleted = GeneralUtils.ValidateRecordByLine(sLine, 0, 1, ref len);
                    note.NoteCode = GeneralUtils.ValidateRecordByLine(sLine, len, 2, ref len);
                    note.CompNum = GeneralUtils.ValidateRecordByLine(sLine, len, 20, ref len).Trim();
                    note.NoteDate = GeneralUtils.ValidateRecordByLine(sLine, len, 8, ref len);
                    note.NoteTime = GeneralUtils.ValidateRecordByLine(sLine, len, 4, ref len);
                    note.EnforcerId = GeneralUtils.ValidateRecordByLine(sLine, len, 7, ref len).Trim();
                    note.NoteDesc = GeneralUtils.ValidateRecordByLine(sLine, len, 60, ref len).Trim();

                    listNote.Add(note);
                }
            }

            objStream.Close();
            objStream.Dispose();

            return listNote;
        }
    }
}