using System.ComponentModel.DataAnnotations;

namespace bill.ViewModels
{
    public class ReceiptViewModel
    {
        public int receipt_id { get; set; }
        public string? receipt_code { get; set; }
        public string? receipt_date { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal receipt_product_price { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal? receipt_product_discount { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal receipt_discount { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal receipt_total_price { get; set; }
        [Required]
        public ListViewModel[]? receipt_list { get; set; }
    }
}
