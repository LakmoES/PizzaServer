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
        public string small { set; get; }
        public string medium { set; get; }
        public string large { set; get; }
    }
}