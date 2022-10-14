using BusinessLayer.interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        public NoteRL noteRL;
        public NoteBL(NoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public NoteEntity AddNote(int userId, NoteModel noteModel)
        {
            return noteRL.AddNote(userId, noteModel);
        }

        public NoteEntity GetNote(int noteId, int userId)
        {
            return noteRL.GetNote(noteId, userId);
        }
        public int DeleteUserNote(int noteId, int userId)
        {
            return noteRL.DeleteUserNote(noteId, userId);
        }
        public int UpdateUserNote(int noteId, int userId, NoteModel noteModel)
        {
            return noteRL.UpdateUserNote(noteId, userId, noteModel);
        }
        public NoteEntity GetAllNotes(int userId)
        {
            return noteRL.GetAllNotes(userId);
        }

        public NoteEntity UpdateColor(int userId, long noteID, string color)
        {
            try
            {
                return noteRL.UpdateColor(userId, noteID, color);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public NoteEntity ArchieveNotes(int userId, int noteID)
        {
            return noteRL.ArchieveNotes(userId, noteID);
        }

        public NoteEntity ToPinNotes(int userId, int noteID)
        {
            return noteRL.ToPinNotes(userId, noteID);
        }

        public NoteEntity ToTrashNotes(int userId, int noteID)
        {
            return noteRL.ToPinNotes(userId, noteID);
        }

        public bool AddImage(int noteId, IFormFile image)
        {
            return noteRL.AddImage(noteId, image);
        }
    }
}
