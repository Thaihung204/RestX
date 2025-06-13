using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class DishIngredient
{
    public int Id { get; set; }

    public int DishId { get; set; }

    public int IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
