# Azure Deployment Guide - Million Luxury

> Complete guide to deploy the full-stack application to Microsoft Azure with CI/CD

## 📋 Table of Contents

- [Overview](#overview)
- [Azure Architecture](#azure-architecture)
- [Prerequisites](#prerequisites)
- [Azure Resources Setup](#azure-resources-setup)
- [Backend Deployment](#backend-deployment)
- [Frontend Deployment](#frontend-deployment)
- [Database Setup](#database-setup)
- [CI/CD with GitHub Actions](#cicd-with-github-actions)
- [Monitoring and Logging](#monitoring-and-logging)
- [Cost Estimation](#cost-estimation)
- [Troubleshooting](#troubleshooting)

---

## Overview

This guide will help you deploy:
- **Backend API** → Azure App Service (Linux)
- **Frontend** → Azure Static Web Apps
- **Database** → Azure Cosmos DB for MongoDB
- **CI/CD** → GitHub Actions
- **Monitoring** → Azure Application Insights

### Deployment Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     GitHub Repository                        │
│  (Source Code + GitHub Actions)                             │
└────────────┬──────────────────────────┬─────────────────────┘
             │                          │
    ┌────────▼────────┐        ┌───────▼────────┐
    │ GitHub Actions  │        │ GitHub Actions │
    │ (Backend CI/CD) │        │(Frontend CI/CD)│
    └────────┬────────┘        └───────┬────────┘
             │                          │
    ┌────────▼─────────────┐   ┌───────▼──────────────────┐
    │ Azure App Service    │   │ Azure Static Web Apps    │
    │ (.NET 9 API)         │◄──┤ (Next.js 16)             │
    │ - Linux Container    │   │ - CDN + Edge Functions   │
    │ - Auto-scaling       │   │ - Global distribution    │
    └─────────┬────────────┘   └──────────────────────────┘
              │
    ┌─────────▼────────────┐
    │ Azure Cosmos DB      │
    │ (MongoDB API)        │
    │ - Global distribution│
    │ - Auto-scaling       │
    └──────────────────────┘
              │
    ┌─────────▼────────────┐
    │ Application Insights │
    │ (Monitoring & Logs)  │
    └──────────────────────┘
```

---

## Azure Architecture

### Resources to be Created:

| Resource | Service | Purpose | Pricing Tier |
|----------|---------|---------|--------------|
| **Resource Group** | Container | Organize all resources | Free |
| **App Service Plan** | Compute | Host backend API | B1 Basic ($13/mo) |
| **App Service** | Web App | .NET 9 API | Included in plan |
| **Static Web App** | CDN/Hosting | Next.js frontend | Free tier |
| **Cosmos DB** | Database | MongoDB API | Serverless (pay per use) |
| **Application Insights** | Monitoring | Logs and metrics | Pay per GB |
| **Key Vault** | Secrets | Store connection strings | $0.03/10k ops |

**Estimated Monthly Cost:** $15-30 USD (depending on usage)

---

## Prerequisites

### Required Tools

Install these before starting:

```bash
# 1. Azure CLI
# macOS
brew install azure-cli

# Windows
# Download from: https://aka.ms/installazurecliwindows

# Linux
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# 2. Verify installation
az --version

# 3. Login to Azure
az login
# This will open a browser window for authentication
```

### Required Accounts

- ✅ **Azure Account** - [Create Free Account](https://azure.microsoft.com/free/) (includes $200 credit)
- ✅ **GitHub Account** - For repository and Actions
- ✅ **Domain** (Optional) - For custom domain

### Verify Prerequisites

```bash
# Check Azure CLI
az --version

# Check you're logged in
az account show

# List your subscriptions
az account list --output table

# Set active subscription (if you have multiple)
az account set --subscription "Your-Subscription-Name"
```

---

## Azure Resources Setup

### Step 1: Set Environment Variables

```bash
# Set these variables for easier commands
export RESOURCE_GROUP="million-luxury-rg"
export LOCATION="eastus"
export APP_SERVICE_PLAN="million-luxury-plan"
export BACKEND_APP_NAME="million-luxury-api"
export FRONTEND_APP_NAME="million-luxury-web"
export COSMOS_DB_NAME="million-luxury-db"
export COSMOS_DB_ACCOUNT="million-luxury-cosmos"
export APP_INSIGHTS_NAME="million-luxury-insights"
```

### Step 2: Create Resource Group

```bash
# Create resource group (container for all resources)
az group create \
  --name $RESOURCE_GROUP \
  --location $LOCATION

# Expected output:
# {
#   "id": "/subscriptions/.../resourceGroups/million-luxury-rg",
#   "location": "eastus",
#   "name": "million-luxury-rg",
#   "properties": {
#     "provisioningState": "Succeeded"
#   }
# }
```

### Step 3: Create Application Insights

```bash
# Create Application Insights for monitoring
az monitor app-insights component create \
  --app $APP_INSIGHTS_NAME \
  --location $LOCATION \
  --resource-group $RESOURCE_GROUP \
  --application-type web

# Get instrumentation key (save this!)
az monitor app-insights component show \
  --app $APP_INSIGHTS_NAME \
  --resource-group $RESOURCE_GROUP \
  --query instrumentationKey \
  --output tsv
```

---

## Backend Deployment

### Step 1: Create App Service Plan

```bash
# Create Linux-based App Service Plan (B1 tier)
az appservice plan create \
  --name $APP_SERVICE_PLAN \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --is-linux \
  --sku B1

# B1 includes:
# - 1.75 GB RAM
# - 1 Core
# - 10 GB storage
# - Perfect for development/staging
```

### Step 2: Create Web App for Backend

```bash
# Create Web App for .NET 9
az webapp create \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --plan $APP_SERVICE_PLAN \
  --runtime "DOTNET:9.0"

# Enable HTTPS only
az webapp update \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --https-only true
```

### Step 3: Configure Backend App Settings

```bash
# Configure app settings (environment variables)
az webapp config appsettings set \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --settings \
    ASPNETCORE_ENVIRONMENT="Production" \
    WEBSITE_RUN_FROM_PACKAGE="1" \
    AllowedHosts="*"

# Note: MongoDB connection string will be added after Cosmos DB creation
```

### Step 4: Create Backend Dockerfile

Create `backend/Dockerfile`:

```dockerfile
# Use .NET 9 SDK for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["MillionBackend.sln", "./"]
COPY ["src/MillionBackend.API/MillionBackend.API.csproj", "src/MillionBackend.API/"]
COPY ["src/MillionBackend.Application/MillionBackend.Application.csproj", "src/MillionBackend.Application/"]
COPY ["src/MillionBackend.Core/MillionBackend.Core.csproj", "src/MillionBackend.Core/"]
COPY ["src/MillionBackend.Infrastructure/MillionBackend.Infrastructure.csproj", "src/MillionBackend.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MillionBackend.sln"

# Copy all source code
COPY . .

# Build
WORKDIR "/src/src/MillionBackend.API"
RUN dotnet build "MillionBackend.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "MillionBackend.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MillionBackend.API.dll"]
```

---

## Frontend Deployment

### Step 1: Create Static Web App

```bash
# Create Static Web App (Free tier)
az staticwebapp create \
  --name $FRONTEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --sku Free

# Get deployment token (needed for GitHub Actions)
az staticwebapp secrets list \
  --name $FRONTEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --query properties.apiKey \
  --output tsv
```

### Step 2: Configure Frontend Environment

Create `frontend/.env.production`:

```env
# Production environment variables
NEXT_PUBLIC_API_URL=https://million-luxury-api.azurewebsites.net
```

### Step 3: Create staticwebapp.config.json

Create `frontend/staticwebapp.config.json`:

```json
{
  "routes": [
    {
      "route": "/api/*",
      "rewrite": "https://million-luxury-api.azurewebsites.net/api/*"
    }
  ],
  "navigationFallback": {
    "rewrite": "/index.html",
    "exclude": ["/images/*", "/favicon.ico"]
  },
  "responseOverrides": {
    "404": {
      "rewrite": "/not-found.html",
      "statusCode": 404
    }
  },
  "globalHeaders": {
    "content-security-policy": "default-src https: 'unsafe-eval' 'unsafe-inline'; object-src 'none'"
  }
}
```

---

## Database Setup

### Step 1: Create Cosmos DB Account

```bash
# Create Cosmos DB with MongoDB API
az cosmosdb create \
  --name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --locations regionName=$LOCATION failoverPriority=0 \
  --kind MongoDB \
  --server-version 4.2 \
  --default-consistency-level Session \
  --enable-automatic-failover false \
  --capabilities EnableServerless

# Note: Serverless mode is pay-per-use, no minimum cost
```

### Step 2: Create Database

```bash
# Create database
az cosmosdb mongodb database create \
  --account-name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --name MillionTestDB
```

### Step 3: Create Collections

```bash
# Create collections with indexes

# Properties collection
az cosmosdb mongodb collection create \
  --account-name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --database-name MillionTestDB \
  --name properties \
  --shard "idProperty"

# Owners collection
az cosmosdb mongodb collection create \
  --account-name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --database-name MillionTestDB \
  --name owners \
  --shard "idOwner"

# PropertyImages collection
az cosmosdb mongodb collection create \
  --account-name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --database-name MillionTestDB \
  --name propertyImages \
  --shard "idProperty"

# PropertyTraces collection
az cosmosdb mongodb collection create \
  --account-name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --database-name MillionTestDB \
  --name propertyTraces \
  --shard "idProperty"
```

### Step 4: Get Connection String

```bash
# Get MongoDB connection string
az cosmosdb keys list \
  --name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP \
  --type connection-strings \
  --query "connectionStrings[0].connectionString" \
  --output tsv

# Save this connection string!
# Format: mongodb://million-luxury-cosmos:password@million-luxury-cosmos.mongo.cosmos.azure.com:10255/?ssl=true
```

### Step 5: Update Backend Connection String

```bash
# Set MongoDB connection string in backend app
CONNECTION_STRING="<paste-connection-string-here>"

az webapp config appsettings set \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --settings \
    ConnectionStrings__MongoDB="$CONNECTION_STRING" \
    MongoDBSettings__DatabaseName="MillionTestDB"
```

### Step 6: Seed Production Database

You can seed the database using Azure Cloud Shell or your local machine:

```bash
# Option 1: Use mongosh (if you have it locally)
mongosh "$CONNECTION_STRING" < backend/seed-data-heavy.js

# Option 2: Use Azure Data Migration Service
# (GUI-based, good for large datasets)
```

---

## CI/CD with GitHub Actions

### Step 1: Get Deployment Credentials

```bash
# Get backend publish profile
az webapp deployment list-publishing-profiles \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --xml > backend-publish-profile.xml

# View the content (you'll add this to GitHub secrets)
cat backend-publish-profile.xml

# Get frontend deployment token (from earlier)
az staticwebapp secrets list \
  --name $FRONTEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --query properties.apiKey \
  --output tsv
```

### Step 2: Add GitHub Secrets

Go to your GitHub repository → Settings → Secrets and variables → Actions → New repository secret

Add these secrets:

| Secret Name | Value | Description |
|-------------|-------|-------------|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Contents of `backend-publish-profile.xml` | Backend deployment |
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Frontend deployment token | Frontend deployment |
| `AZURE_RESOURCE_GROUP` | `million-luxury-rg` | Resource group name |
| `AZURE_WEBAPP_NAME` | `million-luxury-api` | Backend app name |

### Step 3: Create Backend CI/CD Workflow

Create `.github/workflows/backend-deploy.yml`:

```yaml
name: Backend - Deploy to Azure

on:
  push:
    branches:
      - main
    paths:
      - 'backend/**'
  workflow_dispatch:

env:
  DOTNET_VERSION: '9.0.x'
  AZURE_WEBAPP_NAME: million-luxury-api
  AZURE_WEBAPP_PACKAGE_PATH: 'backend/src/MillionBackend.API'

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}

    - name: Restore dependencies
      run: dotnet restore backend/MillionBackend.sln

    - name: Build
      run: dotnet build backend/MillionBackend.sln --configuration Release --no-restore

    - name: Run tests
      run: dotnet test backend/MillionBackend.sln --configuration Release --no-build --verbosity normal

    - name: Publish
      run: dotnet publish ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}/MillionBackend.API.csproj --configuration Release --output ${{env.DOTNET_ROOT}}/api

    - name: Upload artifact
      uses: actions/upload-artifact@v4
      with:
        name: backend-app
        path: ${{env.DOTNET_ROOT}}/api

  deploy:
    runs-on: ubuntu-latest
    needs: build-and-test
    environment:
      name: 'Production'
      url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}

    steps:
    - name: Download artifact
      uses: actions/download-artifact@v4
      with:
        name: backend-app

    - name: Deploy to Azure Web App
      id: deploy-to-webapp
      uses: azure/webapps-deploy@v3
      with:
        app-name: ${{ env.AZURE_WEBAPP_NAME }}
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: .
```

### Step 4: Create Frontend CI/CD Workflow

Create `.github/workflows/frontend-deploy.yml`:

```yaml
name: Frontend - Deploy to Azure Static Web Apps

on:
  push:
    branches:
      - main
    paths:
      - 'frontend/**'
  pull_request:
    types: [opened, synchronize, reopened, closed]
    branches:
      - main
    paths:
      - 'frontend/**'
  workflow_dispatch:

jobs:
  build_and_deploy:
    if: github.event_name == 'push' || (github.event_name == 'pull_request' && github.event.action != 'closed')
    runs-on: ubuntu-latest
    name: Build and Deploy

    steps:
    - name: Checkout code
      uses: actions/checkout@v4
      with:
        submodules: true

    - name: Setup Node.js
      uses: actions/setup-node@v4
      with:
        node-version: '20'
        cache: 'npm'
        cache-dependency-path: frontend/package-lock.json

    - name: Install dependencies
      run: |
        cd frontend
        npm ci

    - name: Build application
      run: |
        cd frontend
        npm run build
      env:
        NEXT_PUBLIC_API_URL: https://million-luxury-api.azurewebsites.net

    - name: Deploy to Azure Static Web Apps
      uses: Azure/static-web-apps-deploy@v1
      with:
        azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
        repo_token: ${{ secrets.GITHUB_TOKEN }}
        action: 'upload'
        app_location: 'frontend'
        output_location: '.next'
        skip_app_build: true

  close_pull_request:
    if: github.event_name == 'pull_request' && github.event.action == 'closed'
    runs-on: ubuntu-latest
    name: Close Pull Request

    steps:
    - name: Close Pull Request
      uses: Azure/static-web-apps-deploy@v1
      with:
        azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
        action: 'close'
```

### Step 5: Create Full Deployment Workflow

Create `.github/workflows/deploy-all.yml`:

```yaml
name: Deploy Full Stack to Azure

on:
  workflow_dispatch:
  push:
    branches:
      - main

jobs:
  deploy-backend:
    uses: ./.github/workflows/backend-deploy.yml
    secrets: inherit

  deploy-frontend:
    uses: ./.github/workflows/frontend-deploy.yml
    secrets: inherit
    needs: deploy-backend
```

---

## Monitoring and Logging

### Step 1: Configure Application Insights

```bash
# Get Application Insights connection string
INSTRUMENTATION_KEY=$(az monitor app-insights component show \
  --app $APP_INSIGHTS_NAME \
  --resource-group $RESOURCE_GROUP \
  --query instrumentationKey \
  --output tsv)

# Add to backend app settings
az webapp config appsettings set \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --settings \
    APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=$INSTRUMENTATION_KEY"
```

### Step 2: View Logs

```bash
# Stream backend logs
az webapp log tail \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP

# View metrics in Azure Portal
# Go to: Application Insights → Metrics
```

### Step 3: Set Up Alerts

```bash
# Create alert for high error rate
az monitor metrics alert create \
  --name "High-Error-Rate" \
  --resource-group $RESOURCE_GROUP \
  --scopes "/subscriptions/{subscription-id}/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.Web/sites/$BACKEND_APP_NAME" \
  --condition "avg Percentage of 5xx Errors > 5" \
  --description "Alert when 5xx error rate exceeds 5%"
```

---

## Cost Estimation

### Monthly Costs (Estimated)

| Service | Tier | Cost | Notes |
|---------|------|------|-------|
| App Service Plan | B1 Basic | $13.14/mo | 1 core, 1.75 GB RAM |
| Static Web App | Free | $0 | 100 GB bandwidth/mo |
| Cosmos DB | Serverless | $5-15/mo | Pay per request |
| Application Insights | Pay-as-go | $2-5/mo | First 5 GB free |
| **Total** | | **$20-33/mo** | Varies by usage |

### Cost Optimization Tips:

1. **Use Serverless Cosmos DB** - Only pay for what you use
2. **Scale App Service** - Use B1 for dev, scale to S1+ for production
3. **CDN Caching** - Reduce bandwidth costs
4. **Reserved Instances** - Save up to 72% with 1-3 year commitment

---

## Troubleshooting

### Issue 1: Backend App Won't Start

**Check logs:**
```bash
az webapp log tail \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP
```

**Common causes:**
- Missing connection string
- Wrong .NET version
- Port binding issues (must use PORT env var)

### Issue 2: Frontend Can't Connect to Backend

**Check CORS:**
```bash
# Add frontend URL to CORS allowed origins
az webapp cors add \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --allowed-origins "https://$FRONTEND_APP_NAME.azurestaticapps.net"
```

### Issue 3: Database Connection Timeout

**Check Cosmos DB:**
```bash
# Verify Cosmos DB is running
az cosmosdb show \
  --name $COSMOS_DB_ACCOUNT \
  --resource-group $RESOURCE_GROUP
```

**Common causes:**
- Firewall rules blocking connection
- Wrong connection string
- SSL certificate issues

---

## Next Steps

After successful deployment:

1. **Configure Custom Domain:**
   ```bash
   az staticwebapp hostname set \
     --name $FRONTEND_APP_NAME \
     --resource-group $RESOURCE_GROUP \
     --hostname www.yourdomain.com
   ```

2. **Enable Auto-scaling:**
   ```bash
   az monitor autoscale create \
     --resource-group $RESOURCE_GROUP \
     --resource $BACKEND_APP_NAME \
     --resource-type Microsoft.Web/serverFarms \
     --name autoscale-$BACKEND_APP_NAME \
     --min-count 1 \
     --max-count 3 \
     --count 1
   ```

3. **Set Up Staging Slots:**
   ```bash
   az webapp deployment slot create \
     --name $BACKEND_APP_NAME \
     --resource-group $RESOURCE_GROUP \
     --slot staging
   ```

4. **Configure Backup:**
   ```bash
   az webapp config backup create \
     --resource-group $RESOURCE_GROUP \
     --webapp-name $BACKEND_APP_NAME \
     --backup-name initial-backup \
     --container-url "<storage-container-url>"
   ```

---

## Useful Commands

```bash
# View all resources
az resource list \
  --resource-group $RESOURCE_GROUP \
  --output table

# Delete all resources (careful!)
az group delete \
  --name $RESOURCE_GROUP \
  --yes --no-wait

# View costs
az consumption usage list \
  --start-date 2024-01-01 \
  --end-date 2024-01-31

# Restart backend
az webapp restart \
  --name $BACKEND_APP_NAME \
  --resource-group $RESOURCE_GROUP
```

---

**Your full-stack application is now deployed to Azure with CI/CD! 🎉**
