using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class DiscountDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Discount value is required")]
        public decimal Discount { get; set; }   // mapujemy na Discount1

        public string? Description { get; set; }
        
        public DateOnly? ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
        
    }
}
