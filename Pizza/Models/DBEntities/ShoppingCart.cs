using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class ShoppingCart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, Index("ShoppingCart_ProductUser_unique", 1, IsUnique = true)]
        public int product { set; get; }
        [ForeignKey("product")]
        public virtual Product Product { set; get; }

        [Required]
        public int amount { set; get; }

        [Required, Index("ShoppingCart_ProductUser_unique", 2, IsUnique = true)]
        public int user { set; get; }
        [ForeignKey("user")]
        public virtual User User { set; get; }
    }
}