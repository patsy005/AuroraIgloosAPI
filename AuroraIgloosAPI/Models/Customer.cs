using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class Customer
{

    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    //[Column("nationality")]
    //[StringLength(100)]
    //public string? Nationality { get; set; }

    //[Column("idUser")]
    //public int? IdUser { get; set; }

    //[JsonIgnore]
    //[InverseProperty("Customer")]
    //public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    //[InverseProperty("IdCustomerNavigation")]
    //public virtual ICollection<CustomerNotification> CustomerNotification { get; set; } = new List<CustomerNotification>();

    //[ForeignKey("IdUser")]
    //[InverseProperty("Customer")]
    //public virtual User? User { get; set; }

    public int Id { get; set; }
    public int IdUser { get; set; }


    public required User User { get; set; }
}
