using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        new CompactJsonFormatter(), 
        "logs/serilog{Date}.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        buffered: false) // Disable buffering for immediate writes
    .CreateLogger();

builder.Host.UseSerilog(serilogLogger);

// Clear default logging providers to prevent double logging
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(serilogLogger);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();

// Make the Program class public for integration tests
public partial class Program { } 