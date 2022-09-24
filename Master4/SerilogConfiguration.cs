using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Master4;

/// <summary>
/// Configures Serilog Logger
/// </summary>
public static class SerilogConfiguration
{
	#region Private Members

    private const string DEFAULT_DIRECTORYPATH = "C:\\Logs";
    private const string LOG_FILENAME = "application_.log";
    private const long MAX_LOGFILE_SIZE = 10000000; // 10 MB
    private const string DEFAULT_OUTPUT_TEMPLATE = "{Timestamp:o} ({Level:u3}) ({SourceContext}) ({ThreadId}) {Message}{NewLine}{Exception}";

    private const string LoggingSection = "Logging";
	private const string OutputTemplate = "OutputTemplate";

	private const string SubLoggerSection = "Subloggers";
	private const string EnrichSection = "Enrich";

	private const string DirectoryPath = "DirectoryPath";
	private const string FilterExpression = "FilterExpression";

	private static IHostEnvironment HostingEnvironment;

	#endregion Private Members

	/// <summary>
	/// Configures the Serilog logger for the application
	/// </summary>
	/// <param name="hostBuilderContext"></param>
	/// <param name="logger"></param>
	public static void SetLoggerConfiguration(HostBuilderContext hostBuilderContext, LoggerConfiguration logger)
	{
		HostingEnvironment = hostBuilderContext.HostingEnvironment;

		bool sectionExists = hostBuilderContext.Configuration.GetChildren().Any(item => item.Key == LoggingSection);

		if (!sectionExists) return;

		IConfigurationSection loggingSection = hostBuilderContext.Configuration.GetSection("Logging");

		logger.SetMinimumLogLevel(loggingSection);

		logger.SetLogFilter(loggingSection);

		logger.SetEnrichers(loggingSection);

		if (loggingSection.GetChildren().Any(item => item.Key == SubLoggerSection))
		{
			logger.SetSubLoggers(loggingSection);
		}
		else
		{
			logger.SetLogSink(loggingSection);
		}
	}

	private static void SetSubLoggers(this LoggerConfiguration logger, IConfigurationSection loggingSection)
	{
		IConfigurationSection subloggerSection = loggingSection.GetSection(SubLoggerSection);

		//If sub-logger exists then all the loggers should be registered as sub-logger only to avoid individual filters

		logger.WriteTo.Logger((sublogger) =>
		{
			sublogger.SetLogSink(loggingSection);
			//Also exclude filters of all individual sub-loggers
			sublogger.ExcludeFilterExpressions(subloggerSection.GetChildren());
		});

		//Now configure individual sub-loggers;
		foreach (IConfigurationSection subloggerConfig in subloggerSection.GetChildren())
		{
			logger.WriteTo.Logger((sublogger) =>
			{
				sublogger.SetSubLogSink(loggingSection, subloggerConfig);
				sublogger.IncludeFilterExpression(subloggerConfig);
			});
		}
	}

	/// <summary>
	/// Sets the Serilog Sink
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="configurationSection"></param>
	private static void SetLogSink(this LoggerConfiguration logger, IConfigurationSection configurationSection)
	{
		string directoryPath = configurationSection[DirectoryPath];
		if (string.IsNullOrWhiteSpace(directoryPath))
		{
			directoryPath = DEFAULT_DIRECTORYPATH;
		}

		string logFilePath = Path.Combine(directoryPath, LOG_FILENAME);
		logger.SetLogSink(configurationSection, logFilePath);
	}

	private static void SetSubLogSink(this LoggerConfiguration logger, IConfigurationSection parentConfigSection, IConfigurationSection subLogConfigSection)
	{
		string directoryPath = subLogConfigSection[DirectoryPath];
		if (string.IsNullOrWhiteSpace(directoryPath)) return;
		string logFilePath = Path.Combine(directoryPath, LOG_FILENAME);

		logger.SetLogSink(parentConfigSection, logFilePath, subLogConfigSection);
	}

