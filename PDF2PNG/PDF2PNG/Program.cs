using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PDF2PNG
{
    public class Program
    {
        static void SetupLogglyConfiguration()
        {
            //Configure Loggly
            var config = LogglyConfig.Instance;
            config.CustomerToken = Settings.LoggilyToken;
            config.ApplicationName = Settings.LoggilyAppName;
            config.Transport = new TransportConfiguration()
            {
                EndpointHostname = Settings.LoggilyEndPoint,
                EndpointPort = Settings.LoggilyPort,
                LogTransport = LogTransport.Https
            };
            config.ThrowExceptions = true;

            //Define Tags sent to Loggly
            config.TagConfig.Tags.AddRange(new ITag[]{
                new ApplicationNameTag {Formatter = "application-{0}"},
                new HostnameTag { Formatter = "host-{0}" }
            });
        }

        public static void Main(string[] args)
        {
            SetupLogglyConfiguration();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Loggly()
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Startup");
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:" + Settings.ApplicationPort)
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
