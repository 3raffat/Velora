using Hangfire;
using Velora.Api;
using Velora.Application;
using Velora.Infrastructure;
using Velora.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Host.UseDefaultServiceProvider(opt =>
{
    opt.ValidateOnBuild = true;
    opt.ValidateScopes = true;
});

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddApplication().AddPresentation().AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/jobs");

HangfireExtensions.RegisterRecurringJobs();

app.Run();
