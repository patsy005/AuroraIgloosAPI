using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class User
{


    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("name")]
    //[StringLength(100)]
    //public string? Name { get; set; }

    //[Column("surname")]
    //[StringLength(100)]
    //public string? Surname { get; set; }

    //[Column("email")]
    //[StringLength(100)]
    //public string? Email { get; set; }

    //[Column("phoneNumber")]
    //[StringLength(20)]
    //public string? PhoneNumber { get; set; }

    //[Column("idAddress")]
    //public int? IdAddress { get; set; }

    //[Column("genderId")]
    //public int? GenderId { get; set; }

    //[Column("securityQuestion")]
    //public string? SecurityQuestion { get; set; }

    //[Column("securityAnswer")]
    //public string? SecurityAnswer { get; set; }

    //[Column("createdAt", TypeName = "datetime")]
    //public DateTime? CreatedAt { get; set; }

    //[Column("languageId")]
    //public int? LanguageId { get; set; }

    //[Column("timezoneId")]
    //public int? TimezoneId { get; set; }

    //[InverseProperty("User")]
    //public virtual ICollection<Customer> Customer { get; set; } = new List<Customer>();

    //[InverseProperty("User")]
    //public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();

    //[ForeignKey("GenderId")]
    //[InverseProperty("User")]
    //public virtual Gender? Gender { get; set; }

    //[ForeignKey("IdAddress")]
    //[InverseProperty("User")]
    //public virtual Address? Address { get; set; }

    //[ForeignKey("LanguageId")]
    //[InverseProperty("User")]
    //public virtual Language? Language { get; set; }

    //[ForeignKey("TimezoneId")]
    //[InverseProperty("User")]
    //public virtual Timezone? Timezone { get; set; }

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
