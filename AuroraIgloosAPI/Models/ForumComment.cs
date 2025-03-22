using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class ForumComment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idPost")]
    public int? IdPost { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("commentDate")]
    public DateOnly? CommentDate { get; set; }

    [ForeignKey("idEmployee")]
    [InverseProperty("ForumComment")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("IdPost")]
    [InverseProperty("ForumComment")]
    public virtual ForumPost? ForumPost { get; set; }
}
