using System.ComponentModel.DataAnnotations;

namespace bill.ViewModels
{
    public class UnitViewModel
    {
        public int unit_id { get; set; }

        [Required]
        [StringLength(45)]
        public string unit_name { get; set; } = null!;
    }
}
