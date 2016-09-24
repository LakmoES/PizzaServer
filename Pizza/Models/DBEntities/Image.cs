using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(150)]
        public string small { set; get; }

        [Required, MaxLength(150)]
        public string medium { set; get; }

        [Required, MaxLength(150)]
        public string large { set; get; }

        public virtual List<Product> Products { set; get; }
    }
}