using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Delivery
    {
        [Key]
        public int id { set; get; }
        public int? address { set; get; }
        public Decimal? cost { set; get; }
        public int? runner { set; get; }
    }
}