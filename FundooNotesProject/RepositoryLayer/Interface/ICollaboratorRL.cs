using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {
        public CollaboratorEntity AddCollaborator(CollaboratorModel collabaoratorModel, int userID);
        public IEnumerable<CollaboratorEntity> GetCollaboratorsByID(int NoteId, int userID);
        public bool RemoveCollaborator(int CollaboratorId, int userID);
    }
}
