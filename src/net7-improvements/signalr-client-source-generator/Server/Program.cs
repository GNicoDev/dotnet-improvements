using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSignalRHubServices()
    .AddCustomServices();

var app = builder.Build();

app.MapSignalRHubs();

app.Run();
