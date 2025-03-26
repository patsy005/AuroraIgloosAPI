using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class IglooDTO
    {
        public int Id { get; set; }

        public int IdDiscount { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "PricePerNight is required")]
        public decimal PricePerNight { get; set; }

        public decimal? Discount { get; set; }
        public required string DiscountName { get; set; }

    }
}
