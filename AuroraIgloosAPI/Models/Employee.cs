using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Employee
{
    public int Id { get; set; }
    public int IdPerson { get; set; }
    public int RoleId { get; set; }
    
    public int IdUser { get; set; }

    public string? PhotoUrl { get; set; }

    public required Person Person { get; set; }
    public required EmployeeRole EmployeeRole { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
    
    public ICollection<Trip> GuidedTrips { get; set; } = new List<Trip>();
}
