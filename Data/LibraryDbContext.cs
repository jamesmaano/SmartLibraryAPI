using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Data;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCatalog> BookCatalogs { get; set; }

    public virtual DbSet<Catalog> Catalogs { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=LibraryManagementSystem;Username=TheTwoPoints;Password=5212006");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.HasIndex(e => e.Email, "IX_accounts_Email");

            entity.HasIndex(e => e.Username, "IX_accounts_Username").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("books");

            entity.HasIndex(e => e.Isbn, "IX_books_ISBN");

            entity.HasIndex(e => e.Title, "IX_books_Title");

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<BookCatalog>(entity =>
        {
            entity.ToTable("book_catalogs");

            entity.HasIndex(e => new { e.BookId, e.CatalogId }, "IX_book_catalogs_BookId_CatalogId").IsUnique();

            entity.HasIndex(e => e.CatalogId, "IX_book_catalogs_CatalogId");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCatalogs).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.Catalog).WithMany(p => p.BookCatalogs).HasForeignKey(d => d.CatalogId);
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.ToTable("catalogs");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.ToTable("fines");

            entity.HasIndex(e => e.LoanId, "IX_fines_LoanId");

            entity.HasIndex(e => e.UserId, "IX_fines_UserId");

            entity.Property(e => e.Amount).HasPrecision(10, 2);
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.IssuedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("loans");

            entity.HasIndex(e => e.BookId, "IX_loans_BookId");

            entity.HasIndex(e => e.UserId, "IX_loans_UserId");

            entity.Property(e => e.BorrowDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsReturned).HasDefaultValue(false);

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("reservations");

            entity.HasIndex(e => e.BookId, "IX_reservations_BookId");

            entity.HasIndex(e => e.UserId, "IX_reservations_UserId");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ReservationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "IX_users_Email");

            entity.HasIndex(e => e.EmployeeId, "IX_users_EmployeeId");

            entity.HasIndex(e => e.StudentId, "IX_users_StudentId");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmployeeId).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RegisteredDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.StudentId).HasMaxLength(50);
            entity.Property(e => e.UserType).HasMaxLength(8);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
