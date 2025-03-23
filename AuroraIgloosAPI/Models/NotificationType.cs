using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class NotificationType
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("type")]
    //[StringLength(50)]
    public string? Type { get; set; }

    //[InverseProperty("NotificationType")]
    //public virtual ICollection<CustomerNotification> CustomerNotification { get; set; } = new List<CustomerNotification>();
}
