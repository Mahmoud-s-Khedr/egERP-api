using System;
using EG_ERP.Models;
using EG_ERP.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EG_ERP.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    private readonly IConfiguration config;

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<BankAccount> BankAccounts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<OrderPayment> OrderPayments { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Payroll> Payrolls { get; set; }
    public virtual DbSet<PayrollPayment> PayrollPayments { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<WareHouse> WareHouses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _config) : base(options)
    {
        config = _config;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrderDetail>()
            .HasKey(o => new { o.OrderId, o.ProductId });
        
        builder.Entity<OrderPayment>()
            .HasKey(o => new { o.OrderId, o.PaymentId });
        
        builder.Entity<PayrollPayment>()
            .HasKey(p => new { p.PayrollId, p.PaymentId });
        
        builder.Entity<AppUser>()
            .ToTable("Users");
        builder.Entity<AppRole>()
            .ToTable("Roles");


        builder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Employee>()
            .HasOne(e => e.ManagerOf)
            .WithOne(m => m.Manager)
            .HasForeignKey<Department>(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.SeedRoles();
        builder.SeedAdmin(config);
    }
}
