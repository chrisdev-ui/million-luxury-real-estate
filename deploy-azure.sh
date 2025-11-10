#!/bin/bash

# Million Luxury - Azure Deployment Script
# This script automates the deployment of the full-stack application to Azure

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
RESOURCE_GROUP="million-luxury-rg"
LOCATION="eastus"
APP_SERVICE_PLAN="million-luxury-plan"
BACKEND_APP_NAME="million-luxury-api"
FRONTEND_APP_NAME="million-luxury-web"
COSMOS_DB_ACCOUNT="million-luxury-cosmos"
APP_INSIGHTS_NAME="million-luxury-insights"

# Helper functions
print_header() {
    echo -e "\n${BLUE}========================================${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}========================================${NC}\n"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

# Check prerequisites
check_prerequisites() {
    print_header "Checking Prerequisites"

    # Check Azure CLI
    if ! command -v az &> /dev/null; then
        print_error "Azure CLI is not installed"
        echo "Install from: https://docs.microsoft.com/cli/azure/install-azure-cli"
        exit 1
    fi
    print_success "Azure CLI is installed"

    # Check if logged in
    if ! az account show &> /dev/null; then
        print_error "Not logged in to Azure"
        echo "Run: az login"
        exit 1
    fi
    print_success "Logged in to Azure"

    # Display account info
    ACCOUNT_NAME=$(az account show --query name -o tsv)
    print_info "Using subscription: $ACCOUNT_NAME"
}

# Create resource group
create_resource_group() {
    print_header "Creating Resource Group"

    if az group exists --name $RESOURCE_GROUP | grep -q "true"; then
        print_warning "Resource group $RESOURCE_GROUP already exists"
    else
        az group create \
            --name $RESOURCE_GROUP \
            --location $LOCATION \
            --output none
        print_success "Resource group created: $RESOURCE_GROUP"
    fi
}

# Create Application Insights
create_app_insights() {
    print_header "Creating Application Insights"

    az monitor app-insights component create \
        --app $APP_INSIGHTS_NAME \
        --location $LOCATION \
        --resource-group $RESOURCE_GROUP \
        --application-type web \
        --output none

    INSTRUMENTATION_KEY=$(az monitor app-insights component show \
        --app $APP_INSIGHTS_NAME \
        --resource-group $RESOURCE_GROUP \
        --query instrumentationKey \
        --output tsv)

    print_success "Application Insights created"
    print_info "Instrumentation Key: $INSTRUMENTATION_KEY"
}

# Create Cosmos DB
create_cosmos_db() {
    print_header "Creating Cosmos DB"

    print_info "This may take 5-10 minutes..."

    az cosmosdb create \
        --name $COSMOS_DB_ACCOUNT \
        --resource-group $RESOURCE_GROUP \
        --locations regionName=$LOCATION failoverPriority=0 \
        --kind MongoDB \
        --server-version 4.2 \
        --default-consistency-level Session \
        --enable-automatic-failover false \
        --capabilities EnableServerless \
        --output none

    print_success "Cosmos DB account created"

    # Create database
    az cosmosdb mongodb database create \
        --account-name $COSMOS_DB_ACCOUNT \
        --resource-group $RESOURCE_GROUP \
        --name MillionTestDB \
        --output none

    print_success "Database created: MillionTestDB"

    # Get connection string
    CONNECTION_STRING=$(az cosmosdb keys list \
        --name $COSMOS_DB_ACCOUNT \
        --resource-group $RESOURCE_GROUP \
        --type connection-strings \
        --query "connectionStrings[0].connectionString" \
        --output tsv)

    print_info "Connection string retrieved"
}

# Create App Service Plan and Web App
create_backend() {
    print_header "Creating Backend App Service"

    # Create App Service Plan
    az appservice plan create \
        --name $APP_SERVICE_PLAN \
        --resource-group $RESOURCE_GROUP \
        --location $LOCATION \
        --is-linux \
        --sku B1 \
        --output none

    print_success "App Service Plan created: $APP_SERVICE_PLAN"

    # Create Web App
    az webapp create \
        --name $BACKEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --plan $APP_SERVICE_PLAN \
        --runtime "DOTNET:9.0" \
        --output none

    print_success "Web App created: $BACKEND_APP_NAME"

    # Configure Web App
    az webapp update \
        --name $BACKEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --https-only true \
        --output none

    # Set app settings
    az webapp config appsettings set \
        --name $BACKEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --settings \
            ASPNETCORE_ENVIRONMENT="Production" \
            WEBSITE_RUN_FROM_PACKAGE="1" \
            AllowedHosts="*" \
            ConnectionStrings__MongoDB="$CONNECTION_STRING" \
            MongoDBSettings__DatabaseName="MillionTestDB" \
            APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=$INSTRUMENTATION_KEY" \
        --output none

    print_success "Backend configured"

    BACKEND_URL="https://$BACKEND_APP_NAME.azurewebsites.net"
    print_info "Backend URL: $BACKEND_URL"
}

# Create Static Web App
create_frontend() {
    print_header "Creating Frontend Static Web App"

    az staticwebapp create \
        --name $FRONTEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --location $LOCATION \
        --sku Free \
        --output none

    print_success "Static Web App created: $FRONTEND_APP_NAME"

    # Get deployment token
    DEPLOYMENT_TOKEN=$(az staticwebapp secrets list \
        --name $FRONTEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --query properties.apiKey \
        --output tsv)

    FRONTEND_URL="https://$FRONTEND_APP_NAME.azurestaticapps.net"
    print_info "Frontend URL: $FRONTEND_URL"
    print_info "Deployment Token: $DEPLOYMENT_TOKEN"
}

# Configure CORS
configure_cors() {
    print_header "Configuring CORS"

    az webapp cors add \
        --name $BACKEND_APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --allowed-origins "https://$FRONTEND_APP_NAME.azurestaticapps.net" \
        --output none

    print_success "CORS configured"
}

# Display summary
display_summary() {
    print_header "Deployment Summary"

    echo -e "${GREEN}✅ All Azure resources created successfully!${NC}\n"

    echo "📋 Resource Details:"
    echo "  Resource Group: $RESOURCE_GROUP"
    echo "  Location: $LOCATION"
    echo ""
    echo "🌐 URLs:"
    echo "  Backend API: https://$BACKEND_APP_NAME.azurewebsites.net"
    echo "  Frontend App: https://$FRONTEND_APP_NAME.azurestaticapps.net"
    echo "  Swagger Docs: https://$BACKEND_APP_NAME.azurewebsites.net/swagger"
    echo ""
    echo "📊 Monitoring:"
    echo "  Application Insights: $APP_INSIGHTS_NAME"
    echo ""
    echo "🔐 GitHub Secrets to Add:"
    echo "  AZURE_WEBAPP_PUBLISH_PROFILE: (Get from Azure Portal)"
    echo "  AZURE_STATIC_WEB_APPS_API_TOKEN: $DEPLOYMENT_TOKEN"
    echo ""
    echo "📝 Next Steps:"
    echo "  1. Add GitHub secrets (see above)"
    echo "  2. Push code to trigger CI/CD"
    echo "  3. Visit your deployed application"
    echo ""
    echo "💰 Estimated Monthly Cost: \$20-30 USD"
    echo ""
    echo "📚 Documentation: docs/deployment/azure-deployment-guide.md"
}

# Main execution
main() {
    print_header "Million Luxury - Azure Deployment"
    print_info "Starting deployment to Azure..."

    check_prerequisites
    create_resource_group
    create_app_insights
    create_cosmos_db
    create_backend
    create_frontend
    configure_cors
    display_summary

    print_success "Deployment completed successfully! 🎉"
}

# Run main function
main "$@"
