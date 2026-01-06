namespace AuroraIgloosAPI.DTOs;

public class TripSeasonDTO
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
}