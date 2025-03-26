using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Invoice
{
 
    public int Id { get; set; }

    public int? IdBooking { get; set; }

 
    public string? Number { get; set; }

    public DateOnly? IssuedDate { get; set; }

}
