using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly FundooDBContext fundooDBContext;
        public IConfiguration configuration { get; }
        public ResponseModel responseModel;

        public CollaboratorRL(FundooDBContext fundooDBContext, IConfiguration configuration, ResponseModel responseModel)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;
            this.responseModel = responseModel;

        }
        public CollaboratorEntity AddCollaborator(CollaboratorModel collabaoratorModel, int userID)
        {
            try
            {
                CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
                NoteEntity noteEntity = fundooDBContext.NoteTable.Where(e => e.NoteId == collabaoratorModel.NoteId && e.userID == userID).FirstOrDefault();
                if (noteEntity != null)
                {
                    collaboratorEntity.NoteId = collabaoratorModel.NoteId;
                    collaboratorEntity.CollaboratorEmail = collabaoratorModel.CollaboratorEmail;
                    collaboratorEntity.userID = userID;
                    fundooDBContext.CollaboratorTable.Add(collaboratorEntity);
                    var result = fundooDBContext.SaveChanges();
                    return collaboratorEntity;
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
        public IEnumerable<CollaboratorEntity> GetCollaboratorsByID(int NoteId, int userID)
        {
            try
            {
                var result = fundooDBContext.CollaboratorTable.Where(e => e.userID == userID && e.NoteId == NoteId).ToList();
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch (Exception)
            {
                throw null;
            }
        }

        public bool RemoveCollaborator(int CollaboratorId, int userID)
        {
            try
            {
                var collaborator = fundooDBContext.CollaboratorTable.Where(e => e.CollaboratorId==CollaboratorId && e.userID ==userID).FirstOrDefault();
                if (collaborator != null)
                {
                    fundooDBContext.CollaboratorTable.Remove(collaborator);
                    fundooDBContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
