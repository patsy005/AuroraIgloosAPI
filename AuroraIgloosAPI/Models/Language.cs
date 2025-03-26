using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

[Index("Name", Name = "UQ__Language__72E12F1BB898ABBB", IsUnique = true)]
public partial class Language
{

    public int Id { get; set; }


    public string Name { get; set; } = null!;

   
}
