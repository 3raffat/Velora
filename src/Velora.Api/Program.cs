using Scalar.AspNetCore;

using Velora.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();
