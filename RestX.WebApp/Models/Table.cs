using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Table
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int TableStatusId { get; set; }

    public int TableNumber { get; set; }

    public string Qrcode { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual TableStatus TableStatus { get; set; } = null!;
}
