namespace AuroraIgloosAPI.DTOs;

public class EmployeeFormDTO
{
    
    public int Id { get; set; }
    
    // Employee / Role
    public int? RoleId { get; set; }

    // Person + Address
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

    // User
    public string? Login { get; set; }
    public string? Password { get; set; }
    public int? UserTypeId { get; set; }
    public int? UserRoleId { get; set; }

    // Zdjęcie – PRZYCHODZI z formularza
    public IFormFile? PhotoFile { get; set; }
}