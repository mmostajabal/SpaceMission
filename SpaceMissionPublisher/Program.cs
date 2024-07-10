using Serilog;
using SpaceMissionPublisher;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        DateTime dateTimeFile = DateTime.Now;
        string fileName = dateTimeFile.Year.ToString() + dateTimeFile.Month.ToString() + dateTimeFile.Day.ToString();

        builder.Services.AddHostedService<Worker>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            //.WriteTo.Console()
            .WriteTo.File("logs/" + fileName + ".txt")
            .CreateLogger();

        var host = builder.Build();
        host.Run();
    }
}