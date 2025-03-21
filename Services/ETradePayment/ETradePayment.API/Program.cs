using Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureService(builder.Configuration);
var app = builder.Build();
app.Run();
