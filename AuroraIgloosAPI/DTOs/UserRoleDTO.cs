using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs;

public class UserRoleDTO
{
    
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Description { get; set; }
}