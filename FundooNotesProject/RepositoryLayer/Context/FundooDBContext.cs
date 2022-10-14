using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    //DbContext is a predefine class inside EntityFrameworkCore  responsible for interacting with data as objects
    public class FundooDBContext : DbContext
    {/// <summary>
     /// DbContext corresponds to your database (or a collection of tables and views in your database)
     /// whereas a DbSet corresponds to a table or view in your database.
     /// </summary>
     /// <param name="options"></param>
        public FundooDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<NoteEntity> NoteTable { get; set; }
        public DbSet<CollaboratorEntity> CollaboratorTable { get; set; }
        public DbSet<LabelEntity> LabelTable { get; set; }
    }
}
