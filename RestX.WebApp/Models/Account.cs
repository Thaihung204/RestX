using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Account
{
    public int Id { get; set; }

    public int? AdminId { get; set; }

    public int? OwnerId { get; set; }

    public int? StaffId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual Staff? Staff { get; set; }
}
