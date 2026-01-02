namespace AuroraIgloosAPI.DTOs;

public class MeResponseDTO
{
    public int Id { get; set; }
    public string Login { get; set; } = "";
    public string Role { get; set; } = "";
    public string UserType { get; set; } = "";
    
    public string Name { get; set; } = "";
    
    public string Surname { get; set; } = "";
    
    public string Email { get; set; } = "";

    public string? PhotoUrl { get; set; } = "";
    
    public int EmployeeId { get; set; }
}