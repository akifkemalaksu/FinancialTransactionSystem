namespace ServiceDefaults.Interfaces
{
    public interface ICommandDispatcher
    {
        Task<TCommandResult> DispatchAsync<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken = default);
    }
}
