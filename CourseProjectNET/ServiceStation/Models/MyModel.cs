namespace ServiceStation.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyModel : DbContext
    {
        public MyModel() : base("name=MyModel") { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<RepairPart> RepairParts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<OrderStatus>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.OrderStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Place>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.Place)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RepairPart>()
                .Property(e => e.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Schedule>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Schedule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Schedule>()
                .HasMany(e => e.RepairParts)
                .WithRequired(e => e.Schedule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Schedule>()
                .HasMany(e => e.Workers)
                .WithMany(e => e.Schedules)
                .Map(m => m.ToTable("ScheduleToWorkers")
                .MapLeftKey("ScheduleId")
                .MapRightKey("WorkerId"));

            modelBuilder.Entity<Service>()
                .Property(e => e.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }

        public DbSet<RegisterModel> RegisterModels { get; set; }

        public DbSet<LoginModel> LoginModels { get; set; }
    }
}
