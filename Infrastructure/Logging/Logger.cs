using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Infrastructure.Logging;

public static class Logger
{
    public static Serilog.Core.Logger Create(string connectionString)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(connectionString))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                BatchAction = ElasticOpType.Create,
                TypeName = null,
                IndexFormat = "SerilogToElastic-{0:yyyy-MM-dd}"
            })
            .CreateLogger();
    }
}