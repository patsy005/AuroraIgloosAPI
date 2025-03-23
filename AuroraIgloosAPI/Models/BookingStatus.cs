using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class BookingStatus
{
    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("statusName")]
    //[StringLength(100)]
    //public string? StatusName { get; set; }

    //[InverseProperty("Status")]
    //public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();
    public int Id { get; set; }
    public string? StatusName { get; set; }
}
