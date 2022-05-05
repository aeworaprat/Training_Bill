using System;
using System.Collections.Generic;

namespace bill.Models
{
    public partial class list
    {
        public int list_id { get; set; }
        public decimal list_quantity { get; set; }
        public decimal list_price { get; set; }
        public decimal list_discount { get; set; }
        public decimal list_discount_bath { get; set; }
        public decimal list_total_price { get; set; }
        public int list_item_id { get; set; }
        public int list_bill_id { get; set; }

        public virtual receipt list_bill { get; set; } = null!;
        public virtual item list_item { get; set; } = null!;
    }
}
