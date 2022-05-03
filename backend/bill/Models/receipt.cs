using System;
using System.Collections.Generic;

namespace bill.Models
{
    public partial class receipt
    {
        public receipt()
        {
            lists = new HashSet<list>();
        }

        public int receipt_id { get; set; }
        public string receipt_code { get; set; } = null!;
        public DateOnly receipt_date { get; set; }
        public decimal receipt_product_price { get; set; }
        public decimal? receipt_product_discount { get; set; }
        public decimal receipt_discount { get; set; }
        public decimal receipt_total_price { get; set; }

        public virtual ICollection<list> lists { get; set; }
    }
}
