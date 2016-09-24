using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Pizza.Models.DBEntities;

namespace Pizza.Models.Auth.DBEntities
{
    public class Token
    {
        [Key, MaxLength(40)]
        public string hash { set; get; }

        [Required, MaxLength(20)]
        public string ip { set; get; }

        [Required]
        public DateTime createdate { set; get; }

        [Required]
        public DateTime expdate { set; get; }

        [Required]
        public int user { set; get; }
        [ForeignKey("user")]
        public virtual User User { set; get; }
    }
}