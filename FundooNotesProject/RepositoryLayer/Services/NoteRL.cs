using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Account;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        private readonly FundooDBContext fundooDBContext;
        public IConfiguration configuration { get; }
        // private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        public ResponseModel responseModel;

        public NoteRL(FundooDBContext fundooDBContext, IConfiguration configuration, ResponseModel responseModel)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;
            this.responseModel = responseModel;
        }

        public NoteEntity AddNote(int userId, NoteModel noteModel)
        {
            int result = 0;
            UserEntity userdetails;
            try
            {
                userdetails = fundooDBContext.UserTable.Where(u => u.userID == userId).SingleOrDefault();
                NoteEntity noteEntity = new NoteEntity();
                if (userdetails != null)
                {
                    noteEntity.userID = userId;
                    noteEntity.Description = noteModel.Description;
                    noteEntity.Title = noteModel.Title;
                    noteEntity.BgColour = noteModel.BgColour;
                    noteEntity.CreatedDateTime = DateTime.Now;
                    noteEntity.EditedDateTime = DateTime.Now;
                    noteEntity.ReminderDate = DateTime.Now.AddDays(1);
                    var notedetails = fundooDBContext.NoteTable.Add(noteEntity);
                    result = fundooDBContext.SaveChanges();
                }
                if (result != 0)
                {
                    return noteEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw null;
            }
        }
        /// <summary>
        /// get details of user for particular Note
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public NoteEntity GetNote(int noteId, int userId)
        {
            NoteEntity userdetails = null;
            try
            {
                userdetails = fundooDBContext.NoteTable.Where(u => u.userID == userId && u.NoteId == noteId).FirstOrDefault();
            }
            catch (Exception e)
            {

            }
            return userdetails;
        }

        public NoteEntity GetAllNotes(int userId)
        {
            NoteEntity userdetails = null;
            try
            {
                userdetails = fundooDBContext.NoteTable.Where(u => u.userID == userId).FirstOrDefault();
            }
            catch (Exception e)
            {

            }
            return userdetails;
        }

        public int UpdateUserNote(int noteId, int userId, NoteModel noteModel)
        {
            int result = 0;
            try
            {
                NoteEntity userdetails = fundooDBContext.NoteTable.Where(u => u.userID == userId && u.NoteId == noteId).FirstOrDefault();
                userdetails.Description = noteModel.Description;
                userdetails.Title = noteModel.Title;
                userdetails.EditedDateTime = DateTime.Now;
                result = fundooDBContext.SaveChanges();
                if (result != 0)
                {
                    responseModel.ResponseCode = "200";
                    responseModel.ResponseMassage = "Updated note successfully";
                }
                else
                {
                    responseModel.ResponseCode = "404";
                    responseModel.ResponseMassage = "Note is not Updated";
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }

        public int DeleteUserNote(int noteId, int userId)
        {
            int result = 0;
            NoteEntity userdetails = fundooDBContext.NoteTable.Where(u => u.userID == userId && u.NoteId == noteId).FirstOrDefault();
            if (userdetails != null)
            {
                fundooDBContext.Remove(userdetails);
                result = fundooDBContext.SaveChanges();
                if (result != 0)
                {
                    responseModel.ResponseCode = "200";
                    responseModel.ResponseMassage = "Deleted note successfully";
                }
                else
                {
                    responseModel.ResponseCode = "404";
                    responseModel.ResponseMassage = "Note is not Deleted";
                }
            }
            return result;
        }

        public NoteEntity UpdateColor(int userId, long noteID, string color)
        {
            try
            {
                var note = fundooDBContext.NoteTable.Where(e => e.userID == userId && e.NoteId == noteID).FirstOrDefault();
                if (note != null)
                {
                    note.BgColour = color;
                    fundooDBContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public NoteEntity ArchieveNotes(int userId, int noteID)
        {
            NoteEntity noteEntity = null;
            try
            {
                noteEntity = fundooDBContext.NoteTable.FirstOrDefault(e => e.userID == userId && e.NoteId == noteID);
                if (noteEntity != null)
                {
                    bool checkArchieve = noteEntity.IsArchieve;
                    if (checkArchieve == true)
                    {
                        noteEntity.IsArchieve = false;
                    }
                    if (checkArchieve == false)
                    {
                        noteEntity.IsArchieve = true;
                    }
                    fundooDBContext.SaveChanges();
                    return noteEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw null;
            }
        }

        public NoteEntity ToPinNotes(int userId, int noteID)
        {
            NoteEntity noteEntity = null;
            bool checkPinned = true;
            try
            {
                noteEntity = fundooDBContext.NoteTable.FirstOrDefault(e => e.userID == userId && e.NoteId == noteID);
                if (noteEntity != null)
                {
                    checkPinned = noteEntity.IsPin;
                    if (checkPinned == true)
                    {
                        noteEntity.IsPin = false;
                    }
                    if (checkPinned == false)
                    {
                        noteEntity.IsPin = true;
                    }
                    fundooDBContext.SaveChanges();
                    return noteEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity ToTrashNotes(int userId, int noteID)
        {
            NoteEntity noteEntity = null;
            bool checkIsTrash = true;
            try
            {
                noteEntity = fundooDBContext.NoteTable.FirstOrDefault(e => e.userID == userId && e.NoteId == noteID);
                if (noteEntity != null)
                {
                    checkIsTrash = noteEntity.IsTrash;
                    if (checkIsTrash == true)
                    {
                        noteEntity.IsTrash = false;
                    }
                    if (checkIsTrash == false)
                    {
                        noteEntity.IsTrash = true;
                    }
                    fundooDBContext.SaveChanges();
                    return noteEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool AddImage(int noteId, IFormFile image)
        {
            try
            {
                CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("dghxlt009", "651423643215437", "u756xpLyy5oYLhQ5861HHafriTs");
                Cloudinary cloudinary = new Cloudinary(account);
                var stream = image.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), stream),
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                var path = uploadResult.Url;
                var note = this.fundooDBContext.NoteTable.Where(x => x.NoteId == noteId).SingleOrDefault();
                if (note != null)
                {
                    note.Image = path.ToString();
                    this.fundooDBContext.NoteTable.Update(note);
                    this.fundooDBContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
