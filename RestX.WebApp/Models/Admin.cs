using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
