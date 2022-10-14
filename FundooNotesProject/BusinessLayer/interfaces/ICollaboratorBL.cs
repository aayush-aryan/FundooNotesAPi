using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.interfaces
{
   public interface ICollaboratorBL
    {
        public CollaboratorEntity AddCollaborator(CollaboratorModel collabaoratorModel, int userID);
        public IEnumerable<CollaboratorEntity> GetCollaboratorsByID(int NoteId, int userID);
        public bool RemoveCollaborator(int CollaboratorId, int userID);
    }
}
