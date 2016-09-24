using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Delivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        public int? address { set; get; }
        [ForeignKey("address")]
        public virtual UserAddress UserAddress { set; get; }

        public Decimal? cost { set; get; }

        public int? runner { set; get; }
        [ForeignKey("runner")]
        public virtual Runner Runner { set; get; }

        public virtual List<Bill> Bills { set; get; }
    }
}