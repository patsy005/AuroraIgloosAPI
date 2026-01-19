using System.ComponentModel.DataAnnotations;
using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class IglooDTO
    {
        public int Id { get; set; }

        public int? IdDiscount { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "PricePerNight is required")]
        public decimal PricePerNight { get; set; }

        // public decimal? Discount { get; set; }
        // public required string DiscountName { get; set; }
        
        public Discount? Discount {get; set;}
        
        public string? Description { get; set; }
        
        public string? PhotoUrl { get; set; }
        
        // public DateOnly CreatedAt { get; set; }
        public DateOnly LastModifiedAt { get; set; }
    }

    public class IglooRowDTO
    {
        public int IglooId { get; set; }

        public int Capacity { get; set; }
        public int PricePerNight { get; set; }

        public Discount? Discount { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; } = "";

        // wyliczenia:
        public int BookingsCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public double OccupancyPercent { get; set; }
    }
}
