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
    public class LabelRL : ILabelRL
    {
        private readonly FundooDBContext fundooDBContext;
        public IConfiguration configuration { get; }
        public ResponseModel responseModel;
        public LabelRL(FundooDBContext fundooDBContext, IConfiguration configuration, ResponseModel responseModel)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;
            this.responseModel = responseModel;
        }
        public LabelEntity CreateLabel(LabelModel labelModel, int userID)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();
                var findlabel = fundooDBContext.LabelTable.Where(e => e.userID == userID && e.LabelName == labelModel.LabelName && e.NoteId == labelModel.NoteId).FirstOrDefault();
                if (findlabel == null)
                {
                    labelEntity.LabelName = labelModel.LabelName;
                    labelEntity.NoteId = labelModel.NoteId;
                    labelEntity.userID = userID;
                    fundooDBContext.LabelTable.Add(labelEntity);
                    fundooDBContext.SaveChanges();
                    return labelEntity;
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

        public IEnumerable<LabelEntity> EditLabel(UpdateLabelModel updateLabel, int userID)
        {
            try
            {

                IEnumerable<LabelEntity> labelEntities;
                labelEntities = fundooDBContext.LabelTable.Where(e => e.userID == userID && e.LabelName == updateLabel.LabelName).ToList();
                if (labelEntities != null)
                {
                    foreach (var label in labelEntities)
                    {
                        label.LabelName = updateLabel.NewLabelName;
                        //fundooDBContext.LabelTable.Update(label);
                    }
                    fundooDBContext.SaveChanges();
                    return labelEntities;
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

        public bool RemoveLabel(int userID, string labelName)
        {
            try
            {
                IEnumerable<LabelEntity> labelEntities;
                labelEntities = fundooDBContext.LabelTable.Where(e => e.userID == userID && e.LabelName == labelName).ToList();
                if (labelEntities != null)
                {
                    foreach (var label in labelEntities)
                    {
                        fundooDBContext.LabelTable.Remove(label);
                    }
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
                throw null;
            }
        }

        public bool RemoveLabelByNoteID(LabelModel labelModel, int userID)
        {
            try
            {
                var label = fundooDBContext.LabelTable.Where(e => e.userID == userID && e.LabelName == labelModel.LabelName && e.NoteId == labelModel.NoteId).FirstOrDefault();
                if (label != null)
                {
                    fundooDBContext.LabelTable.Remove(label);
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
                throw null;
            }
        }

        public IEnumerable<LabelEntity> GetLabelsByNoteID(int noteID, int userID)
        {
            try
            {
                var result = fundooDBContext.LabelTable.Where(e => e.NoteId == noteID && e.userID == userID).ToList();
                if (result != null)
                {
                    return result;
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
    }
}
