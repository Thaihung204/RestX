using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class TableStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
