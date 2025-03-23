using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }

        //public User? User { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
