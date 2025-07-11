# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.36.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.5.0"
    }
  }
  required_version = ">= 1.0.0"
}

provider "azurerm" {
  features {}
}

# Variables
variable "datadog_api_key" {
  description = "Datadog API Key for monitoring"
  type        = string
  sensitive   = true
}

variable "datadog_site" {
  description = "Datadog site (e.g., datadoghq.com, datadoghq.eu, us3.datadoghq.com)"
  type        = string
  default     = "datadoghq.com"
}

# Generate a random integer to create a globally unique name
resource "random_integer" "ri" {
  min = 10000
  max = 99999
}

# Create the resource group (if in your own Azure account)
# This is not needed if you are using the Datadog sandbox repo
# However it switches all the logic to use data instead of a created resource
# So for now, we'll lookup the existing resource group
# resource "azurerm_resource_group" "rg" {
#   name     = "myResourceGroup-${random_integer.ri.result}"
#   location = "eastus"
# }

# Use the existing default eastus resource group (using the Datadog sandbox repo)
data "azurerm_resource_group" "rg" {
  name = "DefaultResourceGroup-EUS"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "webapp-asp-${random_integer.ri.result}"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B1"
}

# Create the web app with Datadog configuration
resource "azurerm_linux_web_app" "webapp" {
  name                  = "webapp-${random_integer.ri.result}"
  location              = data.azurerm_resource_group.rg.location
  resource_group_name   = data.azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  depends_on            = [azurerm_service_plan.appserviceplan]
  https_only            = true
  
  app_settings = {
    # Datadog configuration for main app
    "DD_API_KEY"             = var.datadog_api_key
    "DD_SITE"                = var.datadog_site
    "DD_SERVICE"             = "dotnetcore-hello-world"
    "DD_VERSION"             = "1.0.0"
    "DD_ENV"                 = "production"
    "DD_LOGS_INJECTION"      = "true"
    "DD_TRACE_ENABLED"       = "true"
    "DD_APM_ENABLED"         = "true"
    # Sidecar container settings
    "DD_APM_NON_LOCAL_TRAFFIC" = "true"
    "DD_LOGS_ENABLED"        = "true"
    # Required for sidecar log collection
    "DD_SERVERLESS_LOG_PATH" = "/home/LogFiles/*.log"
    # Enable App Service Storage for sidecar access
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "true"
    # .NET-specific Datadog configuration
    "DD_DOTNET_TRACER_HOME"  = "/home/site/wwwroot/datadog"
    "DD_TRACE_LOG_DIRECTORY" = "/home/LogFiles/dotnet"
    "CORECLR_ENABLE_PROFILING" = "1"
    "CORECLR_PROFILER"       = "{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
    "CORECLR_PROFILER_PATH"  = "/home/site/wwwroot/datadog/linux-musl-x64/Datadog.Trace.ClrProfiler.Native.so"
    "DD_PROFILING_ENABLED"   = "true"
  }

  site_config { 
    minimum_tls_version = "1.2"
    application_stack {
      dotnet_version = "8.0"
    }
  }
}

# Add Datadog sidecar container using Azure REST API resource
resource "azurerm_resource_group_template_deployment" "datadog_sidecar" {
  name                = "datadog-sidecar-${random_integer.ri.result}"
  resource_group_name = data.azurerm_resource_group.rg.name
  deployment_mode     = "Incremental"
  
  template_content = jsonencode({
    "$schema"      = "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#"
    contentVersion = "1.0.0.0"
    resources = [
      {
        type       = "Microsoft.Web/sites/sitecontainers"
        apiVersion = "2024-04-01"
        name       = "${azurerm_linux_web_app.webapp.name}/datadog-agent"
        properties = {
          image      = "datadog/serverless-init:latest"
          isMain     = false
          authType   = "Anonymous"
          targetPort = "8126"
          volumeMounts = []
          environmentVariables = [
            {
              name  = "DD_API_KEY"
              value = "DD_API_KEY"
            },
            {
              name  = "DD_SITE"
              value = "DD_SITE"
            },
            {
              name  = "DD_APM_ENABLED"
              value = "DD_APM_ENABLED"
            },
            {
              name  = "DD_APM_NON_LOCAL_TRAFFIC"
              value = "DD_APM_NON_LOCAL_TRAFFIC"
            },
            {
              name  = "DD_LOGS_ENABLED"
              value = "DD_LOGS_ENABLED"
            }
          ]
        }
      }
    ]
  })
  
  depends_on = [azurerm_linux_web_app.webapp]
}

#  Deploy code from a public GitHub repo (with Datadog sidecar)
resource "azurerm_app_service_source_control" "sourcecontrol" {
  app_id             = azurerm_linux_web_app.webapp.id
  repo_url           = "https://github.com/petems/Azure-App-Service-Terraform-Datadog-APM-Logs-Sandbox"
  branch             = "master"
  use_manual_integration = true
  use_mercurial      = false
}

# Outputs
output "app_service_name" {
  description = "Name of the App Service"
  value       = azurerm_linux_web_app.webapp.name
}

output "app_service_default_hostname" {
  description = "Default hostname of the App Service"
  value       = azurerm_linux_web_app.webapp.default_hostname
}

output "app_service_url" {
  description = "URL of the App Service"
  value       = "https://${azurerm_linux_web_app.webapp.default_hostname}"
}

output "resource_group_name" {
  description = "Name of the resource group"
  value       = data.azurerm_resource_group.rg.name
}