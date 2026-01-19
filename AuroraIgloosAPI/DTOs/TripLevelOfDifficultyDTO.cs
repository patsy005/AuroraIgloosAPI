namespace AuroraIgloosAPI.DTOs;

public class TripLevelOfDifficultyDTO
{
    public int Id { get; set; }
    
    public int Level { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    // public DateOnly CreatedAt { get; set; }
    // public DateOnly UpdatedAt { get; set; }
    public DateOnly LastModifiedAt { get; set; }
}