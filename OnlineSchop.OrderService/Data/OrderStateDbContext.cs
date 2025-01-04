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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка схемы для PostgreSQL
            modelBuilder.HasDefaultSchema("public");
            
            // Добавление поддержки UUID
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }
    }

    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            // Настройка таблицы
            entity.ToTable("order_states");

            // Настройка первичного ключа
            entity.HasKey(x => x.CorrelationId);
            
            // Настройка свойств
            entity.Property(x => x.CorrelationId)
                .HasColumnName("correlation_id")
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.Property(x => x.CurrentState)
                .HasColumnName("current_state")
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            entity.Property(x => x.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            entity.Property(x => x.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.OrderDate)
                .HasColumnName("order_date")
                .IsRequired();

            entity.Property(x => x.IsClientValidated)
                .HasColumnName("is_client_validated");

            entity.Property(x => x.IsOrderValidated)
                .HasColumnName("is_order_validated");

            entity.Property(x => x.FailureReason)
                .HasColumnName("failure_reason")
                .HasMaxLength(1024);

            // Добавление индексов
            entity.HasIndex(x => x.OrderId)
                .HasDatabaseName("ix_order_states_order_id");

            entity.HasIndex(x => x.ClientId)
                .HasDatabaseName("ix_order_states_client_id");

            entity.HasIndex(x => x.CurrentState)
                .HasDatabaseName("ix_order_states_current_state");
        }
    }
}