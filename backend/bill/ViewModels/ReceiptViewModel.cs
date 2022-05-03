namespace bill.ViewModels
{
    public class ReceiptViewModel
    {
        public int receipt_id { get; set; }
        public string receipt_code { get; set; } = null!;
        public DateOnly receipt_date { get; set; }
        public decimal receipt_product_price { get; set; }
        public decimal? receipt_product_discount { get; set; }
        public decimal receipt_discount { get; set; }
        public decimal receipt_total_price { get; set; }
    }
}
