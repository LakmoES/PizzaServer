using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class UserTelephone
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public string number { set; get; }
        public int user { set; get; }
    }
}