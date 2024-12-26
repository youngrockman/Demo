using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demofinish.Models;

public partial class DemodbContext : DbContext
{
    public DemodbContext()
    {
    }

    public DemodbContext(DbContextOptions<DemodbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=demodb;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderid");
            entity.Property(e => e.Orderdeliverydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus)
                .HasColumnType("character varying")
                .HasColumnName("orderstatus");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productarticlenumber }).HasName("OrderProduct_pkey");

            entity.ToTable("OrderProduct");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderProduct_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderProduct_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Pickuppointid).HasName("pickuppoint_pk");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Pickuppointid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("pickuppointid");
            entity.Property(e => e.Pickupname)
                .HasColumnType("character varying")
                .HasColumnName("pickupname");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.Productarticlenumber)
                .HasColumnType("character varying")
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Discountmax)
                .HasPrecision(19, 4)
                .HasColumnName("discountmax");
            entity.Property(e => e.Productcategory)
                .HasColumnType("character varying")
                .HasColumnName("productcategory");
            entity.Property(e => e.Productcost)
                .HasPrecision(19, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdescription)
                .HasColumnType("character varying")
                .HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturer)
                .HasColumnType("character varying")
                .HasColumnName("productmanufacturer");
            entity.Property(e => e.Productname)
                .HasColumnType("character varying")
                .HasColumnName("productname");
            entity.Property(e => e.Productphoto)
                .HasColumnType("character varying")
                .HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Productstatus)
                .HasColumnType("character varying")
                .HasColumnName("productstatus");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Userid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property(e => e.Userlogin)
                .HasColumnType("character varying")
                .HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword)
                .HasColumnType("character varying")
                .HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_userrole_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
