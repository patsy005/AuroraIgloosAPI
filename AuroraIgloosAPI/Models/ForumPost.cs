using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class ForumPost
{
    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("idEmployee")]
    //public int? IdEmployee { get; set; }

    //[Column("title")]
    //[StringLength(255)]
    //public string? Title { get; set; }

    //[Column("postContent")]
    //public string? PostContent { get; set; }

    //[Column("postDate")]
    //public DateOnly? PostDate { get; set; }

    //[Column("categoryId")]
    //public int? CategoryId { get; set; }

    //[Column("statusId")]
    //public int? StatusId { get; set; }

    //[Column("tags")]
    //public string? Tags { get; set; }

    //[ForeignKey("CategoryId")]
    //[InverseProperty("ForumPost")]
    //public virtual ForumCategory? Category { get; set; }

    //[InverseProperty("ForumPost")]
    //public virtual ICollection<ForumComment> ForumComment { get; set; } = new List<ForumComment>();

    //[ForeignKey("IdEmployee")]
    //[InverseProperty("ForumPost")]
    //public virtual Employee? Employee { get; set; }

    //[ForeignKey("StatusId")]
    //[InverseProperty("ForumPost")]
    //public virtual ForumStatus? Status { get; set; }

    public int Id { get; set; }
    public int? IdEmployee { get; set; }
    public string? Title { get; set; }
    public string? PostContent { get; set; }
    public DateOnly? PostDate { get; set; }
    public int? CategoryId { get; set; }
    public int? StatusId { get; set; }
    public string? Tags { get; set; }

    public required Employee Employee { get; set; }
    public required ForumCategory Category { get; set; }

    public virtual ICollection<ForumComment> ForumComment { get; set; } = new List<ForumComment>();
}
