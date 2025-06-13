using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
