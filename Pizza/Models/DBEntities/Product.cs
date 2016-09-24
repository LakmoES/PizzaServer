using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(50)]
        public string title { set; get; }

        public int measure { set; get; }
        [ForeignKey("measure")]
        public virtual Measure Measure { set; get; }

        public int type { set; get; }
        public virtual ProductType ProductType { set; get; }

        [Required]
        public decimal cost { set; get; }

        [Required]
        public int available { set; get; }

        [Required]
        public int advertising { set; get; }

        public int? image { set; get; }
        [ForeignKey("image")]
        public virtual Image Image { set; get; }

        public virtual List<OrderedProduct> OrderedProducts { set; get; }
    }
}