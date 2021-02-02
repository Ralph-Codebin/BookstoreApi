using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System;

namespace bookstore_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            //.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
            .MinimumLevel.Override("Application.ErrorHandlers.RepositoryExceptionHandler",LogEventLevel.Information)
            .MinimumLevel.Error()
            .WriteTo.MSSqlServer(
                connectionString: config.GetConnectionString("BookstoreDataDatabase"),
                sinkOptions: new SinkOptions { TableName = "Log" }
                )
            .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
    
}