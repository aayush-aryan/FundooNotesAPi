using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity  
{
    public class NoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public string BgColour { get; set; }
        public bool IsArchieve { get; set; }

        [DefaultValue(false)]
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime EditedDateTime { get; set; }

        [ForeignKey("UserEntity")]
        public int userID { get; set; }  
        //lazy loading :
        public virtual UserEntity UserEntity { get; set; }
        public string Image { get; set; }
    }
}
