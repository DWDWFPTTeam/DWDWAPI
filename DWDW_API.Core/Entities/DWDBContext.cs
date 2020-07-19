using System;
using DWDW_API.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DWDW_API.Core.Entities
{
    public partial class DWDBContext : DbContext
    {
        public DWDBContext()
        {
        }

        public DWDBContext(DbContextOptions<DWDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arrangement> Arrangement { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Record> Record { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomDevice> RoomDevice { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Constant.B_CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arrangement>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.LocationId })
                    .HasName("FK");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Arrangement)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_UserLocation_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Arrangement)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserLocation_User");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MessageTime).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Notifications_User");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.DeviceCode).HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationCode).HasMaxLength(50);
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasIndex(e => e.DeviceId)
                    .HasName("FK");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.RecordDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Record)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_Record_Device");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.RoleName)
                    .HasColumnName("roleName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasIndex(e => e.LocationId)
                    .HasName("FK");

                entity.Property(e => e.RoomCode).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Room_Location");
            });

            modelBuilder.Entity<RoomDevice>(entity =>
            {
                entity.HasIndex(e => new { e.RoomId, e.DeviceId })
                    .HasName("FK");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.RoomDevice)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_RoomDevice_Device");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomDevice)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_RoomDevice_Room");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasIndex(e => new { e.ArrangementId, e.RoomId })
                    .HasName("FK");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Arrangement)
                    .WithMany(p => p.Shift)
                    .HasForeignKey(d => d.ArrangementId)
                    .HasConstraintName("FK_Shift_UserLocation");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Shift)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Shift_Room");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("FK");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.DeviceToken).HasMaxLength(300);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
