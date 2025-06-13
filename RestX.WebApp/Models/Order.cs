using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int TableId { get; set; }

    public int OwnerId { get; set; }

    public int OrderStatusId { get; set; }

    public DateTime? Time { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Table Table { get; set; } = null!;
}
