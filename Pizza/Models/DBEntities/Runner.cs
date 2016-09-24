using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Runner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(50)]
        public string name { set; get; }

        [Required, MaxLength(50)]
        public string surname { set; get; }

        public virtual List<Delivery> Deliveries { set; get; }

        public virtual List<RunnerTelephone> RunnerTelephones { set; get; }
    }
}