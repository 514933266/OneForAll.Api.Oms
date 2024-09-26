using System;
using Oms.Domain;
using Oms.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Oms.Host
{
    public partial class OmsDbContext : DbContext
    {
        private Guid _tenantId;
        public OmsDbContext(DbContextOptions<OmsDbContext> options)
            : base(options)
        {

        }

        public OmsDbContext(
            DbContextOptions<OmsDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        public virtual DbSet<OmsOrder> OmsOrder { get; set; }
        public virtual DbSet<OmsOrderItem> OmsOrderItem { get; set; }
        public virtual DbSet<OmsOrderLog> OmsOrderLog { get; set; }
        public virtual DbSet<OmsWxPaySetting> OmsWxPaySetting { get; set; }
        public virtual DbSet<OmsOrderCallbackRecord> OmsOrderCallbackRecord { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OmsOrder>(entity =>
            {
                entity.ToTable("Oms_Order");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<OmsOrderItem>(entity =>
            {
                entity.ToTable("Oms_OrderItem");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OmsOrderLog>(entity =>
            {
                entity.ToTable("Oms_OrderLog");
                entity.Property(e => e.OmsOrderId).ValueGeneratedNever();
            });

            modelBuilder.Entity<OmsWxPaySetting>(entity =>
            {
                entity.ToTable("Oms_WxPaySetting");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<OmsOrderCallbackRecord>(entity =>
            {
                entity.ToTable("Oms_OrderCallbackRecord");
                entity.Property(e => e.OmsOrderId).ValueGeneratedNever();
            });
        }
    }
}
