using Microsoft.Extensions.Logging;
using Xunit;
using AzureAppServiceSample.Pages;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace AzureAppServiceSample.Tests;

public class PageModelTests
{
    [Fact]
    public void IndexModel_OnGet_ShouldNotThrow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<IndexModel>>();
        var indexModel = new IndexModel(mockLogger.Object);

        // Act & Assert
        var exception = Record.Exception(() => indexModel.OnGet());
        Assert.Null(exception);
    }

    [Fact]
    public void PrivacyModel_OnGet_ShouldNotThrow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<PrivacyModel>>();
        var privacyModel = new PrivacyModel(mockLogger.Object);

        // Act & Assert
        var exception = Record.Exception(() => privacyModel.OnGet());
        Assert.Null(exception);
    }

    [Fact]
    public void ErrorModel_Properties_ShouldWork()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(mockLogger.Object);

        // Act
        errorModel.RequestId = "test-request-id";

        // Assert
        Assert.Equal("test-request-id", errorModel.RequestId);
        Assert.True(errorModel.ShowRequestId);
    }

    [Fact]
    public void ErrorModel_ShowRequestId_ShouldBeFalse_WhenRequestIdIsNull()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(mockLogger.Object);

        // Act
        errorModel.RequestId = null;

        // Assert
        Assert.False(errorModel.ShowRequestId);
    }

    [Fact]
    public void ErrorModel_ShowRequestId_ShouldBeFalse_WhenRequestIdIsEmpty()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(mockLogger.Object);

        // Act
        errorModel.RequestId = "";

        // Assert
        Assert.False(errorModel.ShowRequestId);
    }

    [Fact]
    public void ErrorModel_OnGet_ShouldSetRequestId()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(mockLogger.Object);
        
        // Create a mock HttpContext
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        
        // Set up PageContext
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor());
        var pageContext = new PageContext(actionContext);
        errorModel.PageContext = pageContext;

        // Act
        errorModel.OnGet();

        // Assert
        Assert.NotNull(errorModel.RequestId);
        Assert.Equal("test-trace-id", errorModel.RequestId);
    }
} 