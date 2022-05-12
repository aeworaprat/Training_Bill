using System.ComponentModel.DataAnnotations;

namespace bill.ViewModels
{
    public class ListViewModel
    {
        public int list_id { get; set; }
        [Required]
        public decimal list_quantity { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal list_price { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal list_discount { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal list_discount_bath { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal list_total_price { get; set; }
        [Required]
        public int list_item_id { get; set; }
        [Required]
        public int list_bill_id { get; set; }

        public virtual ReceiptViewModel? list_receipt { get; set; } = null!;
        public virtual ItemViewModel? list_item { get; set; } = null!;

    }
}
