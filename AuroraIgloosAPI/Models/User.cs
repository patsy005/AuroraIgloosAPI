namespace AuroraIgloosAPI.Models;

public class User
{
    public int Id { get; set; }
    
    
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    
    public int UserRoleId { get; set; }
    public UserRole Role { get; set; }
    
    public int UserTypeId { get; set; }
    public UserType UserType { get; set; }
    
    public Employee? Employee { get; set; }
    public Customer? Customer { get; set; }
    
    public DateOnly LastModifiedAt { get; set; }

    
    
}