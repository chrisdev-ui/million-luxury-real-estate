# Azure Deployment - Quick Start Guide

> Get your Million Luxury application deployed to Azure in minutes!

## 🚀 Quick Start (5 Minutes)

### Prerequisites

1. **Azure Account** - [Create free account](https://azure.microsoft.com/free/) ($200 credit)
2. **Azure CLI** - Install and login
3. **GitHub Account** - For CI/CD

### Option 1: Automated Deployment (Recommended)

```bash
# 1. Login to Azure
az login

# 2. Run deployment script
./deploy-azure.sh

# 3. Follow on-screen instructions
# Script will create all Azure resources automatically
```

**That's it!** ✨ Your infrastructure is ready in ~15 minutes.

### Option 2: Manual Deployment

Follow the detailed guide: [`docs/deployment/azure-deployment-guide.md`](docs/deployment/azure-deployment-guide.md)

---

## 📋 What Gets Created

| Resource | Purpose | Cost |
|----------|---------|------|
| **Resource Group** | Container for all resources | Free |
| **App Service** | Backend API (.NET 9) | ~$13/mo |
| **Static Web App** | Frontend (Next.js 16) | Free |
| **Cosmos DB** | MongoDB database | ~$5-15/mo |
| **Application Insights** | Monitoring & logs | ~$2-5/mo |

**Total: ~$20-30/month**

---

## 🔐 GitHub Secrets Setup

After running the deployment script, add these secrets to your GitHub repository:

### Get Backend Publish Profile

```bash
# Download publish profile
az webapp deployment list-publishing-profiles \
  --name million-luxury-api \
  --resource-group million-luxury-rg \
  --xml > publish-profile.xml

# Copy the entire content
cat publish-profile.xml
```

### Get Frontend Deployment Token

```bash
# Get Static Web App token
az staticwebapp secrets list \
  --name million-luxury-web \
  --resource-group million-luxury-rg \
  --query properties.apiKey \
  --output tsv
```

### Add to GitHub

1. Go to: `https://github.com/YOUR-USERNAME/YOUR-REPO/settings/secrets/actions`
2. Click "New repository secret"
3. Add these secrets:

| Secret Name | Value | Source |
|-------------|-------|--------|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Content of `publish-profile.xml` | Backend profile |
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Token from command above | Frontend token |

---

## 🎯 Deploy Your Code

### First Deployment

```bash
# 1. Commit all code
git add .
git commit -m "feat: add Azure deployment configuration"

# 2. Push to GitHub
git push origin main

# 3. GitHub Actions will automatically deploy!
# Watch the progress at: https://github.com/YOUR-REPO/actions
```

### Check Deployment Status

```bash
# Backend logs
az webapp log tail \
  --name million-luxury-api \
  --resource-group million-luxury-rg

# Frontend status
az staticwebapp show \
  --name million-luxury-web \
  --resource-group million-luxury-rg
```

---

## 🌐 Access Your Application

After deployment completes (5-10 minutes):

- **Frontend**: https://million-luxury-web.azurestaticapps.net
- **Backend API**: https://million-luxury-api.azurewebsites.net
- **Swagger Docs**: https://million-luxury-api.azurewebsites.net/swagger

---

## 🔧 Common Tasks

### View All Resources

```bash
az resource list \
  --resource-group million-luxury-rg \
  --output table
```

### View Costs

```bash
az consumption usage list \
  --start-date 2024-01-01 \
  --end-date 2024-01-31
```

### Restart Backend

```bash
az webapp restart \
  --name million-luxury-api \
  --resource-group million-luxury-rg
```

### View Logs

```bash
# Backend logs (live)
az webapp log tail \
  --name million-luxury-api \
  --resource-group million-luxury-rg

# Application Insights
az monitor app-insights component show \
  --app million-luxury-insights \
  --resource-group million-luxury-rg
```

---

## 🐛 Troubleshooting

### Backend Won't Start

```bash
# Check logs
az webapp log tail \
  --name million-luxury-api \
  --resource-group million-luxury-rg

# Common issues:
# - Missing connection string
# - Wrong .NET version
# - Port configuration (must use PORT env var)
```

### Frontend Can't Connect to Backend

```bash
# Check CORS settings
az webapp cors show \
  --name million-luxury-api \
  --resource-group million-luxury-rg

# Add frontend origin
az webapp cors add \
  --name million-luxury-api \
  --resource-group million-luxury-rg \
  --allowed-origins "https://million-luxury-web.azurestaticapps.net"
```

### Database Connection Issues

```bash
# Verify Cosmos DB
az cosmosdb show \
  --name million-luxury-cosmos \
  --resource-group million-luxury-rg

# Get connection string again
az cosmosdb keys list \
  --name million-luxury-cosmos \
  --resource-group million-luxury-rg \
  --type connection-strings
```

---

## 🗑️ Clean Up (Delete Everything)

**⚠️ WARNING: This deletes all resources permanently!**

```bash
# Delete resource group (and all resources in it)
az group delete \
  --name million-luxury-rg \
  --yes --no-wait

# Verify deletion
az group exists --name million-luxury-rg
# Should return: false
```

---

## 📚 Additional Resources

- **Full Documentation**: [`docs/deployment/azure-deployment-guide.md`](docs/deployment/azure-deployment-guide.md)
- **Azure Portal**: https://portal.azure.com
- **GitHub Actions**: https://github.com/YOUR-REPO/actions
- **Azure Pricing Calculator**: https://azure.microsoft.com/pricing/calculator/

---

## 💡 Pro Tips

### 1. Enable Auto-scaling

```bash
az monitor autoscale create \
  --resource-group million-luxury-rg \
  --resource million-luxury-api \
  --resource-type Microsoft.Web/serverFarms \
  --name autoscale-million-luxury-api \
  --min-count 1 \
  --max-count 3 \
  --count 1
```

### 2. Add Custom Domain

```bash
# Frontend
az staticwebapp hostname set \
  --name million-luxury-web \
  --resource-group million-luxury-rg \
  --hostname www.yourdomain.com

# Backend
az webapp config hostname add \
  --webapp-name million-luxury-api \
  --resource-group million-luxury-rg \
  --hostname api.yourdomain.com
```

### 3. Enable Staging Slot

```bash
az webapp deployment slot create \
  --name million-luxury-api \
  --resource-group million-luxury-rg \
  --slot staging
```

---

## ✅ Next Steps

After successful deployment:

1. ✅ Configure custom domain
2. ✅ Set up monitoring alerts
3. ✅ Enable auto-scaling
4. ✅ Configure backup
5. ✅ Add SSL certificate

---

## 🎉 You're Done!

Your full-stack application is now running on Azure with:

- ✅ Automated CI/CD with GitHub Actions
- ✅ Global CDN for frontend
- ✅ Scalable backend API
- ✅ Managed database
- ✅ Monitoring and logging
- ✅ HTTPS by default

**Happy deploying! 🚀**

---

**Need help?** Check the [full deployment guide](docs/deployment/azure-deployment-guide.md) or open an issue.
