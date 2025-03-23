using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

[Index("Name", Name = "UQ__Timezone__72E12F1B291DD80D", IsUnique = true)]
public partial class Timezone
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("name")]
    //[StringLength(50)]
    public string Name { get; set; } = null!;

    //[Column("offset")]
    //[StringLength(10)]
    public string Offset { get; set; } = null!;

    //[InverseProperty("Timezone")]
    //public virtual ICollection<User> User { get; set; } = new List<User>();
}
