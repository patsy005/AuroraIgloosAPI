namespace AuroraIgloosAPI.Models;

public class TripLevelOfDifficulty
{
    public int Id { get; set; }
    
    public int Level { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<Trip>? Trips { get; set; } =  new List<Trip>();
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
}