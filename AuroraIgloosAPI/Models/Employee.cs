using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Employee
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int RoleId { get; set; }

    public string? PhotoUrl { get; set; }

    public required User User { get; set; }
    public required EmployeeRole EmployeeRole { get; set; }
}
