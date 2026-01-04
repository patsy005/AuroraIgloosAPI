using AuroraIgloosAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "IdCustomer is required")]
        public int IdCustomer { get; set; }
        
        public int? IdIgloo { get; set; }

        public DateOnly? CheckIn { get; set; }

        public DateOnly? CheckOut{ get; set; }
        
        public DateOnly? TripDate { get; set; }

        [Required(ErrorMessage = "PaymentMethodId is required")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        

        public bool? EarlyCheckInRequest { get; set; }
        public bool? LateCheckOutRequest { get; set; }
        public DateOnly? BookingDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerSurname { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }

        public string? IglooName { get; set; }
        // public Igloo? Igloo { get; set; }
        

        public string? PaymentMethodName { get; set; }
        
        public int? TripId { get; set; }
        // public Trip? Trip { get; set; }

        public string? TripName { get; set; }
        
        public int Guests { get; set; }
    }

    public class BookingRowDTO
    {
        public int BookingId { get; set; }

        public string CustomerName { get; set; } = "";
        public string CustomerSurname { get; set; } = "";
        public string CustomerEmail { get; set; } = "";

        public string IglooName { get; set; } = "";

        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }

        public DateOnly BookingDate { get; set; }

        public DateOnly? TripDate { get; set; }
        public string TripName { get; set; } = "";

        public decimal Amount { get; set; }
        public int Guests { get; set; }

        public string PaymentMethodName { get; set; } = "";
        public string EarlyCheckInRequest { get; set; } = "";
        public string LateCheckOutRequest { get; set; } = "";
    }
}
