using BusinessLayer.interfaces;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }
        public LabelEntity CreateLabel(LabelModel labelModel, int userID)
        {
            return labelRL.CreateLabel(labelModel, userID);
        }

        public IEnumerable<LabelEntity> EditLabel(UpdateLabelModel updateLabel, int userID)
        {
            return labelRL.EditLabel(updateLabel, userID);
        }

        public bool RemoveLabel(int userID, string labelName)
        {
            return labelRL.RemoveLabel(userID, labelName);
        }

        public bool RemoveLabelByNoteID(LabelModel labelModel, int userID)
        {
            return labelRL.RemoveLabelByNoteID(labelModel, userID);
        }

        public IEnumerable<LabelEntity> GetLabelsByNoteID(int noteID, int userID)
        {
            return labelRL.GetLabelsByNoteID(noteID, userID);
        }
    }
}
