using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class BillStatus
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
    }
}