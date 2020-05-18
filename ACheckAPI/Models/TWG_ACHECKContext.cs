using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACheckAPI.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ACheckAPI.Models
{
    public partial class TWG_ACHECKContext : DbContext
    {
        private TWG_ACHECKContext _context;
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TWG_ACHECKContext()
        {
        }

        public TWG_ACHECKContext(DbContextOptions<TWG_ACHECKContext> options)
            : base(options)
        {
        }
        public TWG_ACHECKContext(DbContextOptions<TWG_ACHECKContext> options, ILoggerFactory loggerFactory)
        : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<AssetCategory> AssetCategory { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Assign> Assign { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<DeptAsset> DeptAsset { get; set; }
        public virtual DbSet<EavAttribute> EavAttribute { get; set; }
        public virtual DbSet<EavAttributeValue> EavAttributeValue { get; set; }
        public virtual DbSet<Floor> Floor { get; set; }
        public virtual DbSet<AuditTrail> AuditTrail { get; set; }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var temoraryAuditEntities = await AuditNonTemporaryProperties();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await AuditTemporaryProperties(temoraryAuditEntities);
            return result;
        }

        async Task AuditTemporaryProperties(IEnumerable<Tuple<EntityEntry, AuditTrail>> temporatyEntities)
        {
            if (temporatyEntities != null && temporatyEntities.Any())
            {
                await AuditTrail.AddRangeAsync(
                temporatyEntities.ForEach(t => t.Item2.KeyValues = JsonConvert.SerializeObject(t.Item1.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()))
                .Select(t => t.Item2)
                );
                await SaveChangesAsync();
            }
            await Task.CompletedTask;
        }

        async Task<IEnumerable<Tuple<EntityEntry, AuditTrail>>> AuditNonTemporaryProperties()
        {
            Dictionary<string, string> abc = new Dictionary<string, string>();
            ChangeTracker.DetectChanges();
            var entitiesToTrack = ChangeTracker.Entries().Where(e => !(e.Entity is AuditTrail) && e.State != EntityState.Detached && e.State != EntityState.Unchanged);
            var a = entitiesToTrack.Where(e => !e.Properties.Any(p => p.IsTemporary)).Select(e => new AuditTrail()
            {
                Action = Enum.GetName(typeof(EntityState), e.State),
                Table = e.Metadata.Relational().TableName,
                Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                KeyValues = JsonConvert.SerializeObject(e.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                NewValue = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Added || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                OldValue = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue).NullIfEmpty())
            }).ToList();
            List<AuditTrail> lsAuditTrail = new List<AuditTrail>();
            foreach (var res in a)
            {
                string json = JsonConvert.DeserializeObject(res.OldValue == "null" ? "{}" : res.OldValue).DetailedCompare(JsonConvert.DeserializeObject(res.NewValue == "null" ? "{}" : res.NewValue));
                ViewAudit CompareObject = JsonConvert.DeserializeObject<ViewAudit>(json);
                AuditTrail auditTrailObj = new AuditTrail();
                auditTrailObj.AuditTrailId = Guid.NewGuid().ToString();
                auditTrailObj.Action = res.Action;
                auditTrailObj.Table = res.Table;
                auditTrailObj.KeyValues = res.KeyValues;
                auditTrailObj.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                auditTrailObj.OldValue = JsonConvert.SerializeObject(CompareObject.oldValue);
                auditTrailObj.NewValue = JsonConvert.SerializeObject(CompareObject.newValue);
                lsAuditTrail.Add(auditTrailObj);
            }
            await AuditTrail.AddRangeAsync(
                lsAuditTrail.Where(p => p.NewValue != "{}" && p.OldValue != "{}").ToList()
            );

            return entitiesToTrack.Where(e => e.Properties.Any(p => p.IsTemporary))
                 .Select(e => new Tuple<EntityEntry, AuditTrail>(
                     e,
                 new AuditTrail()
                 {
                     AuditTrailId = Guid.NewGuid().ToString(),
                     Action = Enum.GetName(typeof(EntityState), e.State),
                     Table = e.Metadata.Relational().TableName,
                     Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                     KeyValues = JsonConvert.SerializeObject(e.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                     NewValue = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Added || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)),
                     OldValue = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue))
                 }
                 )).ToList();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=MINHYNGUYEN\\MSSQLSERVER16;Initial Catalog=TWG_ACHECK;Integrated Security=True;MultipleActiveResultSets=true");
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

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.AssetCode)
                    .IsRequired()
                    .HasColumnName("Asset_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.AssetName)
                    .HasColumnName("Asset_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Origin).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(16, 3)");

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Folder).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);
            });

            modelBuilder.Entity<AssetCategory>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__AssetCategory__E452D3DB41C896C3");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssetId)
                    .HasColumnName("Asset_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("Category_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetCategory)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("fk_Asset_AssetCategory_1");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.AssetCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_Category_AssetCategory_1");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__Image__E452D3DB41C896C3");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ImageName)
                    .HasColumnName("Image_Name")
                    .HasColumnType("ntext");

                entity.Property(e => e.OriginalName)
                    .HasColumnName("Original_Name")
                    .HasColumnType("ntext");

                entity.Property(e => e.Path).HasColumnType("ntext");

                entity.Property(e => e.ReferenceId)
                    .HasColumnName("Reference_ID")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("fk_Asset_Image_1");

                entity.HasOne(d => d.ReferenceNavigation)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("fk_Asset_Image_2");
            });

            modelBuilder.Entity<Assign>(entity =>
            {
                entity.Property(e => e.AssignId)
                    .HasColumnName("Assign_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
                
                entity.Property(e => e.AssetId)
                    .HasColumnName("Asset_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_At")
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
                    .HasColumnName("Updated_At")
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

                entity.Property(e => e.CategoryType).HasMaxLength(50);

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

                entity.Property(e => e.Path).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Updater).HasMaxLength(50);
            });
            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.Property(e => e.AuditTrailId)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Column).HasMaxLength(50);

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.Date).HasMaxLength(20);

                entity.Property(e => e.NewValue).HasColumnType("ntext");

                entity.Property(e => e.KeyValues).HasMaxLength(250);

                entity.Property(e => e.OldValue).HasColumnType("ntext");

                entity.Property(e => e.Table).HasMaxLength(250);
            });
            modelBuilder.Entity<DeptAsset>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__DeptAsset__E452D3DB41C896C3");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_AT")
                    .HasMaxLength(20);

                entity.Property(e => e.Creater).HasMaxLength(50);

                entity.Property(e => e.Updater).HasMaxLength(50);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AssetId)
                    .HasColumnName("Asset_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.DeptId)
                    .HasColumnName("Dept_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.FromDate)
                    .HasColumnName("From_Date")
                    .HasMaxLength(20);

                entity.Property(e => e.ToDate)
                    .HasColumnName("To_Date")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.DeptAsset)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("fk_Asset_DeptAsset_1");
            });

            modelBuilder.Entity<EavAttribute>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__Eav_Attribute__E452D3DB41C896C3");

                entity.ToTable("Eav_Attribute");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AttributeGroup).HasMaxLength(50);

                entity.Property(e => e.Display).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<EavAttributeValue>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__Eav_Attribute_Value__E452D3DB41C896C3");

                entity.ToTable("Eav_Attribute_Value");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("Category_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.EavId)
                    .HasColumnName("EAV_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.Property(e => e.AttributeGroup).HasMaxLength(50);

                entity.HasOne(d => d.Eav)
                    .WithMany(p => p.EavAttributeValue)
                    .HasForeignKey(d => d.EavId)
                    .HasConstraintName("fk_EAV_Eav_Attribute_Value_1");
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
