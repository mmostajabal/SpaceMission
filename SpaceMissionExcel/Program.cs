using SpaceMissionExcel;
using SpaceMissionExcel.Classes;
using SpaceMissionExcel.CommonClass;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
ReadConfig.LoadConfig();

HttpClient httpClient = new HttpClient
{
    BaseAddress = new Uri(CommonVariable.BASEURL_API.Trim()), // Replace with your API URL
    Timeout = TimeSpan.FromSeconds(30) // Adjust timeout as needed
};
builder.Services.AddSingleton<HttpClient>(httpClient);

var host = builder.Build();
host.Run();
