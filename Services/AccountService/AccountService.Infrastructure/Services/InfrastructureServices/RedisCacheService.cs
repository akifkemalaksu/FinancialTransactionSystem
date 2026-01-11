using AccountService.Application.Services.InfrastructureServices;
using StackExchange.Redis;
using System.Text.Json;

namespace AccountService.Infrastructure.Services.InfrastructureServices
{
    public class RedisCacheService : IDistributedCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer _connectionMultiplexer)
        {
            _database = _connectionMultiplexer.GetDatabase();
        }

        public Task<bool> ExistsAsync(string key) => _database.KeyExistsAsync(key);

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value.ToString());
        }

        public Task RemoveAsync(string key) => _database.KeyDeleteAsync(key);

        public Task SetAsync<T>(string key, T value) => _database.StringSetAsync(key, JsonSerializer.Serialize(value));

        public Task SetAsync<T>(string key, T value, TimeSpan timeSpan) => _database.StringSetAsync(key, JsonSerializer.Serialize(value), timeSpan);
    }
}
