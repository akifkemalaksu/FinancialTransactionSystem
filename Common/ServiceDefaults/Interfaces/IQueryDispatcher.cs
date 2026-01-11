namespace ServiceDefaults.Interfaces
{
    public interface IQueryDispatcher
    {
        Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query, CancellationToken cancellationToken = default);
    }
}
