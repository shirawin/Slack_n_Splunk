using System;
using System.Diagnostics;
using Slack.Webhooks;
using Microsoft.Extensions.Logging;

namespace Slack_n_Splunk
{
    public class SlackNotifier
    {
        private readonly ILogger _logger;

        public SlackNotifier(ILogger logger)
        {
            _logger = logger;
        }

        public void NotifySlack(string webhookUrl)
        {
            using (var activity = new Activity("SlackNotification"))
            {
                activity.Start(); 

                var client = new SlackClient(webhookUrl);

                var message = new SlackMessage
                {
                    Text = "New message from Slack_n_Splunk app"
                };

                client.Post(message);

                string machineName = Environment.MachineName;
                string localIp = "192.168.1.191";
                string currentIp = GetLocalIPAddress();

                bool isLocal = currentIp == localIp || machineName == "localhost";

                if (isLocal)
                {
                    _logger.LogWarning("Running in a local environment (Machine: {machineName}, IP: {currentIp})", machineName, currentIp);
                }
                else
                {
                    _logger.LogWarning("Running in a non-local environment (Machine: {machineName}, IP: {currentIp})", machineName, currentIp);
                }

                var loggerInstance = new InstanceLoggingExample(_logger);
                loggerInstance.CouldNotOpenSocket(machineName);

                _logger.LogInformation("Trace ID for this operation: {TraceId}", activity.TraceId);

                _logger.LogInformation("Sent a Slack message to {webhookUrl} by {Username}", webhookUrl, Environment.UserName);
                activity.Stop(); 
            }
        }

        private string GetLocalIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "192.168.1.191"; // ret my local IP
        }
    }
}
