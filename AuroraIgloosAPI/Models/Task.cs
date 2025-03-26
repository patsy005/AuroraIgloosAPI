using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Task
{
 
    public int Id { get; set; }

    public int? IdEmployee { get; set; }

 
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? Date { get; set; }

    public int? IdStatus { get; set; }


    public required Employee Employee { get; set; }
    public required TaskStatus TaskStatus { get; set; }


}
