using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class OrderedProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required]
        public int bill { set; get; }
        [ForeignKey("bill")]
        public virtual Bill Bill { set; get; }

        [Required]
        public int product { set; get; }
        [ForeignKey("product")]
        public virtual Product Product { set; get; }

        [Required]
        public int amount { set; get; }

        public decimal? cost { set; get; }
    }
}