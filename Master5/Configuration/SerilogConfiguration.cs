using Serilog;

namespace Master5.Configuration;

public static class SerilogConfiguration
{
    public static void SetLoggerConfiguration(HostBuilderContext hostBuilderContext, LoggerConfiguration logger)
    {
        const string logFilePath = "Logs/application_.log";
        const string outputTemplate = "{Timestamp:o} ({Level:u3}) ({SourceContext}) ({ThreadId}) {Message}{NewLine}{Exception}";
        const long maxLogFileSize = 10000000; // 10 MB



        logger
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Async(configuration =>
            {
                configuration
                    .File(
                        path: logFilePath,
                        outputTemplate: outputTemplate,
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: maxLogFileSize,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: null,
                        shared: true);
            })
            .WriteTo.Console();
    }
}