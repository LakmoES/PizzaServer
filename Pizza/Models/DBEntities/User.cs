using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class User
    {
        [Key]
        public int id { set; get; }
        public string username { set; get; }
        public string password { set; get; }
        public string email { set; get; }
        public string name { set; get; }
        public string surname { set; get; }
        public int guest { set; get; }
    }
}