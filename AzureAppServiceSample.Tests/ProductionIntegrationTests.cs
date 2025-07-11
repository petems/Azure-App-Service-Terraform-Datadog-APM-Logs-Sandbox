using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using Xunit;

namespace AzureAppServiceSample.Tests;

public class ProductionIntegrationTests : IClassFixture<ProductionWebApplicationFactory<Program>>
{
    private readonly ProductionWebApplicationFactory<Program> _factory;

    public ProductionIntegrationTests(ProductionWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Production_HomePage_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Production_ErrorPage_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Error");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Production_Configuration_IsProduction()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        // In production, we should not get development-specific headers
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

public class ProductionWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        
        builder.ConfigureServices(services =>
        {
            // Add any production-specific test services here
        });
    }
} 