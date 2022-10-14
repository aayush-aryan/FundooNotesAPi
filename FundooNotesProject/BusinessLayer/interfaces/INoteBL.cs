using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.interfaces
{
    public interface INoteBL
    {
        public NoteEntity AddNote(int userId, NoteModel noteModel);
        public NoteEntity GetNote(int noteId, int userId);
        public int DeleteUserNote(int noteId, int userId);
        public int UpdateUserNote(int noteId, int userId, NoteModel noteModel);
        public NoteEntity GetAllNotes(int userId);
        public NoteEntity UpdateColor(int userId, long noteID, string color);
        public NoteEntity ArchieveNotes(int userId, int noteID);
        public NoteEntity ToPinNotes(int userId, int noteID);
        public NoteEntity ToTrashNotes(int userId, int noteID);

        public bool AddImage(int noteId, IFormFile image);
    }
}
