using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class RunnerTelephone
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        [Required, MaxLength(30)]
        public string number { set; get; }

        [Required]
        public int runner { set; get; }
        [ForeignKey("runner")]
        public virtual Runner Runner { set; get; }
    }
}