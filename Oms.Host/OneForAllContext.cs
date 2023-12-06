using System;
using Oms.Domain;
using Oms.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Oms.Host
{
    public partial class OneForAllContext : DbContext
    {
        private Guid _tenantId;
        public OneForAllContext(DbContextOptions<OneForAllContext> options)
            : base(options)
        {

        }

        public OneForAllContext(
            DbContextOptions<OneForAllContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        public virtual DbSet<OmsOrder> OmsOrder { get; set; }
        public virtual DbSet<OmsOrderItems> OmsOrderItems { get; set; }
        public virtual DbSet<OmsWxPaySetting> OmsWxPaySetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OmsOrder>(entity =>
            {
                entity.ToTable("Oms_Order");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<OmsOrderItems>(entity =>
            {
                entity.ToTable("Oms_OrderItems");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OmsWxPaySetting>(entity =>
            {
                entity.ToTable("Oms_WxPaySetting");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });
        }
    }
}
