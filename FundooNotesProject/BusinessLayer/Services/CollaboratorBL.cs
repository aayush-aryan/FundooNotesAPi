using BusinessLayer.interfaces;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollaboratorBL : ICollaboratorBL
    {
        ICollaboratorRL collaboratorRL;
        public CollaboratorBL(ICollaboratorRL collaboratorRL)
        {
            this.collaboratorRL = collaboratorRL;
        }

        public CollaboratorEntity AddCollaborator(CollaboratorModel collabaoratorModel, int userID)
        {
            return collaboratorRL.AddCollaborator(collabaoratorModel, userID);
        }

        public IEnumerable<CollaboratorEntity> GetCollaboratorsByID(int NoteId, int userID)
        {
            return collaboratorRL.GetCollaboratorsByID(NoteId, userID);
        }

        public bool RemoveCollaborator(int CollaboratorId, int userID)
        {
            return collaboratorRL.RemoveCollaborator(CollaboratorId, userID);
        }
    }
}
