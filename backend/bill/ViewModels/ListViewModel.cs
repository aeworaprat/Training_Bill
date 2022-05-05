namespace bill.ViewModels
{
    public class ListViewModel
    {
        public int list_id { get; set; }
        public decimal list_quantity { get; set; }
        public decimal list_price { get; set; }
        public decimal list_discount { get; set; }
        public decimal list_discount_bath { get; set; }
        public decimal list_total_price { get; set; }
        public int list_item_id { get; set; }
        public int list_bill_id { get; set; }

        public virtual ReceiptViewModel list_receipt { get; set; } = null!;
        public virtual ItemViewModel list_item { get; set; } = null!;

    }
}
