using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? RoleId { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Role { get; set; }

        public string ? PhotoUrl { get; set; }
        //public User? User { get; set; }

        //public EmployeeRole? EmployeeRole { get; set; }
    }
}
