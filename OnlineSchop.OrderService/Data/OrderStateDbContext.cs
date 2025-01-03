using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchop.OrderService.Saga;

namespace OnlineSchop.OrderService.Data
{
    public class OrderStateDbContext : SagaDbContext
    {
        public OrderStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }
    }

    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.FailureReason).HasMaxLength(1024);
        }
    }
}