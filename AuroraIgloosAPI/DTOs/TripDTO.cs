using System;
using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class TripDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public int Duration { get; set; }
        public decimal PricePerPerson { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }

        public int LevelOfDifficultyId { get; set; }
        public string LevelOfDifficultyName { get; set; } = "";

        public int SeasonId { get; set; }
        public string SeasonName { get; set; } = "";

        public int GuideId { get; set; }
        
        public string GuideName { get; set; } = "";
        public Employee? Guide { get; set; }
        
        // public DateOnly CreatedAt { get; set; }
        // public DateOnly? UpdatedAt { get; set; }
        public DateOnly LastModifiedAt { get; set; }
        
        public string? PhotoUrl { get; set; }
    }

    public class TripRowDTO
    {
        public int TripId { get; set; }
        public string Name { get; set; } = "";

        public int Duration { get; set; }
        public decimal PricePerPerson { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }

        public string LevelOfDifficultyName { get; set; } = "";
        public string SeasonName { get; set; } = "";
    }
}