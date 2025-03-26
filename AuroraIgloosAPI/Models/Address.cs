using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Address
{

    public int Id { get; set; }

    public string? Street { get; set; }

    public string? StreetNumber { get; set; }

    public string? HouseNumber { get; set; }

    public string? City { get; set; }


    public string? Country { get; set; }


    public string? PostalCode { get; set; }
}
