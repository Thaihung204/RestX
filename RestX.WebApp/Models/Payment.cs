using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime? Time { get; set; }

    public decimal Cost { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
