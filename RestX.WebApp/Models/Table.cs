using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestX.WebApp.Models;

[Table("Table")]
public partial class Table : Entity<int>
{
    [Required]
    [ForeignKey("Owner")]
    public Guid OwnerId { get; set; }

    [Required]
    [ForeignKey("TableStatus")]
    public int TableStatusId { get; set; }

    [Required]
    public int TableNumber { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("QRCode")]
    public string Qrcode { get; set; } = null!;

    [Column(TypeName = "bit")]
    public bool? IsActive { get; set; } = true;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual TableStatus TableStatus { get; set; } = null!;
}
