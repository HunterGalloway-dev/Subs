using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

// DI, Serilog (logging), Settings

namespace SubscriptionCleanup
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Subscription Cleanup Service Starting...");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                    // Transient - always different; new instance is provided to every controller and every service
                    // Scoped - Same within a request, but different across different requests
                    // Singleton - Objects are the same for every object and every request
                    services.AddTransient<SubscriptionCleanup, SubscriptionCleanup>();
                    services.AddTransient<FileProcessor, FileProcessor>();
                })
                .UseSerilog()
                .Build();

            // Host has DI, Config, looking for IGreetingService instance and give the approriate service
            var svc = ActivatorUtilities.CreateInstance<SubscriptionCleanup>(host.Services);
            svc.GenerateSubscriptionRecord();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            // Builds connection to appsettings json file 
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true) // Set settings file based on level of production
                .AddEnvironmentVariables();
        }
    }
}
