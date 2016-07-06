using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Models.Auth.DBEntities
{
    public class Token
    {
        [Key]
        public string hash { get; set; }
        public string ip { get; set; }
        public DateTime createdate { get; set; }
        public DateTime expdate { get; set; }
        public int user { get; set; }
    }
}