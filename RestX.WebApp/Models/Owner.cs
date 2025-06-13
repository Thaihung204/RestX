using System;
using System.Collections.Generic;

namespace RestX.WebApp.Models;

public partial class Owner
{
    public int Id { get; set; }

    public int FileId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Information { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

    public virtual File File { get; set; } = null!;

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
