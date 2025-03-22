using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Invoice
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idBooking")]
    public int? IdBooking { get; set; }

    [Column("number")]
    [StringLength(100)]
    public string? Number { get; set; }

    [Column("issuedDate")]
    public DateOnly? IssuedDate { get; set; }

}
