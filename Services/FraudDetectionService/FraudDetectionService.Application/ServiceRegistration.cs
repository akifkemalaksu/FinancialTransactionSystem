using FraudDetectionService.Application.Features.FraudCheck;
using Microsoft.Extensions.Hosting;
using ServiceDefaults.Extensions;

namespace FraudDetectionService.Application
{
    public static class ServiceRegistration
    {
        public static IHostApplicationBuilder RegisterApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.RegisterCQRSServices(typeof(CheckFraudQuery));

            builder.ConfigureOpenTelemetry();

            return builder;
        }
    }
}
