using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Igloo
{

    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Capacity { get; set; }

    public decimal? PricePerNight { get; set; }

    [ForeignKey("IdIgloo")]
    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();
}
