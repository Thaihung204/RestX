using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Staff
{
    public int Id { get; set; }

    public int? OwnerId { get; set; }

    public int FileId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual File File { get; set; } = null!;

    public virtual Owner? Owner { get; set; }
}
