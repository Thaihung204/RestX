using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Dish
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int FileId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();

    public virtual File File { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Owner Owner { get; set; } = null!;
}
