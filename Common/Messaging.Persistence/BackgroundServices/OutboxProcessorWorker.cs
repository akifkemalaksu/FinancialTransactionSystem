using Messaging.Abstractions;
using Messaging.Attributes;
using Messaging.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Messaging.Persistence.BackgroundServices
{
    public class OutboxProcessorWorker(
        IServiceProvider _serviceProvider,
        ILogger<OutboxProcessorWorker> _logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Processor Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

                    var actualProducer = scope.ServiceProvider.GetRequiredService<IActualKafkaProducer>();

                    var messages = await dbContext.Set<OutboxMessage>()
                        .Where(m => m.ProcessedOnUtc == null && m.RetryCount < 5)
                        .OrderBy(m => m.OccurredOnUtc)
                        .Take(50)
                        .ToListAsync(stoppingToken);

                    if (messages.Any())
                    {
                        foreach (var message in messages)
                        {
                            try
                            {
                                var msgType = AppDomain.CurrentDomain.GetAssemblies()
                                    .Select(a => a.GetType(message.Type))
                                    .FirstOrDefault(t => t != null);

                                if (msgType is null)
                                {
                                    throw new Exception($"Type '{message.Type}' not found in loaded assemblies.");
                                }

                                var topicAttr = msgType.GetCustomAttribute<KafkaTopicAttribute>();

                                if (topicAttr is null)
                                {
                                    throw new Exception($"KafkaTopicAttribute not found on type '{msgType.Name}'.");
                                }

                                await actualProducer.PublishToKafkaAsync(topicAttr.Name, message.Id.ToString(), message.Content, stoppingToken);

                                message.ProcessedOnUtc = DateTime.UtcNow;
                                _logger.LogInformation("Outbox message sent successfully: {Id}", message.Id);
                            }
                            catch (Exception ex)
                            {
                                message.RetryCount++;
                                message.Error = ex.Message;
                                _logger.LogError(ex, "Error occurred while sending Outbox message: {Id}", message.Id);
                            }
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox Processor encountered an unexpected error.");
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
