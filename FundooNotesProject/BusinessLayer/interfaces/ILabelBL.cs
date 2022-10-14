using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.interfaces
{
    public interface ILabelBL
    {
        public LabelEntity CreateLabel(LabelModel labelModel, int userID);
        public IEnumerable<LabelEntity> EditLabel(UpdateLabelModel updateLabel, int userID);
        public bool RemoveLabel(int userID, string labelName);
        public bool RemoveLabelByNoteID(LabelModel labelModel, int userID);
        public IEnumerable<LabelEntity> GetLabelsByNoteID(int noteID, int userID);

    }
}
