using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Customer
{
    public int Id { get; set; }
    public int IdUser { get; set; }


    public required User User { get; set; }
}
