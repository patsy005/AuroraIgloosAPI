using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Address
{
    //[Key]
    //[Column("id")]

    //public int Id { get; set; }

    //public string? Street { get; set; }
    //public string? StreetNumber { get; set; }
    //public string? HouseNumber { get; set; }
    //public string? City { get; set; }
    //public string? Country { get; set; }
    //public string? PostalCode { get; set; }

    //public virtual ICollection<User> User { get; set; } = new List<User>();


    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("street")]
    //[StringLength(255)]
    //public string? Street { get; set; }

    //[Column("streetNumber")]
    //[StringLength(10)]
    //public string? StreetNumber { get; set; }

    //[Column("houseNumber")]
    //[StringLength(10)]
    //public string? HouseNumber { get; set; }

    //[Column("city")]
    //[StringLength(100)]
    //public string? City { get; set; }

    //[Column("country")]
    //[StringLength(100)]
    //public string? Country { get; set; }

    //[Column("postalCode")]
    //[StringLength(20)]
    //public string? PostalCode { get; set; }

    //[InverseProperty("Address")]
    //public virtual ICollection<User> User { get; set; } = new List<User>();

    public int Id { get; set; }

    public string? Street { get; set; }

    public string? StreetNumber { get; set; }

    public string? HouseNumber { get; set; }

    public string? City { get; set; }


    public string? Country { get; set; }


    public string? PostalCode { get; set; }
}
