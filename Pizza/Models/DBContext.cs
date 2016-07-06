using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Pizza.Models.DBEntities;
using Pizza.Models.Auth.DBEntities;

namespace Pizza.Models
{
    public class DBContext : DbContext
    {
        private string schemaName = "dbo";
        //задать имена таблиц и схемы БД
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schemaName);
            modelBuilder.Entity<User>().ToTable("user", schemaName);
            modelBuilder.Entity<Token>().ToTable("token", schemaName);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}