using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class BookingChannel
{
    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("nazwa")]
    //[StringLength(10)]
    //public string? Nazwa { get; set; }

    //[InverseProperty("BookingChannel")]
    //public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    public int Id { get; set; }

    public string? Nazwa { get; set; }

}
