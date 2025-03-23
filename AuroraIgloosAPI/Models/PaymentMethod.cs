using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class PaymentMethod
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("name")]
    //[StringLength(100)]
    public string? Name { get; set; }

    //[InverseProperty("PaymentMethod")]
    //public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();
}
