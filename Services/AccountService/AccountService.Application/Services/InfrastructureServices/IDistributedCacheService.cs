namespace AccountService.Application.Services.InfrastructureServices
{
    public interface IDistributedCacheService
    {
        Task<bool> ExistsAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, T value, TimeSpan timeSpan);
        Task RemoveAsync(string key);
    }
}