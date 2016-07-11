using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Bill
    {
        //"id" "client" "status" "promocode" "delivery" "staff" "date" "cost"
        [Key]
        public int id { set; get; }
        public int client { set; get; }
        public int status { set; get; }
        public string promocode { set; get; }
        public int? delivery { set; get; }
        public int? staff { set; get; }
        public DateTime date { set; get; }
        public Decimal cost { set; get; }
    }
}