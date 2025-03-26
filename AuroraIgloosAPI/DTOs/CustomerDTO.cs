using AuroraIgloosAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }

        //public User? User { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "Surname is required")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public required string Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        public required string City { get; set; }
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public required string Country { get; set; }
    }
}
