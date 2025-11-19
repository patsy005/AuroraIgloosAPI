using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs;

public class UserTypeDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Type { get; set; }
}