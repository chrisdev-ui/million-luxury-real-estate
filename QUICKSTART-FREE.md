# Quick Start - Free Deployment

> Deploy Million Luxury for **FREE** in 15 minutes!

## 🚀 3-Step Deployment

### Step 1: Database (5 min)
1. Sign up at [MongoDB Atlas](https://www.mongodb.com/cloud/atlas/register)
2. Create **M0 FREE** cluster
3. Create database user
4. Allow access from anywhere (0.0.0.0/0)
5. Get connection string

### Step 2: Backend (5 min)
1. Sign up at [Railway.app](https://railway.app)
2. Create new project from GitHub repo
3. Set root directory: `backend/src/MillionBackend.API`
4. Add environment variables:
   ```env
   ASPNETCORE_ENVIRONMENT=Production
   ConnectionStrings__MongoDB=mongodb+srv://user:password@cluster.mongodb.net/MillionTestDB
   MongoDBSettings__DatabaseName=MillionTestDB
   CorsOrigins=https://your-app.vercel.app,http://localhost:3000
   ```
5. Generate domain and copy URL

### Step 3: Frontend (5 min)
1. Sign up at [Vercel.com](https://vercel.com)
2. Import GitHub repo
3. Set root directory: `frontend`
4. Add environment variable:
   ```env
   NEXT_PUBLIC_API_URL=https://your-app.up.railway.app
   ```
5. Deploy!

## ✅ Done!

Your app is live at:
- **Frontend**: https://your-app.vercel.app
- **Backend**: https://your-app.up.railway.app/swagger
- **Cost**: $0/month 🎉

---

📖 **Full Guide**: See [FREE-DEPLOYMENT.md](FREE-DEPLOYMENT.md) for detailed instructions.
