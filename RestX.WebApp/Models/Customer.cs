using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Customer
{
    public int Id { get; set; }

    public int? OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? Point { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Owner? Owner { get; set; }
}
