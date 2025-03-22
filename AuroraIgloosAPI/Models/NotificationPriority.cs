using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class NotificationPriority
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(10)]
    public string? Name { get; set; }

    [InverseProperty("NotificationPriority")]
    public virtual ICollection<CustomerNotification> CustomerNotification { get; set; } = new List<CustomerNotification>();
}
