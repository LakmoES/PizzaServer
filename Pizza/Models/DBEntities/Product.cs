using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Product
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
        public int measure { set; get; }
        public int type { set; get; }
        public Decimal cost { set; get; }
        public int available { set; get; }
        public int advertising { set; get; }
        public int? image { set; get; }
    }
}