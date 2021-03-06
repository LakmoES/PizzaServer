﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Staff
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(50), Index(IsUnique = true)]
        public string username { set; get; }

        [Required, MaxLength(50)]
        public string password { set; get; }

        [Required, MaxLength(50)]
        public string name { set; get; }

        [Required, MaxLength(50)]
        public string surname { set; get; }

        public virtual List<Staff> Staves { set; get; }
    }
}