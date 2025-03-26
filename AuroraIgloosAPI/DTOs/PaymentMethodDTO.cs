using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
    }
}
