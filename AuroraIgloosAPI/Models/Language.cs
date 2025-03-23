using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

[Index("Name", Name = "UQ__Language__72E12F1BB898ABBB", IsUnique = true)]
public partial class Language
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("name")]
    //[StringLength(50)]
    public string Name { get; set; } = null!;

    //[InverseProperty("Language")]
    //public virtual ICollection<User> User { get; set; } = new List<User>();
}
