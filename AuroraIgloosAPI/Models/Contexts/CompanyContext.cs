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
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3213E83FF9776E19");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83F90597462");

            entity.Property(e => e.IdStatus).HasDefaultValue(2);

            entity.HasOne(d => d.BookingChannel).WithMany(p => p.Booking).HasConstraintName("FK_Booking_BookingChannel");

            entity.HasOne(d => d.Employee).WithMany(p => p.Booking).HasConstraintName("FK_Booking_Employee");

            entity.HasOne(d => d.Currency).WithMany(p => p.Booking).HasConstraintName("FK_Booking_Currency");

            entity.HasOne(d => d.Customer).WithMany(p => p.Booking).HasConstraintName("FK__Booking__idCusto__48CFD27E");

            entity.HasOne(d => d.Igloo).WithMany(p => p.Booking).HasConstraintName("FK__Booking__idIgloo__49C3F6B7");

            entity.HasOne(d => d.Status).WithMany(p => p.Booking).HasConstraintName("FK_Booking_BookingStatus");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Booking).HasConstraintName("FK__Booking__payment__4AB81AF0");
        });

        modelBuilder.Entity<BookingChannel>(entity =>
        {
            entity.Property(e => e.Nazwa).IsFixedLength();
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingS__3213E83FF3D92774");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.Currency1).IsFixedLength();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83FEC7671A5");

            entity.HasOne(d => d.User).WithMany(p => p.Customer).HasConstraintName("FK__Customer__idUser__3C69FB99");
        });

        modelBuilder.Entity<CustomerNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83F33396710");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.CustomerNotification).HasConstraintName("FK__CustomerN__idCus__52593CB8");

            entity.HasOne(d => d.NotificationPriority).WithMany(p => p.CustomerNotification).HasConstraintName("FK_CustomerNotification_NotificationPriority");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.CustomerNotification).HasConstraintName("FK_CustomerNotification_NotificationType");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3213E83FEBFF7096");

            entity.HasOne(d => d.Igloo).WithMany(p => p.Discount).HasConstraintName("FK__Discount__idIglo__4F7CD00D");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FFF3E587E");

            entity.HasOne(d => d.User).WithMany(p => p.Employee).HasConstraintName("FK__Employee__idUser__412EB0B6");

            entity.HasOne(d => d.Role).WithMany(p => p.Employee).HasConstraintName("FK__Employee__roleId__4222D4EF");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F3C82F2D9");
        });

        modelBuilder.Entity<ForumCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumCat__3213E83FAE556A82");
        });

        modelBuilder.Entity<ForumComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumCom__3213E83F73283A0E");

            entity.HasOne(d => d.Employee).WithMany(p => p.ForumComment).HasConstraintName("FK__ForumComm__idEmp__619B8048");

            entity.HasOne(d => d.ForumPost).WithMany(p => p.ForumComment).HasConstraintName("FK__ForumComm__idPos__60A75C0F");
        });

        modelBuilder.Entity<ForumPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumPos__3213E83FFD00D0DD");

            entity.HasOne(d => d.Category).WithMany(p => p.ForumPost).HasConstraintName("FK_ForumPost_Category");

            entity.HasOne(d => d.Employee).WithMany(p => p.ForumPost).HasConstraintName("FK__ForumPost__idEmp__5DCAEF64");

            entity.HasOne(d => d.Status).WithMany(p => p.ForumPost).HasConstraintName("FK_ForumPost_ForumStatus");
        });

        modelBuilder.Entity<ForumStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumSta__3213E83FCB6D7209");
        });

        modelBuilder.Entity<Igloo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Igloo__3213E83F6828342C");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoice__3213E83F10EA02A9");

        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3213E83F873FCF32");
        });

        modelBuilder.Entity<NotificationPriority>(entity =>
        {
            entity.Property(e => e.Name).IsFixedLength();
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.Property(e => e.Type).IsFixedLength();
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3213E83F28D66F6F");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3213E83F35C81F8A");

            entity.HasOne(d => d.Employee).WithMany(p => p.Task).HasConstraintName("FK__Task__idEmployee__59FA5E80");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Task).HasConstraintName("FK__Task__idStatus__5AEE82B9");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskStat__3213E83F8A589755");
        });

        modelBuilder.Entity<Timezone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Timezone__3213E83F7EB1151E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F9BF3A7F1");

            entity.HasOne(d => d.Gender).WithMany(p => p.User).HasConstraintName("FK_User_Gender");

            entity.HasOne(d => d.Address).WithMany(p => p.User).HasConstraintName("FK__User__idAddress__398D8EEE");

            entity.HasOne(d => d.Language).WithMany(p => p.User).HasConstraintName("FK_User_Language");

            entity.HasOne(d => d.Timezone).WithMany(p => p.User).HasConstraintName("FK_User_Timezone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
