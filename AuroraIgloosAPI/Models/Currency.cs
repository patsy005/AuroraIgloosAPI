using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Currency
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string? Currency1 { get; set; }

    [InverseProperty("Currency")]
    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();
}
