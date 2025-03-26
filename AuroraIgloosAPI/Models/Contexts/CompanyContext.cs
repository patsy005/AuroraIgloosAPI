using System;
using System.Collections.Generic;
using AuroraIgloosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Models.Contexts;

public partial class CompanyContext : DbContext
{
    public CompanyContext()
    {
    }

    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Address { get; set; }

    public virtual DbSet<Booking> Booking { get; set; }

    public virtual DbSet<BookingChannel> BookingChannel { get; set; }

    public virtual DbSet<BookingStatus> BookingStatus { get; set; }

    public virtual DbSet<Currency> Currency { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<CustomerNotification> CustomerNotification { get; set; }

    public virtual DbSet<Discount> Discount { get; set; }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }

    public virtual DbSet<ForumCategory> ForumCategory { get; set; }

    public virtual DbSet<ForumComment> ForumComment { get; set; }

    public virtual DbSet<ForumPost> ForumPost { get; set; }

    public virtual DbSet<ForumStatus> ForumStatus { get; set; }

    public virtual DbSet<Gender> Gender { get; set; }

    public virtual DbSet<Igloo> Igloo { get; set; }

    public virtual DbSet<Invoice> Invoice { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<NotificationPriority> NotificationPriority { get; set; }

    public virtual DbSet<NotificationType> NotificationType { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

    public virtual DbSet<Task> Task { get; set; }

    public virtual DbSet<TaskStatus> TaskStatus { get; set; }

    public virtual DbSet<Timezone> Timezone { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Initial Catalog=AuroraIgloos;Integrated Security=True;TrustServerCertificate=True;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
            .HasOne(e => e.Address)
            .WithOne()
            .HasForeignKey<User>(u => u.IdAddress)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<Customer>(c => c.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<Employee>(e => e.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeRole)
            .WithMany()
            .HasForeignKey(e => e.RoleId);

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.IdCustomer);

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.Igloo)
            .WithMany()
            .HasForeignKey(e => e.IdIgloo);

        //modelBuilder.Entity<Booking>()
        //    .HasOne(e => e.Status)
        //    .WithMany()
        //    .HasForeignKey(e => e.IdStatus);

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.PaymentMethod)
            .WithMany()
            .HasForeignKey(e => e.PaymentMethodId);

        //modelBuilder.Entity<Booking>()
        //    .HasOne(e => e.Currency)
        //    .WithMany()
        //    .HasForeignKey(e => e.CurrencyId);

        //modelBuilder.Entity<Booking>()
        //    .HasOne(e => e.BookingChannel)
        //    .WithMany()
        //    .HasForeignKey(e => e.BookingChannelId);

        modelBuilder.Entity<ForumPost>()
            .HasOne(p => p.Employee)
            .WithMany()
            .HasForeignKey(p => p.IdEmployee);

        modelBuilder.Entity<ForumPost>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<ForumPost>()
            .HasMany(p => p.ForumComment)
            .WithOne(c => c.ForumPost);
            //.HasForeignKey(c => c.IdPost);

        modelBuilder.Entity<ForumComment>()
            .HasOne(c => c.Employee)
            .WithMany()
            .HasForeignKey(c => c.IdEmployee);

        modelBuilder.Entity<ForumComment>()
            .HasOne(c => c.ForumPost)
            .WithMany(p => p.ForumComment)
            .HasForeignKey(c => c.IdPost);

        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
