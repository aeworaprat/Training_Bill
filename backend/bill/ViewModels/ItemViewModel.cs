using System.ComponentModel.DataAnnotations;

namespace bill.ViewModels
{
    public class ItemViewModel
    {
        public int item_id { get; set; }
        public string? item_code { get; set; }
        [Required]
        [StringLength(42)]
        public string item_name { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "1000000000")]
        public decimal item_price { get; set; }
        [Required]
        public int item_unit_id { get; set; }

        public UnitViewModel? item_unit { get; set; }
    }
}
