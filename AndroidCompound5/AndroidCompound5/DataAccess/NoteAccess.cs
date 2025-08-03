using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;

namespace AndroidCompound5
{
    public static class NoteAccess
    {
        public static void UpdateNoteAccess(NoteDto noteDto)
        {
            DbContextProvider.Instance.Notes.Add(noteDto);
        }

        public static void AddNoteAccess(NoteDto noteDto)
        {
            DbContextProvider.Instance.Notes.Add(noteDto);
        }

        public static List<NoteDto> GetNoteAccess(string strFullFileName)
        {
            return [.. DbContextProvider.Instance.Notes];
        }
    }
}