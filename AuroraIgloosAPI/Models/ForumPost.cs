using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class ForumPost
{
    public int Id { get; set; }
    public required int IdEmployee { get; set; }
    public required string Title { get; set; }
    public required string PostContent { get; set; }
    public DateOnly? PostDate { get; set; }
    public DateOnly? UpdateDate { get; set; }
    public required int CategoryId { get; set; }
    public string? Tags { get; set; }

    public required Employee Employee { get; set; }
    public required ForumCategory Category { get; set; }

    public virtual ICollection<ForumComment> ForumComment { get; set; } = new List<ForumComment>();
}
