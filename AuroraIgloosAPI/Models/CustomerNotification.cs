using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models;

public partial class CustomerNotification
{
    //[Key]
    //[Column("id")]
    public int Id { get; set; }

    //[Column("idCustomer")]
    public int? IdCustomer { get; set; }

    //[Column("message")]
    public string? Message { get; set; }

    //[Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    //[Column("notificationTypeId")]
    public int? NotificationTypeId { get; set; }

    //[Column("notificationPriorityId")]
    public int? NotificationPriorityId { get; set; }

    //[ForeignKey("IdCustomer")]
    //[InverseProperty("CustomerNotification")]
    //public virtual Customer? IdCustomerNavigation { get; set; }

    //[ForeignKey("NotificationPriorityId")]
    //[InverseProperty("CustomerNotification")]
    //public virtual NotificationPriority? NotificationPriority { get; set; }

    //[ForeignKey("NotificationTypeId")]
    //[InverseProperty("CustomerNotification")]

    //public virtual NotificationType? NotificationType { get; set; }

    public required Customer Customer { get; set; }
    public required NotificationPriority NotificationPriority { get; set; }
}
