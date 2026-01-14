using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ServiceDefaults.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(
                    serviceName: builder.Configuration["OTEL_SERVICE_NAME"] ?? builder.Environment.ApplicationName,
                    serviceVersion: "1.0.0")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment"] = builder.Environment.EnvironmentName,
                    ["host.name"] = Environment.MachineName
                });

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(resourceBuilder);
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;

                logging.AddOtlpExporter();
            });

            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddOtlpExporter();
                })
                .WithTracing(tracing =>
                {
                    tracing.SetResourceBuilder(resourceBuilder);

                    if (builder.Environment.IsDevelopment())
                    {
                        tracing.SetSampler(new AlwaysOnSampler());
                    }
                    else
                    {
                        tracing.SetSampler(new TraceIdRatioBasedSampler(0.1));
                    }

                    tracing.AddAspNetCoreInstrumentation(n => {
                        n.RecordException = true;
                    })
                    .AddHttpClientInstrumentation(n => {
                        n.RecordException = true;
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter();
                });

            return builder;
        }
    }
}
