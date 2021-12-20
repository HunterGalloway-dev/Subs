using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// DI, Serilog (logging), Settings

namespace SubscriptionCleanup
{
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _config;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                // No string interpolation due to logging taking advantage of seeing runNumber not string substring in log
                _log.LogInformation("Run number {runNumber}", i);
            }
        }
    }
}
