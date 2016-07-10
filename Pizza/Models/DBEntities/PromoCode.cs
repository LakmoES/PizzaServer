using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class PromoCode
    {
        [Key]
        public string code { set; get; }
        public string title { set; get; }
        public int discount { set; get; }
        public int active { set; get; }
    }
}