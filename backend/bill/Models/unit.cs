using System;
using System.Collections.Generic;

namespace bill.Models
{
    public partial class unit
    {
        public unit()
        {
            items = new HashSet<item>();
        }

        public int unit_id { get; set; }
        public string unit_name { get; set; } = null!;

        public virtual ICollection<item> items { get; set; }
    }
}
