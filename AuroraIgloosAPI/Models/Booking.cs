using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Booking

{
    public int Id { get; set; }
    
    // igloo and trip
    public int IdCustomer { get; set; }
    public Customer Customer { get; set; }
    
    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    
    public decimal Amount { get; set; }
    
    public string? Notes { get; set; }
    
    public DateOnly BookingDate { get; set; }
    public DateOnly UpdateDate { get; set; }
    
    public int Guests { get; set; }
    
    // igloo booking only
    public int? IdIgloo { get; set; }
    public Igloo? Igloo { get; set; }
    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }
    
    public bool? EarlyCheckInRequest { get; set; }
    public bool? LateCheckOutRequest { get; set; }
    
    // trip only
    public int? TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public DateOnly? TripDate { get; set; }
    
    
}
