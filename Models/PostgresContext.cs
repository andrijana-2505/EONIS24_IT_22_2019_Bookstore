using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendBookstore.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("address_pkey");

            entity.ToTable("address");

            entity.Property(e => e.AddressId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("address_id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.OrdersId).HasColumnName("orders_id");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(5)
                .HasColumnName("postal_code");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");

            entity.HasOne(d => d.Orders).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.OrdersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("address_orders_id_fkey");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("book_pkey");

            entity.ToTable("book");

            entity.Property(e => e.BookId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("book_id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.BookAuthor)
                .HasMaxLength(50)
                .HasColumnName("book_author");
            entity.Property(e => e.BookPrice)
                .HasPrecision(10, 2)
                .HasColumnName("book_price");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(50)
                .HasColumnName("book_title");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Publisher)
                .HasMaxLength(20)
                .HasColumnName("publisher");
            entity.Property(e => e.PublishingYear).HasColumnName("publishing_year");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("book_category_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrdersId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrdersId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orders_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("order_date");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasConversion<string>()
                .HasColumnName("status");
            entity.Property(e => e.StripeTransactionId)
                .HasMaxLength(30)
                .HasColumnName("stripe_transaction_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orders_users_id_fkey");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("orderitem_pkey");

            entity.ToTable("orderitem");

            entity.Property(e => e.OrderItemId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_item_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.OrdersId).HasColumnName("orders_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Book).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderitem_book_id_fkey");

            entity.HasOne(d => d.Orders).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.OrdersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderitem_orders_id_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("review_pkey");

            entity.ToTable("review");

            entity.Property(e => e.ReviewId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("review_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("review_date");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("review_book_id_fkey");

            entity.HasOne(d => d.Users).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("review_users_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsersId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UsersId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("users_id");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.Genre)
                .HasMaxLength(1)
                .HasColumnName("genre");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordLogin)
                .HasMaxLength(100)
                .HasColumnName("password_login");
            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .HasColumnName("phone");
            entity.Property(e => e.UserRole)
                .HasMaxLength(8)
                .HasDefaultValueSql("'Customer'::character varying")
                .HasColumnName("user_role")
                .HasConversion<string>();
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
