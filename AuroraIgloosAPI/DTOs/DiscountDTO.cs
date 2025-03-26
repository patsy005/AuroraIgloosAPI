using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class DiscountDTO
    {
        public int Id { get; set; }

        public int? IdIgloo { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public decimal Discount { get; set; }


        [Required(ErrorMessage = "Description is required")]
        public required string Description { get; set; }

        public required string IglooName { get; set; }
    }
}
