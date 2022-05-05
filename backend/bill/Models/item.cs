using System;
using System.Collections.Generic;

namespace bill.Models
{
    public partial class item
    {
        public item()
        {
            lists = new HashSet<list>();
        }

        public int item_id { get; set; }
        public string? item_code { get; set; }
        public string item_name { get; set; } = null!;
        public decimal item_price { get; set; }
        public int item_unit_id { get; set; }

        public virtual unit item_unit { get; set; } = null!;
        public virtual ICollection<list> lists { get; set; }
    }
}
