using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestX.WebApp.Models;

public partial class RestXRestaurantManagementContext : DbContext
{
    public RestXRestaurantManagementContext()
    {
    }

    public RestXRestaurantManagementContext(DbContextOptions<RestXRestaurantManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishIngredient> DishIngredients { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientExport> IngredientExports { get; set; }

    public virtual DbSet<IngredientImport> IngredientImports { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<TableStatus> TableStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-PG;Initial Catalog=RestX_RestaurantManagement;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07F1A2F4BA");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "IX_Account_Username");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E4091103CE").IsUnique();

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Admin).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Account__AdminId__5070F446");

            entity.HasOne(d => d.Owner).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Account__OwnerId__5165187F");

            entity.HasOne(d => d.Staff).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Account__StaffId__52593CB8");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC07EF269E8A");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D1053461E0407A").IsUnique();

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07246FAA7F");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC070FFE5D62");

            entity.ToTable("Customer");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Point).HasDefaultValue(0);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Owner).WithMany(p => p.Customers)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Customer__OwnerI__59063A47");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC078710F14A");

            entity.ToTable("Dish");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__CategoryId__6C190EBB");

            entity.HasOne(d => d.File).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__FileId__6B24EA82");

            entity.HasOne(d => d.Owner).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__OwnerId__6A30C649");
        });

        modelBuilder.Entity<DishIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishIngr__3214EC07DA521C07");

            entity.ToTable("DishIngredient");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Dish).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__DishI__18EBB532");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__Ingre__19DFD96B");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__File__3214EC07B3B51A30");

            entity.ToTable("File");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Url).HasMaxLength(500);
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07EDD26217");

            entity.ToTable("Ingredient");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrentQuantity)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Owner).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Owner__0E6E26BF");
        });

        modelBuilder.Entity<IngredientExport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07B73E63AE");

            entity.ToTable("IngredientExport");

            entity.HasIndex(e => e.Time, "IX_IngredientExport_Time");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Time).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientExports)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__1F98B2C1");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.IngredientExports)
                .HasForeignKey(d => d.OrderDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Order__208CD6FA");
        });

        modelBuilder.Entity<IngredientImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07DAF4CE0A");

            entity.ToTable("IngredientImport");

            entity.HasIndex(e => e.Time, "IX_IngredientImport_Time");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Time).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientImports)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Ingre__2645B050");

            entity.HasOne(d => d.Supplier).WithMany(p => p.IngredientImports)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__Suppl__2739D489");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC071746C568");

            entity.ToTable("Order");

            entity.HasIndex(e => e.Time, "IX_Order_Time");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Time).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__74AE54BC");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__OrderStat__778AC167");

            entity.HasOne(d => d.Owner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__OwnerId__76969D2E");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__TableId__75A278F5");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC075D22CF97");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Dish).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__DishI__7F2BE32F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__7E37BEF6");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC074A17875B");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Owner__3214EC075104BA13");

            entity.ToTable("Owner");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Information).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.File).WithMany(p => p.Owners)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Owner__FileId__4316F928");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07C1A79D7F");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.Time, "IX_Payment_Time");

            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Time).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__OrderId__07C12930");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Payment__08B54D69");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC077D0DFA23");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC07FE654C3F");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.File).WithMany(p => p.Staff)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__FileId__49C3F6B7");

            entity.HasOne(d => d.Owner).WithMany(p => p.Staff)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Staff__OwnerId__48CFD27E");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC073B99F139");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Owner).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Supplier__OwnerI__14270015");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Table__3214EC074DA84762");

            entity.ToTable("Table");

            entity.HasIndex(e => e.Qrcode, "IX_Table_QRCode");

            entity.HasIndex(e => e.Qrcode, "UQ__Table__5B869AD94ADFB29D").IsUnique();

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Owner).WithMany(p => p.Tables)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Table__OwnerId__619B8048");

            entity.HasOne(d => d.TableStatus).WithMany(p => p.Tables)
                .HasForeignKey(d => d.TableStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Table__TableStat__628FA481");
        });

        modelBuilder.Entity<TableStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TableSta__3214EC07EFEE3B5E");

            entity.ToTable("TableStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
