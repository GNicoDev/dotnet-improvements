using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSignalRHubServices()
    .AddCustomServices();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapSignalRHubs();

app.Run();
