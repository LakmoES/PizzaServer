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
        private string guestIDSecuenceName = "GuestID_sequence";
        //задать имена таблиц и схемы БД
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schemaName);
            modelBuilder.Entity<User>().ToTable("user", schemaName);
            modelBuilder.Entity<Token>().ToTable("token", schemaName);
            modelBuilder.Entity<UserTelephone>().ToTable("usertelephone", schemaName);
            modelBuilder.Entity<UserAddress>().ToTable("useraddress", schemaName);
            modelBuilder.Entity<Product>().ToTable("product", schemaName);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserTelephone> UserTelephones { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Product> Products { get; set; }

        public Int64 GetNextGuestIDValue()
        {
            var rawQuery = Database.SqlQuery<Int64>(String.Format("SELECT NEXT VALUE FOR {0}.{1};", schemaName, guestIDSecuenceName));
            var task = rawQuery.SingleAsync();
            Int64 nextVal = task.Result;

            return nextVal;
        }
    }
}