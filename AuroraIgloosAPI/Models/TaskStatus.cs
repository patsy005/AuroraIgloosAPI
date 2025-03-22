using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class TaskStatus
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("statusName")]
    [StringLength(100)]
    public string? StatusName { get; set; }

    [InverseProperty("TaskStatus")]
    public virtual ICollection<Task> Task { get; set; } = new List<Task>();
}
