using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class OrderedProduct
    {
        [Key]
        public int id { set; get; }
        public int bill { set; get; }
        public int product { set; get; }
        public int amount { set; get; }
        public Decimal cost { set; get; }
    }
}