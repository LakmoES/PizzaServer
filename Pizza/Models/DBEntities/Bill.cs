using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pizza.Models.DBEntities
{
    public class Bill
    {
        //"id" "client" "status" "promocode" "delivery" "staff" "date" "cost"
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }

        public int? client { set; get; }
        [ForeignKey("client")]
        public virtual User User { get; set; }

        public int? status { set; get; }
        [ForeignKey("status")]
        public virtual BillStatus BillStatus { get; set; }

        public string promocode { set; get; }
        [ForeignKey("promocode")]
        public virtual PromoCode PromoCode { set; get; }

        public int? delivery { set; get; }
        [ForeignKey("delivery")]
        public virtual Delivery Delivery { set; get; }

        public int? staff { set; get; }
        [ForeignKey("staff")]
        public virtual Staff Staff { set; get; }

        public Decimal cost { set; get; }

        [Required]
        public DateTime date { set; get; }

        public virtual List<OrderedProduct> OrderedProducts { set; get; }
    }
}