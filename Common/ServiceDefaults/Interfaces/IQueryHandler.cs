namespace ServiceDefaults.Interfaces
{
    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }

    public interface IQueryHandler<in TQuery>
    {
        Task HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
