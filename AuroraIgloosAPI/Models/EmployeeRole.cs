using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class EmployeeRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("roleName")]
    [StringLength(100)]
    public string? RoleName { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
}
