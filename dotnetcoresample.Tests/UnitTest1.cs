using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using dotnetcoresample.Pages;
using Xunit;

namespace dotnetcoresample.Tests;

public class PageModelTests
{
    [Fact]
    public void IndexModel_Constructor_ShouldInitializeLogger()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<IndexModel>>();

        // Act
        var indexModel = new IndexModel(logger);

        // Assert
        Assert.NotNull(indexModel);
    }

    [Fact]
    public void IndexModel_OnGet_ShouldExecuteWithoutErrors()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<IndexModel>>();
        var indexModel = new IndexModel(logger);

        // Act & Assert
        var exception = Record.Exception(() => indexModel.OnGet());
        Assert.Null(exception);
    }

    [Fact]
    public void PrivacyModel_Constructor_ShouldInitializeLogger()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<PrivacyModel>>();

        // Act
        var privacyModel = new PrivacyModel(logger);

        // Assert
        Assert.NotNull(privacyModel);
    }

    [Fact]
    public void PrivacyModel_OnGet_ShouldExecuteWithoutErrors()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<PrivacyModel>>();
        var privacyModel = new PrivacyModel(logger);

        // Act & Assert
        var exception = Record.Exception(() => privacyModel.OnGet());
        Assert.Null(exception);
    }

    [Fact]
    public void BasicTest_ShouldPass()
    {
        // Arrange
        var expected = 4;

        // Act
        var actual = 2 + 2;

        // Assert
        Assert.Equal(expected, actual);
    }
}