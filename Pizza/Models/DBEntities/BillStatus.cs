﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class BillStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required/*, Index(IsUnique = true)*/]
        public string title { set; get; }

        public virtual List<Bill> Bills { get; set; }
    }
}