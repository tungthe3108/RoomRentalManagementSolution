using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoomRentalManagementSolution.Models;

public partial class RoomRetalManagementContext : DbContext
{
    public RoomRetalManagementContext()
    {
    }

    public RoomRetalManagementContext(DbContextOptions<RoomRetalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<NumOfPerson> NumOfPeople { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomPrice> RoomPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Area");

            entity.Property(e => e.Area1)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Area");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => new { e.ContractId, e.RoomId, e.CustomerId });

            entity.ToTable("Contract");

            entity.Property(e => e.ContractId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ContractID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateOfExpiration).HasColumnType("date");
            entity.Property(e => e.DateOfHire).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Customer");

            entity.HasOne(d => d.Room).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Room");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Room).WithMany(p => p.Customers)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Customer_Room");

            entity.HasMany(d => d.Rooms).WithMany(p => p.CustomersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerRoom",
                    r => r.HasOne<Room>().WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Customer_Room_Room"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Customer_Room_Customer"),
                    j =>
                    {
                        j.HasKey("CustomerId", "RoomId");
                        j.ToTable("Customer_Room");
                        j.IndexerProperty<int>("CustomerId").HasColumnName("CustomerID");
                        j.IndexerProperty<int>("RoomId").HasColumnName("RoomID");
                    });
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.ToTable("Floor");

            entity.Property(e => e.Floor1).HasColumnName("Floor");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => new { e.InvoiceId, e.RoomId, e.CustomerId });

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("InvoiceID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.InvoiceType)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Customer");

            entity.HasOne(d => d.Room).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Room");
        });

        modelBuilder.Entity<NumOfPerson>(entity =>
        {
            entity.ToTable("NumOfPerson");

            entity.Property(e => e.NumOfPerson1).HasColumnName("NumOfPerson");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.FloorId).HasColumnName("FloorID");
            entity.Property(e => e.NumOfPersonId).HasColumnName("NumOfPersonID");
            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.RoomName)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Area).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Area");

            entity.HasOne(d => d.Floor).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.FloorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Floor");

            entity.HasOne(d => d.NumOfPerson).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.NumOfPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_NumOfPerson");

            entity.HasOne(d => d.Price).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.PriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_RoomPrice");

            entity.HasMany(d => d.Customers1).WithMany(p => p.RoomsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "RoomCustomer",
                    r => r.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Room_Customer_Customer"),
                    l => l.HasOne<Room>().WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Room_Customer_Room"),
                    j =>
                    {
                        j.HasKey("RoomId", "CustomerId");
                        j.ToTable("Room_Customer");
                        j.IndexerProperty<int>("RoomId").HasColumnName("RoomID");
                        j.IndexerProperty<int>("CustomerId").HasColumnName("CustomerID");
                    });
        });

        modelBuilder.Entity<RoomPrice>(entity =>
        {
            entity.ToTable("RoomPrice");

            entity.Property(e => e.Price).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
