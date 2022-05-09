namespace bill.ViewModels
{
    public class ItemViewModel
    {
        public int item_id { get; set; }
        public string? item_code { get; set; }
        public string item_name { get; set; }
        public decimal item_price { get; set; }
        public int item_unit_id { get; set; }

        public UnitViewModel? item_unit { get; set; }
    }
}
