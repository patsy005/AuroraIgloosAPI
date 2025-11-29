namespace AuroraIgloosAPI.Models;

public class Trip
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public int Duration { get; set; } // in days
    
    public decimal PricePerPerson { get; set; }
    
    public required string ShortDescription { get; set; }
    
    public string? LongDescription { get; set; }
    
    public int LevelOfDifficultyId { get; set; }
    public TripLevelOfDifficulty? LevelOfDifficulty { get; set; }
    
    public int SeasonId { get; set; }
    public TripSeason? Season { get; set; }
    
    public int GuideId { get; set; }
    public Employee? Guide { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
}