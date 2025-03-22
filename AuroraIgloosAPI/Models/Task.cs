using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Task
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [Column("idStatus")]
    public int? IdStatus { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("Task")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("IdStatus")]
    [InverseProperty("Task")]
    public virtual TaskStatus? TaskStatus { get; set; }
}
