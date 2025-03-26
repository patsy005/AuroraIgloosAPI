using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class ForumComment
{

    public int Id { get; set; }
    //[ForeignKey("IdPost")]
    //[Column("idPost")]
    public int? IdPost { get; set; }
    public int? IdEmployee { get; set; }
    public string? Comment { get; set; }
    public DateOnly? CommentDate { get; set; }

    public Employee? Employee { get; set; }

    //[ForeignKey("IdPost")]
    //[Column("idPost")]
    public ForumPost? ForumPost { get; set; }
}
