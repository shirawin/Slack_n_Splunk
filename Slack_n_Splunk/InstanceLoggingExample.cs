using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Slack_n_Splunk
{
    public partial class InstanceLoggingExample
    {
        private readonly ILogger _logger;

        public InstanceLoggingExample(ILogger logger)
        {
            _logger = logger;
        }

        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Critical,
            Message = "Could not open socket to `{hostName}`")]
        public partial void CouldNotOpenSocket(string hostName);
    }
}
