var builder = WebApplication.CreateBuilder(args);

// Configure logging with scopes enabled
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(opts =>
{
    opts.IncludeScopes = true; // Enable scopes for correlation identifiers
});
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

// Make the Program class public for integration tests
public partial class Program { } 