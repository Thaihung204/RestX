using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class IngredientExport
{
    public int Id { get; set; }

    public int IngredientId { get; set; }

    public int OrderDetailId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime? Time { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
