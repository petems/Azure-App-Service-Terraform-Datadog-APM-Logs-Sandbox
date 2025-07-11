# .NET 8 Hello World

This sample demonstrates a tiny Hello World .NET Core app for App Service Web App. This sample can be used in a .NET Azure App Service app as well as in a Custom Container Azure App Service app.

## Features

- ASP.NET Core 8.0 Razor Pages
- Bootstrap 5 for styling
- Docker support (Linux and Windows containers)
- Azure App Service deployment ready
- Terraform infrastructure as code

## Running the Application

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, for containerized deployment)

### Local Development

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

### Docker Deployment

#### Linux Container

```bash
docker build -f Dockerfile.linux -t dotnetcore-docs-hello-world-linux .
docker run -p 8080:8080 dotnetcore-docs-hello-world-linux
```

#### Windows Container

```bash
docker build -f Dockerfile.windows -t dotnetcore-docs-hello-world-windows .
docker run -p 8080:8080 dotnetcore-docs-hello-world-windows
```

## Azure Deployment

This project includes Terraform configuration to deploy to Azure App Service. Update the `main.tf` file with your specific configuration and run:

```bash
terraform init
terraform plan
terraform apply
```

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). 

# Application using manual instrumentation

This sample web application shows how to manually instrument an ASP.NET Core application using Datadog .NET APM library.

## Build and Testing

This project includes automated testing and continuous integration:

### Unit Tests

The project includes a comprehensive test suite located in `dotnetcoresample.Tests/` that covers:

- Page model functionality (Index and Privacy pages)
- Constructor validation
- Method execution testing
- Basic framework verification

To run tests locally:
```bash
dotnet test
```

### GitHub Actions Workflow

The repository includes a GitHub Actions workflow (`.github/workflows/main.yml`) that:

1. **Test Job**: Runs all unit tests with code coverage collection
2. **Build Job**: Compiles the application in Release mode (only runs if tests pass)
3. **Deploy Job**: Deploys to Azure Web App (only runs if build succeeds)

The workflow ensures that:
- All tests must pass before deployment
- Code coverage is collected and artifacts are uploaded
- Failed tests prevent deployment to production

### Test Structure

The test project uses:
- **xUnit** as the testing framework
- **Microsoft.AspNetCore.Mvc.Testing** for ASP.NET Core integration testing
- **Microsoft.Extensions.DependencyInjection** for dependency injection in tests

Tests are automatically discovered and run by the test runner and include proper dependency injection setup for testing ASP.NET Core components. 