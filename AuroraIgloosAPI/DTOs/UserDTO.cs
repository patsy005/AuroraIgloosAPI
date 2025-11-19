using AuroraIgloosAPI.Models;
using System.ComponentModel.DataAnnotations;


namespace AuroraIgloosAPI.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Login is required")]
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    
    public int UserRoleId { get; set; }
    public UserRole? Role { get; set; }
    
    public int UserTypeId { get; set; }
    public UserType? UserType { get; set; }
    
    public Employee? Employee { get; set; }
    public Customer? Customer { get; set; }
}

public class UserCreateDTO
{
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public int UserRoleId { get; set; }
    public int UserTypeId { get; set; }
}

public class UserUpdateDTO
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public int UserRoleId { get; set; }
    public int UserTypeId { get; set; }
}