using AuroraIgloosAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs;

public class BookingFormDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "IdCustomer is required")]
    public int IdCustomer { get; set; }
    
    public int? IdIgloo { get; set; }

    public DateOnly? CheckIn { get; set; }

    public DateOnly? CheckOut { get; set; }
    
    public DateOnly? TripDate { get; set; }

    [Required(ErrorMessage = "PaymentMethodId is required")]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }


    public bool? EarlyCheckInRequest { get; set; }
    public bool? LateCheckOutRequest { get; set; }
    // public DateOnly? BookingDate { get; set; }
    public DateOnly LastModifiedAt { get; set; }

    public int? TripId { get; set; }
    
    public int Guests { get; set; }
}