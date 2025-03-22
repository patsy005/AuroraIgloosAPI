using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Discount
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idIgloo")]
    public int? IdIgloo { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("discount", TypeName = "decimal(5, 2)")]
    public decimal? Discount1 { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [ForeignKey("IdIgloo")]
    [InverseProperty("Discount")]
    public virtual Igloo? Igloo { get; set; }
}
