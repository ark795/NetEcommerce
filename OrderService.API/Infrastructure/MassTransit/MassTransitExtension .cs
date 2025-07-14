using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Infrastructure.Persistence;
using OrderService.API.Saga;
namespace OrderService.API.Infrastructure.MassTransit;
public static class MassTransitExtension
{
    public static void AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState>()
            .EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                //r.AddDbContext<DbContext, ApplicationDbContext>((provider, builder) =>
                //{
                //    builder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres.pass;Database=orderDB");
                //});
            });
            x.SetKebabCaseEndpointNameFormatter();
            //x.SetInMemorySagaRepositoryProvider();
            x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.UsePostgres();
                o.UseBusOutbox();
            });
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}