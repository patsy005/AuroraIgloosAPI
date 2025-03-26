using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Booking

{
    public int Id { get; set; }
    public int IdCustomer { get; set; }
    public int IdIgloo { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
    //public int? IdStatus { get; set; }
    public DateOnly BookingDate { get; set; }
    public DateOnly? LastModifiedDate { get; set; }
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public int CreatedById { get; set; }
    public bool? EarlyCheckInRequest { get; set; }
    public bool? LateCheckOutRequest { get; set; }

    public required Customer Customer { get; set; }
    public required Igloo Igloo { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }

    public required Employee Employee { get; set; }

}
