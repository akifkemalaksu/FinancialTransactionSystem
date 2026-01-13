using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Interfaces;

namespace ServiceDefaults.Implementations
{
    public class CommandDispatcher(
        IServiceProvider _serviceProvider
    ) : ICommandDispatcher
    {
        public Task<TCommandResult> DispatchAsync<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken = default)
        {
            var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TCommandResult>>()
                ?? throw new InvalidOperationException($"No command handler registered for command type {typeof(TCommand).FullName} and result type {typeof(TCommandResult).FullName}");
            return handler.HandleAsync(command, cancellationToken);
        }
    }
}
