using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestX.WebApp.Models
{
    public class RestXDbContext : DbContext
    {
        public RestXDbContext(DbContextOptions<RestXDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientExport> IngredientExports { get; set; }
        public DbSet<IngredientImport> IngredientImports { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableStatus> TableStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure to ignore audit properties from Entity<T> base class
            // since they don't exist in the existing database
            
            // Ignore audit properties for all entities
            //modelBuilder.Entity<File>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Admin>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Owner>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Staff>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Account>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Customer>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<TableStatus>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Table>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Category>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Dish>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //// OrderStatus doesn't inherit from Entity<T>, so no need to ignore audit properties
            //modelBuilder.Entity<Order>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<OrderDetail>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //// PaymentMethod doesn't inherit from Entity<T>, so no need to ignore audit properties
            //modelBuilder.Entity<Payment>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Ingredient>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<Supplier>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<DishIngredient>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<IngredientExport>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);
            //modelBuilder.Entity<IngredientImport>().Ignore(e => e.CreatedDate).Ignore(e => e.ModifiedDate)
            //    .Ignore(e => e.CreatedBy).Ignore(e => e.ModifiedBy).Ignore(e => e.Version).Ignore(e => e.PropertiesJson);

            // Configure File entity
            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            });

            // Configure Admin entity
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Owner entity
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Information).HasMaxLength(500);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign key to File
                entity.HasOne(e => e.File)
                      .WithMany()
                      .HasForeignKey(e => e.FileId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Staff entity
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign key to Owner
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Staff)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Foreign key to File
                entity.HasOne(e => e.File)
                      .WithMany()
                      .HasForeignKey(e => e.FileId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Account entity
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Username).IsUnique();

                // Configure table with check constraint for Role
                entity.ToTable(t => t.HasCheckConstraint("CK_Account_Role", "[Role] IN ('Admin', 'Owner', 'Staff')"));

                // Foreign keys
                entity.HasOne(e => e.Admin)
                      .WithMany(a => a.Accounts)
                      .HasForeignKey(e => e.AdminId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Accounts)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Staff)
                      .WithMany(s => s.Accounts)
                      .HasForeignKey(e => e.StaffId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Point).HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign key to Owner
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Customers)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure TableStatus entity
            modelBuilder.Entity<TableStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Configure Table entity
            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.TableNumber).IsRequired();
                entity.Property(e => e.Qrcode).IsRequired().HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasIndex(e => e.Qrcode).IsUnique();

                // Foreign keys
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Tables)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TableStatus)
                      .WithMany(ts => ts.Tables)
                      .HasForeignKey(e => e.TableStatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Configure Dish entity
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Dishes)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.File)
                      .WithMany()
                      .HasForeignKey(e => e.FileId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Dishes)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderStatus entity
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Time).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(e => e.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Table)
                      .WithMany(t => t.Orders)
                      .HasForeignKey(e => e.TableId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Orders)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.OrderStatus)
                      .WithMany(os => os.Orders)
                      .HasForeignKey(e => e.OrderStatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderDetail entity
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Configure table with check constraint for Quantity
                entity.ToTable("OrderDetail", t => t.HasCheckConstraint("CK_OrderDetail_Quantity", "[Quantity] > 0"));

                // Foreign keys
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Dish)
                      .WithMany(d => d.OrderDetails)
                      .HasForeignKey(e => e.DishId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure PaymentMethod entity
            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Configure Payment entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Time).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Cost).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PaymentMethod)
                      .WithMany(pm => pm.Payments)
                      .HasForeignKey(e => e.PaymentMethodId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Ingredient entity
            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CurrentQuantity).HasColumnType("decimal(10,2)").HasDefaultValue(0m);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);

                // Foreign key to Owner
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Ingredients)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Foreign key to Owner
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Suppliers)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure DishIngredient entity
            modelBuilder.Entity<DishIngredient>(entity =>
            {
                entity.ToTable("DishIngredient");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Quantity).HasColumnType("decimal(10,2)").IsRequired();

                // Foreign keys
                entity.HasOne(e => e.Dish)
                      .WithMany(d => d.DishIngredients)
                      .HasForeignKey(e => e.DishId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Ingredient)
                      .WithMany(i => i.DishIngredients)
                      .HasForeignKey(e => e.IngredientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure IngredientExport entity
            modelBuilder.Entity<IngredientExport>(entity =>
            {
                entity.ToTable("IngredientExport");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Quantity).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Time).HasDefaultValueSql("GETDATE()");

                // Foreign keys
                entity.HasOne(e => e.Ingredient)
                      .WithMany(i => i.IngredientExports)
                      .HasForeignKey(e => e.IngredientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.OrderDetail)
                      .WithMany(od => od.IngredientExports)
                      .HasForeignKey(e => e.OrderDetailId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure IngredientImport entity
            modelBuilder.Entity<IngredientImport>(entity =>
            {
                entity.ToTable("IngredientImport");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Quantity).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Time).HasDefaultValueSql("GETDATE()");

                // Foreign keys
                entity.HasOne(e => e.Ingredient)
                      .WithMany(i => i.IngredientImports)
                      .HasForeignKey(e => e.IngredientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                      .WithMany(s => s.IngredientImports)
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Create indexes for better performance
            modelBuilder.Entity<Order>()
                .HasIndex(e => e.Time)
                .HasDatabaseName("IX_Order_Time");

            modelBuilder.Entity<Payment>()
                .HasIndex(e => e.Time)
                .HasDatabaseName("IX_Payment_Time");

            modelBuilder.Entity<IngredientImport>()
                .HasIndex(e => e.Time)
                .HasDatabaseName("IX_IngredientImport_Time");

            modelBuilder.Entity<IngredientExport>()
                .HasIndex(e => e.Time)
                .HasDatabaseName("IX_IngredientExport_Time");
        }
    }
} 