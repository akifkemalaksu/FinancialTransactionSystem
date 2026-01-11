namespace ServiceDefaults.Interfaces
{
    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<in TCommand>
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
