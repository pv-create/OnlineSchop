using MassTransit;
using Microsoft.EntityFrameworkCore;
using OnlineSchop.ClientService.Consumers;
using OnlineSchop.ClientService.Data;
using OnlineSchop.ClientService.Interfaces;
using OnlineSchop.ClientService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ValidateClientConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ReceiveEndpoint("client-validation-queue", e =>
        {
            e.ConfigureConsumer<ValidateClientConsumer>(context);
        });
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