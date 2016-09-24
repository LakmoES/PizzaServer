using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Pizza.Models.Auth.DBEntities;

namespace Pizza.Models.DBEntities
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(50), Index(IsUnique = true)]
        public string username { set; get; }

        [Required, MaxLength(50)]
        public string password { set; get; }

        [MaxLength(50)]
        public string email { set; get; }

        [MaxLength(50)]
        public string name { set; get; }

        [MaxLength(50)]
        public string surname { set; get; }

        [Required]
        public int guest { set; get; }

        [Required]
        public DateTime registrationdate { set; get; }

        public virtual List<Bill> Bills { set; get; }
        public virtual List<ShoppingCart> ShoppingCarts { set; get; }
        public virtual List<Token> Tokens { set; get; }
        public virtual List<UserAddress> UserAddresses { set; get; }
        public virtual List<UserTelephone> UserTelephones { set; get; }
    }
}