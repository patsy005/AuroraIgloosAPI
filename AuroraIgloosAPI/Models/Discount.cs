using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Discount
{
    public int Id { get; set; }

    public int? IdIgloo { get; set; }

    public string? Name { get; set; }

    [Column("discount")]
    public required decimal Discount1 { get; set; }

    public string? Description { get; set; }


    public required Igloo Igloo { get; set; }
}
