using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class ForumCategory
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("name")]
    //[StringLength(100)]
    public string? Name { get; set; }

    //[InverseProperty("Category")]
    //public virtual ICollection<ForumPost> ForumPost { get; set; } = new List<ForumPost>();
}
