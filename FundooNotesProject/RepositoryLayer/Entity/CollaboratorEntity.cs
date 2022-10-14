using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }
        public string CollaboratorEmail { get; set; }

        [ForeignKey("NoteEntity")]
        public int NoteId { get; set; }
        public virtual UserEntity NoteEntity { get; set; }

        [ForeignKey("UserEntity")]
        public int userID { get; set; }
        public virtual UserEntity UserEntity { get; set; }
    }
}
