using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Pizza.Models.DBEntities;
using Pizza.Models.Auth.DBEntities;
using System.Data.Entity.Core.Objects;

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
            modelBuilder.Entity<Measure>().ToTable("measure", schemaName);
            modelBuilder.Entity<ProductType>().ToTable("producttype", schemaName);
            modelBuilder.Entity<ShoppingCart>().ToTable("shoppingcart", schemaName);
            modelBuilder.Entity<BillStatus>().ToTable("billstatus", schemaName);
            modelBuilder.Entity<PromoCode>().ToTable("promocode", schemaName);
            modelBuilder.Entity<Bill>().ToTable("bill", schemaName);
            modelBuilder.Entity<OrderedProduct>().ToTable("orderedproduct", schemaName);
            modelBuilder.Entity<Delivery>().ToTable("delivery", schemaName);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserTelephone> UserTelephones { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<BillStatus> BillStatuses { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        public Int64 GetNextGuestIDValue()
        {
            var rawQuery = Database.SqlQuery<Int64>(String.Format("SELECT NEXT VALUE FOR {0}.{1};", schemaName, guestIDSecuenceName));
            var task = rawQuery.SingleAsync();
            Int64 nextVal = task.Result;

            return nextVal;
        }
    }
}