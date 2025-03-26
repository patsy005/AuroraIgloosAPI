using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class CustomerNotification
{
    public int Id { get; set; }

    public int? IdCustomer { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? NotificationTypeId { get; set; }

    public int? NotificationPriorityId { get; set; }


    public required Customer Customer { get; set; }
    public required NotificationPriority NotificationPriority { get; set; }
}
