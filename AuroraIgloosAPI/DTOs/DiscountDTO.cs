using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class DiscountDTO
    {
        // public int Id { get; set; }
        //
        // public int? IdIgloo { get; set; }
        //
        // [Required(ErrorMessage = "Name is required")]
        // public required string Name { get; set; }
        //
        // [Required(ErrorMessage = "Discount is required")]
        // public decimal Discount { get; set; }
        //
        //
        // [Required(ErrorMessage = "Description is required")]
        // public required string Description { get; set; }
        //
        // public required string IglooName { get; set; }
        
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
