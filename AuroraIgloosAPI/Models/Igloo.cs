using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Igloo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("capacity")]
    public int? Capacity { get; set; }

    [Column("pricePerNight", TypeName = "decimal(10, 2)")]
    public decimal? PricePerNight { get; set; }

    [JsonIgnore]
    [InverseProperty("Igloo")]
    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    [InverseProperty("Igloo")]
    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();
}
