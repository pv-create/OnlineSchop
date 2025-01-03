using MassTransit;
using Microsoft.EntityFrameworkCore;
using OnlineSchop.OrderService.Data;
using OnlineSchop.OrderService.Saga;
using OrderService.StateMachines;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddDbContext<OrderStateDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
            {
                builder.UseSqlServer(connectionString);
            });
        });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
