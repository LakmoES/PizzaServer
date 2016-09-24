using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class PromoCode
    {
        [Key, MaxLength(20)]
        public string code { set; get; }

        [Required, MaxLength(50)]
        public string title { set; get; }

        [Required]
        public int discount { set; get; }

        [Required]
        public int active { set; get; }

        public virtual List<Bill> Bills { set; get; }
    }
}