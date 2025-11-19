using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Person
{
    public int Id { get; set; }
    public int IdAddress { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber
    {
        get; set;
    }


    public required Address Address { get; set; }
}