	private static void SetLogSink(this LoggerConfiguration logger, IConfigurationSection configurationSection, string logFilePath, IConfigurationSection subLogConfigSection = null)
	{
		if (!long.TryParse(configurationSection["MaxLogFileSize"], out long maxLogFileSize))
		{
			maxLogFileSize = MAX_LOGFILE_SIZE;
		}

		bool useElasticSearchFormatting = bool.TryParse(configurationSection["UseElasticSearch"], out bool useElasticSearch) && useElasticSearch;

		IConfigurationSection outputTemplateConfigSection = subLogConfigSection ?? configurationSection;
		string outputTemplate = outputTemplateConfigSection[OutputTemplate];
		if (string.IsNullOrEmpty(outputTemplate)) outputTemplate = DEFAULT_OUTPUT_TEMPLATE;

		logger.SetFileSinkAsText(logFilePath, maxLogFileSize, outputTemplate);
		if (bool.TryParse(configurationSection["UseConsoleLogging"], out bool useConsoleLogging) && useConsoleLogging)
		{
			logger.AddConsoleLogging();
		}
	}

	private static void AddConsoleLogging(this LoggerConfiguration logger)
	{
		logger.WriteTo.Console();
	}

	/// <summary>
	/// Configures the Serilog file sink to output in Text format. Useful for Local machines.
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="logFilePath"></param>
	/// <param name="maxLogFileSize"></param>
	/// <param name="outputTemplate"></param>
	private static void SetFileSinkAsText(this LoggerConfiguration logger, string logFilePath, long maxLogFileSize, string outputTemplate)
	{
        logger.WriteTo.Async(s => s.File(
            path: logFilePath,
            outputTemplate: outputTemplate,
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: maxLogFileSize,
            rollOnFileSizeLimit: true,
            retainedFileCountLimit: null,
            shared: true));
	}

	/// <summary>
	/// Sets the minimum log level in the configuration
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="configurationSection"></param>
	private static void SetMinimumLogLevel(this LoggerConfiguration logger, IConfigurationSection configurationSection)
	{
		string minimumLevelFromConfiguration = configurationSection["MinimumLevel"];
		if (string.IsNullOrEmpty(minimumLevelFromConfiguration))
		{
			return;
		}

		if (Enum.TryParse(minimumLevelFromConfiguration, true, out LogLevel minimumLevel))
		{
			switch (minimumLevel)
			{
				case LogLevel.Debug:
					logger.MinimumLevel.Debug();
					break;
				case LogLevel.Information:
					logger.MinimumLevel.Information();
					break;
				case LogLevel.Warning:
					logger.MinimumLevel.Warning();
					break;
				case LogLevel.Error:
					logger.MinimumLevel.Error();
					break;
				case LogLevel.Critical:
					logger.MinimumLevel.Fatal();
					break;
				case LogLevel.Trace:
					logger.MinimumLevel.Verbose();
					break;
				default:
					logger.MinimumLevel.Information();
					break;

			}
		}
	}

	/// <summary>
	/// Configures the filters to be applied on the logs before writing
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="configurationSection"></param>
	private static void SetLogFilter(this LoggerConfiguration logger, IConfigurationSection configurationSection)
	{
		if (bool.TryParse(configurationSection["OnlyApplicationLogs"], out bool logOnlyApplication) && logOnlyApplication)
		{
			logger.Filter.ByExcluding("StartsWith(SourceContext, 'Microsoft.')");
			logger.Filter.ByExcluding("StartsWith(SourceContext, 'System.Net.Http.HttpClient.')");
			logger.Filter.ByExcluding("StartsWith(SourceContext, 'Grpc.')");
			logger.Filter.ByExcluding("StartsWith(SourceContext, 'ProtoBuf.')");
		}
	}

	private static void ExcludeFilterExpressions(this LoggerConfiguration logger, IEnumerable<IConfigurationSection> configurationSections)
	{
		foreach (IConfigurationSection configurationSection in configurationSections)
		{
			logger.Filter.ByExcluding(configurationSection[FilterExpression]);
		}
	}

	private static void IncludeFilterExpression(this LoggerConfiguration logger, IConfigurationSection configurationSection)
	{
		logger.Filter.ByIncludingOnly(configurationSection[FilterExpression]);
	}

	private static void SetEnrichers(this LoggerConfiguration logger, IConfigurationSection configurationSection)
	{
		IConfigurationSection enrichSection = configurationSection.GetChildren().FirstOrDefault(item => item.Key == EnrichSection);
		if (enrichSection == null)
			return;

		foreach (IConfigurationSection enrichWithItem in enrichSection?.GetChildren())
		{
			switch (enrichWithItem.Value)
			{
				case "FromLogContext":
					logger.Enrich.FromLogContext();
					break;
				case "WithMachineName":
					logger.Enrich.WithMachineName();
					break;
				case "WithProcessId":
					logger.Enrich.WithProcessId();
					break;
				case "WithThreadId":
					logger.Enrich.WithThreadId();
					break;
			}
		}
	}

}
