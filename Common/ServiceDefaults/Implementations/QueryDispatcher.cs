using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Interfaces;

namespace ServiceDefaults.Implementations
{
    public class QueryDispatcher(
        IServiceProvider _serviceProvider
    ) : IQueryDispatcher
    {
        public Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query, CancellationToken cancellationToken = default)
        {
            var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>()
                ?? throw new InvalidOperationException($"No query handler registered for query type {typeof(TQuery).FullName} and result type {typeof(TQueryResult).FullName}");
            return handler.HandleAsync(query, cancellationToken);
        }
    }
}
