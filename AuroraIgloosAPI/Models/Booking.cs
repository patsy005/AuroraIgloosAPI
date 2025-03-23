using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Booking

{
    //[Key]
    public int Id { get; set; }
    public int? IdCustomer { get; set; }
    public int? IdIgloo { get; set; }
    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }
    public int? PaymentMethodId { get; set; }
    public decimal? Amount { get; set; }
    public int? IdStatus { get; set; }
    public DateOnly? BookingDate { get; set; }
    public DateOnly? LastModifiedDate { get; set; }
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public int? CreatedById { get; set; }
    public bool? EarlyCheckInRequest { get; set; }
    public bool? LateCheckOutRequest { get; set; }
    public int? CurrencyId { get; set; }
    public int? BookingChannelId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public required Customer Customer { get; set; }
    public required Igloo Igloo { get; set; }
    public required BookingStatus Status { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required Currency Currency { get; set; }
    public required BookingChannel BookingChannel { get; set; }
    public required Employee Employee { get; set; }



    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("idCustomer")]
    //public int? IdCustomer { get; set; }

    //[Column("idIgloo")]
    //public int? IdIgloo { get; set; }

    //[Column("checkIn")]
    //public DateOnly? CheckIn { get; set; }

    //[Column("checkOut")]
    //public DateOnly? CheckOut { get; set; }

    //[Column("paymentMethodId")]
    //public int? PaymentMethodId { get; set; }

    //[Column("amount", TypeName = "decimal(10, 2)")]
    //public decimal? Amount { get; set; }

    //[Column("idStatus")]
    //public int? IdStatus { get; set; }

    //[Column("bookingDate")]
    //public DateOnly? BookingDate { get; set; }

    //[Column("lastModifiedDate")]
    //public DateOnly? LastModifiedDate { get; set; }

    //[Column("notes")]
    //public string? Notes { get; set; }

    //[Column("cancellationReason")]
    //[StringLength(50)]
    //public string? CancellationReason { get; set; }

    //[Column("createdById")]
    //public int? CreatedById { get; set; }

    //[Column("earlyCheckInRequest")]
    //public bool? EarlyCheckInRequest { get; set; }

    //[Column("lateCheckOutRequest")]
    //public bool? LateCheckOutRequest { get; set; }

    //[Column("currencyId")]
    //public int? CurrencyId { get; set; }

    //[Column("bookingChannelId")]
    //public int? BookingChannelId { get; set; }

    //[ForeignKey("BookingChannelId")]
    //[InverseProperty("Booking")]
    //public virtual BookingChannel? BookingChannel { get; set; }

    //[ForeignKey("CreatedById")]
    //[InverseProperty("Booking")]
    //public virtual Employee? Employee { get; set; }

    //[ForeignKey("CurrencyId")]
    //[InverseProperty("Booking")]
    //public virtual Currency? Currency { get; set; }

    //[ForeignKey("IdCustomer")]
    //[InverseProperty("Booking")]
    //public virtual Customer? Customer { get; set; }

    //[ForeignKey("IdIgloo")]
    //[InverseProperty("Booking")]
    //public virtual Igloo? Igloo { get; set; }

    //[ForeignKey("IdStatus")]
    //[InverseProperty("Booking")]
    //public virtual BookingStatus? Status { get; set; }

    //[ForeignKey("PaymentMethodId")]
    //[InverseProperty("Booking")]
    //public virtual PaymentMethod? PaymentMethod { get; set; }
}
