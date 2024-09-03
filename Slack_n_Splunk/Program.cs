using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using Slack_n_Splunk;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.Tags | ActivityTrackingOptions.Baggage;
                })
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true; // Log everything on a single line if preferred
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                    options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Default;
                })
                .AddEventLog(eventLogSettings =>
                {
                    eventLogSettings.SourceName = "Slack_n_Splunk";  // Set the source name for the event log
                    eventLogSettings.LogName = "Application";  // Set the log name (Application is common)
                })
                .AddOpenTelemetry(options =>
                {
                    // No need to specify a URI, using default settings to send to the local collector
                    options.AddOtlpExporter();
                    
                });
            })
            .BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        // Create an instance of your SlackNotifier and use it
        var slackNotifier = new SlackNotifier(logger);
        slackNotifier.NotifySlack("https://hooks.slack.com/services/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");

        Console.ReadLine();
    }
}
