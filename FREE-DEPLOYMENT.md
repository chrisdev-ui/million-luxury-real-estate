# Free Deployment Guide - Million Luxury

> Deploy your full-stack application for **FREE** using Vercel, Railway, and MongoDB Atlas

## 🎯 Overview

This guide will help you deploy:
- **Frontend**: Vercel (Free tier)
- **Backend**: Railway.app (Free $5/month credit)
- **Database**: MongoDB Atlas (Free M0 tier - 512MB)

**Total Cost: $0/month** 🎉

---

## 📋 Prerequisites

1. **GitHub Account** - [Sign up](https://github.com)
2. **Vercel Account** - [Sign up](https://vercel.com)
3. **Railway Account** - [Sign up](https://railway.app)
4. **MongoDB Atlas Account** - [Sign up](https://www.mongodb.com/cloud/atlas/register)

---

## Step 1: Setup MongoDB Atlas (5 minutes)

### 1.1 Create Free Cluster

1. Go to [MongoDB Atlas](https://www.mongodb.com/cloud/atlas/register)
2. Sign up / Log in
3. Click **"Build a Database"**
4. Choose **M0 FREE** tier
5. Select **Provider**: AWS
6. Select **Region**: Closest to you (e.g., us-east-1)
7. Cluster Name: `million-luxury-cluster`
8. Click **"Create"**

### 1.2 Create Database User

1. In the **Security** section, click **"Database Access"**
2. Click **"Add New Database User"**
3. Choose **"Password"** authentication
4. Username: `millionluxury`
5. Password: Generate a secure password (save it!)
6. Database User Privileges: **"Read and write to any database"**
7. Click **"Add User"**

### 1.3 Configure Network Access

1. In the **Security** section, click **"Network Access"**
2. Click **"Add IP Address"**
3. Click **"Allow Access from Anywhere"** (0.0.0.0/0)
   - This is safe for development; Railway/Vercel use dynamic IPs
4. Click **"Confirm"**

### 1.4 Get Connection String

1. Click **"Database"** in the left sidebar
2. Click **"Connect"** on your cluster
3. Choose **"Connect your application"**
4. Driver: **Node.js** (or any, doesn't matter)
5. Copy the connection string:
   ```
   mongodb+srv://millionluxury:<password>@million-luxury-cluster.xxxxx.mongodb.net/?retryWrites=true&w=majority
   ```
6. Replace `<password>` with your actual password
7. Add database name before the `?`:
   ```
   mongodb+srv://millionluxury:YOUR_PASSWORD@million-luxury-cluster.xxxxx.mongodb.net/MillionTestDB?retryWrites=true&w=majority
   ```

**Save this connection string!** You'll need it for Railway.

### 1.5 Seed Database (Optional)

If you want sample data:

```bash
# In your local terminal
cd backend

# Set connection string
export MONGODB_CONNECTION_STRING="mongodb+srv://millionluxury:YOUR_PASSWORD@million-luxury-cluster.xxxxx.mongodb.net/MillionTestDB?retryWrites=true&w=majority"

# Use mongosh to import data (if you have seed data)
# Or run your backend locally once to create collections
```

---

## Step 2: Deploy Backend to Railway (5 minutes)

### 2.1 Prepare Backend

Railway will automatically detect your .NET app, but we need to ensure it's configured correctly.

**No changes needed!** Your backend is already Railway-ready.

### 2.2 Deploy to Railway

1. Go to [railway.app](https://railway.app)
2. Sign in with GitHub
3. Click **"New Project"**
4. Choose **"Deploy from GitHub repo"**
5. Select your repository: `MillionTest`
6. Railway will detect your backend automatically

### 2.3 Configure Root Directory

1. After project creation, click on your service
2. Go to **"Settings"**
3. Set **Root Directory**: `backend/src/MillionBackend.API`
4. Click **"Save"**

### 2.4 Add Environment Variables

1. Go to **"Variables"** tab
2. Add these variables:

```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__MongoDB=mongodb+srv://millionluxury:YOUR_PASSWORD@million-luxury-cluster.xxxxx.mongodb.net/MillionTestDB?retryWrites=true&w=majority
MongoDBSettings__DatabaseName=MillionTestDB
AllowedHosts=*
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

**Important**: Railway automatically provides `$PORT` variable.

### 2.5 Get Backend URL

1. Go to **"Settings"** tab
2. Scroll to **"Networking"**
3. Click **"Generate Domain"**
4. Copy the URL: `https://your-app-name.up.railway.app`

**Save this URL!** You'll need it for Vercel.

### 2.6 Verify Deployment

1. Wait for deployment to complete (~2-3 minutes)
2. Visit: `https://your-app-name.up.railway.app/swagger`
3. You should see the Swagger API documentation

---

## Step 3: Deploy Frontend to Vercel (3 minutes)

### 3.1 Update Environment Variable

Update your frontend production environment variable:

**File**: `frontend/.env.production`

```env
NEXT_PUBLIC_API_URL=https://your-app-name.up.railway.app
NODE_ENV=production
```

Replace `your-app-name.up.railway.app` with your Railway backend URL.

### 3.2 Deploy to Vercel

1. Go to [vercel.com](https://vercel.com)
2. Sign in with GitHub
3. Click **"Add New..."** → **"Project"**
4. Import your GitHub repository: `MillionTest`
5. Configure project:
   - **Framework Preset**: Next.js
   - **Root Directory**: `frontend`
   - **Build Command**: `npm run build` (auto-detected)
   - **Output Directory**: `.next` (auto-detected)

### 3.3 Configure Environment Variables

1. In the deployment setup, expand **"Environment Variables"**
2. Add:
   - **Key**: `NEXT_PUBLIC_API_URL`
   - **Value**: `https://your-app-name.up.railway.app` (your Railway URL)

### 3.4 Deploy

1. Click **"Deploy"**
2. Wait 2-3 minutes for build and deployment
3. Vercel will provide your frontend URL: `https://your-project.vercel.app`

### 3.5 Update CORS in Backend

Your backend needs to allow requests from your Vercel domain.

**Option A: Update via Railway Dashboard**

1. Go to Railway dashboard
2. Click your backend service
3. Go to **"Variables"**
4. Add/Update:
   ```env
   CorsOrigins=https://your-project.vercel.app
   ```

**Option B: Update in Code (Better)**

Update `backend/src/MillionBackend.API/Program.cs`:

```csharp
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var allowedOrigins = builder.Configuration["CorsOrigins"]?.Split(',')
            ?? new[] { "http://localhost:3000" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ... later in the file
app.UseCors("AllowFrontend");
```

Then set `CorsOrigins` in Railway variables:
```env
CorsOrigins=https://your-project.vercel.app,http://localhost:3000
```

---

## Step 4: Verify Full Stack (2 minutes)

### 4.1 Test Frontend

1. Visit your Vercel URL: `https://your-project.vercel.app`
2. You should see the property listing page
3. Click on a property to see details

### 4.2 Test Backend

1. Visit: `https://your-app-name.up.railway.app/swagger`
2. Test an endpoint (e.g., GET `/api/properties`)
3. Verify data is returned from MongoDB Atlas

### 4.3 Test Integration

1. From your frontend, try:
   - Viewing property list
   - Filtering properties
   - Viewing property details
   - Creating a new property (if applicable)

---

## 🎉 Success!

Your application is now deployed for **FREE**:

- ✅ **Frontend**: https://your-project.vercel.app
- ✅ **Backend**: https://your-app-name.up.railway.app
- ✅ **Database**: MongoDB Atlas M0 (512MB)
- ✅ **Cost**: $0/month

---

## 🔄 Automatic Deployments

Both Vercel and Railway support automatic deployments from GitHub:

### Vercel (Already Configured)
- Push to `main` branch → Frontend auto-deploys
- Pull requests → Preview deployments

### Railway (Already Configured)
- Push to `main` branch → Backend auto-deploys
- View logs in Railway dashboard

---

## 📊 Free Tier Limits

### Vercel Free Tier
- ✅ 100GB bandwidth/month
- ✅ Unlimited websites
- ✅ Automatic HTTPS
- ✅ Serverless functions (100GB-hours)

### Railway Free Tier
- ✅ $5 credit/month (usually enough for small apps)
- ✅ ~500 hours of uptime/month
- ✅ Automatic HTTPS
- ⚠️ App sleeps after 15 minutes of inactivity (restarts on request)

### MongoDB Atlas M0
- ✅ 512MB storage
- ✅ Shared cluster
- ✅ Unlimited connections
- ⚠️ Limited to one M0 cluster per project

---

## 🔧 Common Tasks

### Update Backend Code

```bash
git add backend/
git commit -m "feat: update backend"
git push origin main
```

Railway will automatically rebuild and deploy.

### Update Frontend Code

```bash
git add frontend/
git commit -m "feat: update frontend"
git push origin main
```

Vercel will automatically rebuild and deploy.

### View Backend Logs

1. Go to Railway dashboard
2. Click your service
3. Go to **"Deployments"**
4. Click on the latest deployment
5. View logs in real-time

### View Frontend Logs

1. Go to Vercel dashboard
2. Click your project
3. Go to **"Deployments"**
4. Click on the latest deployment
5. View **"Functions"** logs

### Update Environment Variables

**Backend (Railway)**:
1. Go to Railway dashboard → Variables
2. Add/Update variable
3. Service will automatically redeploy

**Frontend (Vercel)**:
1. Go to Vercel dashboard → Settings → Environment Variables
2. Add/Update variable
3. Manually trigger redeploy or push a commit

---

## 🐛 Troubleshooting

### Backend not connecting to MongoDB

**Error**: `MongoServerError: Authentication failed`

**Solution**:
1. Verify MongoDB connection string is correct
2. Check password has no special characters (or URL-encode them)
3. Verify database user has correct permissions
4. Check Network Access allows 0.0.0.0/0

### Frontend can't reach Backend

**Error**: `CORS error` or `Failed to fetch`

**Solution**:
1. Verify `NEXT_PUBLIC_API_URL` in Vercel matches Railway URL
2. Check CORS configuration in backend allows Vercel domain
3. Verify Railway backend is running (check logs)

### Railway app sleeping

**Issue**: First request takes 10-20 seconds

**Explanation**: Railway free tier puts apps to sleep after 15 minutes of inactivity.

**Solutions**:
- Upgrade to Railway Pro ($5/month, no sleep)
- Use a cron job to ping your backend every 10 minutes
- Accept the cold start delay

### Vercel build fails

**Error**: Build error in Vercel

**Solution**:
1. Check build logs in Vercel dashboard
2. Verify `NEXT_PUBLIC_API_URL` is set
3. Try building locally: `cd frontend && npm run build`
4. Check for TypeScript errors

### MongoDB storage limit

**Error**: `Quota exceeded`

**Solution**:
- M0 tier has 512MB limit
- Optimize images (store URLs, not binary data)
- Clean up old data
- Upgrade to M2 tier ($9/month) if needed

---

## 💰 Cost Comparison

| Service | Free Tier | If You Exceed |
|---------|-----------|---------------|
| **Vercel** | 100GB bandwidth/month | $20/month for Pro |
| **Railway** | $5 credit/month | $0.000231/minute after credit |
| **MongoDB Atlas** | 512MB storage | $9/month for M2 tier |

**Estimated Cost if Free Tier Exceeded**: ~$30-40/month (still cheaper than Azure!)

---

## 🚀 Optional Enhancements

### 1. Add Custom Domain (Vercel)

1. Buy a domain (e.g., from Namecheap, GoDaddy)
2. In Vercel dashboard → Settings → Domains
3. Add your domain
4. Update DNS records as instructed
5. Vercel handles HTTPS automatically

### 2. Set up Railway Cron (Keep Backend Awake)

Create a GitHub Action to ping your backend every 10 minutes:

**File**: `.github/workflows/keep-alive.yml`

```yaml
name: Keep Backend Alive

on:
  schedule:
    - cron: '*/10 * * * *'  # Every 10 minutes
  workflow_dispatch:

jobs:
  ping:
    runs-on: ubuntu-latest
    steps:
      - name: Ping Backend
        run: curl -f https://your-app-name.up.railway.app/health || true
```

### 3. Set up Monitoring

Use free monitoring tools:
- **Uptime Robot** - Free monitoring (50 monitors)
- **Better Stack** - Free tier for logs and monitoring
- **Sentry** - Free error tracking (5K events/month)

---

## 📚 Additional Resources

- **Vercel Docs**: https://vercel.com/docs
- **Railway Docs**: https://docs.railway.app
- **MongoDB Atlas Docs**: https://www.mongodb.com/docs/atlas/
- **Next.js Deployment**: https://nextjs.org/docs/deployment

---

## ✅ Deployment Checklist

- [ ] MongoDB Atlas cluster created (M0 Free)
- [ ] Database user created with password
- [ ] Network access configured (0.0.0.0/0)
- [ ] Connection string saved
- [ ] Backend deployed to Railway
- [ ] Railway environment variables configured
- [ ] Railway domain generated
- [ ] Frontend environment variable updated with Railway URL
- [ ] Frontend deployed to Vercel
- [ ] Vercel environment variable configured
- [ ] CORS configured in backend for Vercel domain
- [ ] Full stack tested and working
- [ ] GitHub auto-deployments working

---

## 🎊 You're Done!

Your Million Luxury application is now running completely **FREE** with:

- ✅ Production-ready hosting
- ✅ Automatic HTTPS
- ✅ CI/CD from GitHub
- ✅ Managed database
- ✅ Global CDN (Vercel)
- ✅ Auto-scaling

**Happy deploying! 🚀**

---

**Need help?** Open an issue or check the platform-specific documentation linked above.
