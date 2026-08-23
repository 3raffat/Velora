using DeliveryService.Api;
using DeliveryService.Application;
using DeliveryService.Infrastructure;
using DeliveryService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder
    .Services.AddApplication(builder.Configuration["OrderService:BaseUrl"]!)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.Services.InitializeInfrastructureAsync();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
