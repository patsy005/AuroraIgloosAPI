using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class TripFormDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal PricePerPerson { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }


        [Required(ErrorMessage = "LevelOfDifficultyId is required")]
        public int LevelOfDifficultyId { get; set; }

        [Required(ErrorMessage = "SeasonId is required")]
        public int SeasonId { get; set; }
        
        [Required(ErrorMessage = "GuideId is required")]
        public int GuideId { get; set; }
    }
}