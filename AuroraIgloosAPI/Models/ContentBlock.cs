using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.Models;

public class ContentBlock
{
    public int Id { get; set; }
    
    [Required]
    public string Key { get; set; }
    
    [Required]
    public string Value { get; set; }
    
    public DateOnly LastModifiedAt { get; set; }
}