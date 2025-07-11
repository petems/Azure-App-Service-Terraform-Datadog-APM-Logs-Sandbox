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