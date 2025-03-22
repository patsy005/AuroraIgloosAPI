using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idUser")]
    public int? IdUser { get; set; }

    [Column("roleId")]
    public int? RoleId { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    [InverseProperty("Employee")]
    public virtual ICollection<ForumComment> ForumComment { get; set; } = new List<ForumComment>();

    [InverseProperty("Employee")]
    public virtual ICollection<ForumPost> ForumPost { get; set; } = new List<ForumPost>();

    [ForeignKey("IdUser")]
    [InverseProperty("Employee")]
    public virtual User? User { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Employee")]
    public virtual EmployeeRole? Role { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Task> Task { get; set; } = new List<Task>();
}
