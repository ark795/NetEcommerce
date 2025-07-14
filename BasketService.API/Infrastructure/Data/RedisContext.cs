using StackExchange.Redis;
namespace BasketService.API.Infrastructure.Data;
public class RedisContext
{
    private readonly Lazy<ConnectionMultiplexer> _lazy;
    public RedisContext(IConfiguration config)
    {
        _lazy = new Lazy<ConnectionMultiplexer>(() =>
        ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")));
    }
    public IDatabase Database => _lazy.Value.GetDatabase();
}