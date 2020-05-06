using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACheckAPI.Models
{
    public partial class TWG_ACHECKContext : DbContext
    {
        public TWG_ACHECKContext()
        {
        }

        public TWG_ACHECKContext(DbContextOptions<TWG_ACHECKContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<Assign> Assign { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Floor> Floor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MINHYNGUYEN\\MSSQLSERVER16;Initial Catalog=TWG_ACHECK;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.AssetId)
                    .HasColumnName("Asset_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AssetCode)
                    .IsRequired()
                    .HasColumnName("Asset_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.AssetName)
                    .HasColumnName("Asset_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("Category_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.FloorId)
                    .HasColumnName("Floor_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Folder).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Asset)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_Asset_Category_1");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.Asset)
                    .HasForeignKey(d => d.FloorId)
                    .HasConstraintName("fk_Asset_Floor_1");
            });

            modelBuilder.Entity<Assign>(entity =>
            {
                entity.Property(e => e.AssignId)
                    .HasColumnName("Assign_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AssetId)
                    .IsRequired()
                    .HasColumnName("Asset_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.FromDate)
                    .HasColumnName("From_Date")
                    .HasMaxLength(20);

                entity.Property(e => e.ReceiverBy)
                    .HasColumnName("Receiver_By")
                    .HasMaxLength(50);

                entity.Property(e => e.Supporter).HasMaxLength(50);

                entity.Property(e => e.ToDate)
                    .HasColumnName("To_Date")
                    .HasMaxLength(20);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Assign)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Asset_Assign_1");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.Property(e => e.BuildingId)
                    .HasColumnName("Building_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.BuildingAddress)
                    .HasColumnName("Building_Address")
                    .HasMaxLength(50);

                entity.Property(e => e.BuildingName)
                    .HasColumnName("Building_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.FloorNumber)
                    .HasColumnName("Floor_Number")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasColumnName("Category_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("Category_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Level).HasMaxLength(50);

                entity.Property(e => e.ParentId)
                    .HasColumnName("Parent_ID")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.Property(e => e.FloorId)
                    .HasColumnName("Floor_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.BuildingId)
                    .IsRequired()
                    .HasColumnName("Building_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.FloorName)
                    .HasColumnName("Floor_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.FloorNumber)
                    .HasColumnName("Floor_Number")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Floor)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Building_Floor_1");
            });
        }
    }
}
