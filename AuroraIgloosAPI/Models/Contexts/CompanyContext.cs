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

    public virtual DbSet<Person> Person { get; set; }
    
    public virtual DbSet<User> User { get; set; }
    
    public virtual DbSet<UserRole> UserRole { get; set; }
    
    public virtual DbSet<UserType> UserType { get; set; }
    
    public virtual DbSet<Trip> Trip { get; set; } = null!;
    
    public virtual DbSet<TripSeason> TripSeason { get; set; } = null!;
    
    public virtual DbSet<TripLevelOfDifficulty> TripLevelOfDifficulty { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=AuroraIgloosEngineering;User Id=sa;Password=JestemArielka123!;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Person>()
            .HasOne(e => e.Address)
            .WithOne()
            .HasForeignKey<Person>(u => u.IdAddress)
            .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.HasIndex(u => u.Login).IsUnique();
        //
        //     entity.HasOne(u => u.UserType)
        //         .WithMany()
        //         .HasForeignKey(u => u.UserTypeId)
        //         .OnDelete(DeleteBehavior.Cascade);
        // });

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserType)
            .WithMany()
            .HasForeignKey(u => u.UserTypeId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.UserRoleId);

        modelBuilder.Entity<Customer>()
            .HasOne(e => e.Person)
            .WithOne()
            .HasForeignKey<Customer>(c => c.IdPerson)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.IdUser)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Person)
            .WithOne()
            .HasForeignKey<Employee>(e => e.IdPerson)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeRole)
            .WithMany()
            .HasForeignKey(e => e.RoleId);
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithOne(u => u.Employee)
            .HasForeignKey<Employee>(e => e.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Booking>()
        //     .HasOne(e => e.Employee)
        //     .WithMany()
        //     .HasForeignKey(e => e.CreatedById)
        //     .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.IdCustomer)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.Igloo)
            .WithMany()
            .HasForeignKey(e => e.IdIgloo)
            .OnDelete(DeleteBehavior.Restrict); 

        //modelBuilder.Entity<Booking>()
        //    .HasOne(e => e.Status)
        //    .WithMany()
        //    .HasForeignKey(e => e.IdStatus);

        modelBuilder.Entity<Booking>()
            .HasOne(e => e.PaymentMethod)
            .WithMany()
            .HasForeignKey(e => e.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Trip)
            .WithMany()
            .HasForeignKey(b => b.TripId)
            .OnDelete(DeleteBehavior.Restrict);

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
        
        modelBuilder.Entity<Igloo>()
            .HasOne(i => i.Discount)
            .WithMany(d => d.Igloos)
            .HasForeignKey(i => i.IdDiscount)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Guide)
            .WithMany(e => e.GuidedTrips)
            .HasForeignKey(t => t.GuideId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
