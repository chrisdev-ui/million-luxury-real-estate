# Frontend Setup Guide

> Complete step-by-step guide to setting up the Million Luxury Frontend application locally on your machine.

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Verification](#verification)
- [Project Structure](#project-structure)
- [Key Features](#key-features)
- [Development Workflow](#development-workflow)
- [Troubleshooting](#troubleshooting)
- [Building for Production](#building-for-production)

---

## Prerequisites

Before starting, ensure you have the following installed on your machine:

### Required Software

| Software        | Version              | Download Link                                           | Purpose                   |
| --------------- | -------------------- | ------------------------------------------------------- | ------------------------- |
| **Node.js**     | 20.x or higher (LTS) | [Download](https://nodejs.org/)                         | Run JavaScript/TypeScript |
| **npm**         | 10.x or higher       | Included with Node.js                                   | Package manager           |
| **Git**         | Latest               | [Download](https://git-scm.com/downloads)               | Clone repository          |
| **Code Editor** | Any                  | [VS Code](https://code.visualstudio.com/) (recommended) | Edit code                 |

### Verify Installations

Open a terminal and run these commands:

```bash
# Check Node.js version
node --version
# Expected output: v20.9.0 or higher

# Check npm version
npm --version
# Expected output: 10.0.0 or higher

# Check Git version
git --version
# Expected output: git version 2.40.0 or higher
```

⚠️ **Important:** The project requires Node.js 20+ for Next.js 16 compatibility.

---

## Installation Steps

### Step 1: Clone the Repository

If you haven't already cloned the repository:

```bash
# Clone the repository
git clone <repository-url>

# Navigate to the project root
cd MillionTest

# Navigate to the frontend directory
cd frontend
```

### Step 2: Install Dependencies

Install all required npm packages:

```bash
# Install dependencies (this may take a few minutes)
npm install
```

**What this does:**

- Downloads all packages from `package.json`
- Creates `node_modules/` directory
- Installs Next.js, React, Tailwind CSS, Shadcn/UI, and all other dependencies
- Generates `package-lock.json` for reproducible builds

**Expected output:**

```
added 450 packages in 2m

45 packages are looking for funding
  run `npm fund` for details
```

**Package sizes:**

- Total size: ~200-300 MB
- Number of packages: ~450

### Step 3: Verify Installation

Check that key packages were installed:

```bash
# List installed packages
npm list --depth=0
```

**You should see:**

- `next@16.x.x`
- `react@19.x.x`
- `typescript@5.x.x`
- `tailwindcss@4.x.x`
- And many more...

---

## Configuration

### Step 1: Create Environment File

Create a `.env.local` file for environment variables:

```bash
# From the frontend directory
touch .env.local
```

### Step 2: Configure API URL

Open `.env.local` in your editor and add:

```env
# Backend API URL
NEXT_PUBLIC_API_URL=http://localhost:5208

# Optional: Enable debug mode
# NEXT_PUBLIC_DEBUG=true
```

**Important Notes:**

- `NEXT_PUBLIC_` prefix makes the variable accessible in the browser
- The API URL must match your backend URL
- Never commit `.env.local` to Git (it's in `.gitignore`)

### Step 3: Verify Configuration File Exists

```bash
# Check if .env.local exists
ls -la | grep .env.local
```

**Expected output:**

```
-rw-r--r--  1 user  staff   50 Jan 15 10:00 .env.local
```

---

## Running the Application

### Step 1: Ensure Backend is Running

Before starting the frontend, make sure the backend API is running:

```bash
# In a separate terminal, from the backend directory
cd backend/src/MillionBackend.API
dotnet run
```

Verify backend is running at: `http://localhost:5208/swagger`

### Step 2: Start the Development Server

From the frontend directory:

```bash
# Start Next.js development server
npm run dev
```

**What this does:**

- Starts Next.js development server
- Enables Hot Module Replacement (HMR) for instant updates
- Compiles TypeScript to JavaScript
- Processes Tailwind CSS
- Starts listening for HTTP requests

**Expected output:**

```
  ▲ Next.js 16.0.0
  - Local:        http://localhost:3000
  - Network:      http://192.168.1.x:3000

 ✓ Ready in 2.5s
 ○ Compiling / ...
 ✓ Compiled / in 1.2s
```

### Step 3: Alternative Commands

```bash
# Start with turbopack (faster builds - experimental)
npm run dev --turbo

# Start on different port
npm run dev -- -p 3001

# Start with specific hostname
npm run dev -- -H 0.0.0.0
```

---

## Verification

### Step 1: Open the Application

Open your web browser and navigate to:

```
http://localhost:3000
```

**What you should see:**

- Million Luxury property listing page
- Header with site name and theme toggle
- Property filters (name, address, price range)
- Grid of property cards with images
- Footer with company information

### Step 2: Test Property Listing

1. **Verify properties are loading:**

   - You should see a grid of property cards
   - Each card shows: image, name, address, price, badges
   - Properties should have data from the backend

2. **Check for errors:**
   - Open browser DevTools (F12)
   - Go to Console tab
   - Should have no red errors
   - If you see connection errors, verify backend is running

### Step 3: Test Filtering

#### Test 1: Filter by Name

1. Type "Villa" in the property name search box
2. Wait 500ms (debounce delay)
3. URL should update to include `?name=Villa`
4. Property list should filter to only show properties with "Villa" in the name

#### Test 2: Filter by Price Range

1. Enter minimum price: `500000`
2. Enter maximum price: `2000000`
3. Click "Apply Filters" or wait for debounce
4. URL should include `?minPrice=500000&maxPrice=2000000`
5. Only properties in that price range should display

#### Test 3: Combined Filters

1. Name: "Luxury"
2. Address: "Miami"
3. Min Price: 1000000
4. Click "Apply Filters"
5. Should show properties matching ALL criteria

#### Test 4: Clear Filters

1. Click "Clear Filters" button
2. All filters should reset
3. URL should return to `/`
4. Full property list should display

### Step 4: Test Property Details

#### Option 1: Modal View (Intercepting Route)

1. Click on any property card
2. Modal should open with property details:
   - Main property image
   - Property name, address, price
   - Image gallery
   - Owner information (with avatar)
   - Sales history timeline
3. Click outside modal or X button to close
4. Should return to property listing

#### Option 2: Full Page View

1. Right-click a property card
2. Open in new tab
3. Full page should load with same property details
4. Back button should be visible
5. Click back button to return to listing

### Step 5: Test Responsive Design

#### Mobile View

1. Open DevTools (F12)
2. Toggle device toolbar (Ctrl+Shift+M or Cmd+Shift+M)
3. Select iPhone or similar device
4. Verify:
   - Properties display in single column
   - Filters stack vertically
   - Navigation menu is touch-friendly
   - Modal fits on screen

#### Tablet View

1. Select iPad or similar tablet device
2. Verify:
   - Properties display in 2 columns
   - Filters display in 2 columns
   - Layout adjusts smoothly

#### Desktop View

1. Return to desktop view (responsive mode off)
2. Verify:
   - Properties display in 3 columns
   - Filters display in 4 columns
   - Full layout with proper spacing

### Step 6: Test Dark Mode

1. Click the theme toggle button (sun/moon icon) in the header
2. Page should switch between light and dark mode
3. All components should adapt to the theme
4. Preference should persist on page reload (saved in localStorage)

---

## Project Structure

Understanding the folder structure will help you navigate and modify the application.

```
frontend/
├── app/                          # Next.js App Router
│   ├── @modal/                   # Parallel route for modals
│   │   ├── (.)properties/[id]/   # Intercepting route
│   │   │   ├── page.tsx          # Modal page
│   │   │   └── modal.tsx         # Modal component
│   │   └── default.tsx           # Default modal slot
│   ├── properties/[id]/          # Property detail page
│   │   └── page.tsx              # Full page view
│   ├── layout.tsx                # Root layout (header, footer)
│   ├── page.tsx                  # Home page (property list)
│   ├── error.tsx                 # Error boundary
│   ├── not-found.tsx             # 404 page
│   └── default.tsx               # Default parallel route
│
├── components/                   # React components
│   ├── properties/               # Property-specific components
│   │   ├── property-card.tsx     # Property card component
│   │   ├── property-list.tsx     # Property list wrapper
│   │   ├── property-filters.tsx  # Filter form
│   │   ├── property-detail.tsx   # Full property detail
│   │   ├── property-detail-modal.tsx  # Modal property detail
│   │   ├── property-pagination.tsx    # Pagination component
│   │   ├── property-skeleton.tsx      # Loading skeleton for list
│   │   ├── property-detail-skeleton.tsx     # Loading skeleton for details
│   │   └── property-filters-skeleton.tsx    # Loading skeleton for filters
│   ├── layout/                   # Layout components
│   │   └── site-header.tsx       # Site header with navigation
│   ├── ui/                       # Shadcn/UI components
│   │   ├── button.tsx            # Button component
│   │   ├── card.tsx              # Card component
│   │   ├── dialog.tsx            # Dialog/Modal component
│   │   ├── input.tsx             # Input component
│   │   ├── skeleton.tsx          # Skeleton component
│   │   └── [35+ more components]
│   ├── footer.tsx                # Site footer
│   ├── bottom-bar.tsx            # Footer bottom bar
│   ├── page-container.tsx        # Page wrapper
│   └── providers.tsx             # Theme provider
│
├── lib/                          # Utility libraries
│   ├── api/                      # API client
│   │   ├── client.ts             # Base API client
│   │   ├── properties.ts         # Property API functions
│   │   └── types.ts              # TypeScript types
│   ├── validations/              # Zod schemas
│   │   └── property.ts           # Property validation
│   ├── fonts.ts                  # Font configurations
│   └── utils.ts                  # Utility functions
│
├── hooks/                        # Custom React hooks
│   └── use-debounced-callback.ts # Debounce hook
│
├── config/                       # Configuration files
│   └── site.ts                   # Site config (name, description, etc.)
│
├── styles/                       # Global styles
│   └── globals.css               # Tailwind CSS + custom styles
│
├── public/                       # Static assets
│   └── images/                   # Images
│
├── .env.local                    # Environment variables (not in Git)
├── .env.example                  # Example environment file
├── next.config.ts                # Next.js configuration
├── tailwind.config.ts            # Tailwind CSS configuration
├── tsconfig.json                 # TypeScript configuration
├── package.json                  # Dependencies and scripts
└── README.md                     # Frontend-specific README
```

---

## Key Features

### 1. Server Components (Default)

Most components are Server Components for better performance:

```typescript
// app/page.tsx - Server Component (default)
export default function Home(props: HomeProps) {
  return (
    <PageContainer>
      <Suspense fallback={<PropertyFiltersSkeleton />}>
        <PropertyFilters />
      </Suspense>
      <Suspense fallback={<PropertyListSkeleton length={10} />}>
        <PropertyListWrapper {...props} />
      </Suspense>
    </PageContainer>
  );
}
```

**Benefits:**

- Faster initial page load
- Reduced JavaScript bundle size
- SEO-friendly

### 2. Client Components (When Needed)

Interactive components use `"use client"` directive:

```typescript
// components/properties/property-filters.tsx
"use client";

import { useForm } from "@tanstack/react-form";
import { useQueryStates } from "nuqs";

export function PropertyFilters() {
  // Client-side state management
  // Form handling
  // Event listeners
}
```

**Used for:**

- Forms and input handling
- State management
- Event listeners
- Browser APIs (localStorage, etc.)

### 3. Suspense Boundaries

Loading states with Suspense:

```typescript
<Suspense fallback={<PropertyDetailSkeleton />}>
  <PropertyDetail promise={promise} />
</Suspense>
```

**Benefits:**

- Progressive rendering
- Better perceived performance
- Automatic loading states

### 4. Intercepting Routes

Modal with intercepting routes:

```
@modal/(.)properties/[id]/page.tsx  → Intercepts /properties/[id]
```

**Flow:**

1. Click property card → Opens modal
2. Direct navigation → Opens full page
3. Best of both worlds!

### 5. URL State Management

Filters synchronized with URL using `nuqs`:

```typescript
const [filters, setFilters] = useQueryStates({
  name: parseAsString,
  address: parseAsString,
  minPrice: parseAsInteger,
  maxPrice: parseAsInteger,
});
```

**Benefits:**

- Shareable URLs
- Browser back/forward works
- Bookmarkable filter states

### 6. TypeScript Type Safety

All code is fully typed:

```typescript
export interface Property {
  idProperty: string;
  name: string;
  address: string;
  price: number;
  // ... all fields typed
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
}
```

**Benefits:**

- Catch errors at compile time
- Better IDE autocomplete
- Self-documenting code

---

## Development Workflow

### Daily Development Routine

#### 1. Start Your Day

```bash
# 1. Navigate to frontend directory
cd frontend

# 2. Ensure backend is running
# (in separate terminal)

# 3. Start development server
npm run dev

# 4. Open browser to http://localhost:3000
```

#### 2. Make Changes

The development server includes Hot Module Replacement (HMR):

1. Edit any file in your IDE
2. Save the file (Ctrl+S or Cmd+S)
3. Browser automatically refreshes with changes
4. No manual reload needed!

#### 3. Check for Errors

**TypeScript Errors:**

```bash
# Check TypeScript errors
npm run type-check
```

**Linting Errors:**

```bash
# Check ESLint errors
npm run lint

# Auto-fix fixable errors
npm run lint --fix
```

#### 4. Test Your Changes

1. **Manual Testing:** Test in browser
2. **Different Devices:** Use DevTools device emulation
3. **Dark Mode:** Toggle theme and verify
4. **Error Cases:** Test error scenarios

#### 5. End Your Day

```bash
# Stop the development server (Ctrl+C in terminal)
```

### Adding a New Component

#### Step 1: Create Component File

```bash
# Create a new component
touch components/properties/property-map.tsx
```

#### Step 2: Implement Component

```typescript
// components/properties/property-map.tsx
import { Property } from "@/lib/api/types";

interface PropertyMapProps {
  property: Property;
}

export function PropertyMap({ property }: PropertyMapProps) {
  return (
    <div className="aspect-video w-full rounded-lg border">
      <p className="text-sm text-muted-foreground">
        Map for: {property.address}
      </p>
    </div>
  );
}
```

#### Step 3: Use Component

```typescript
// app/properties/[id]/page.tsx
import { PropertyMap } from "@/components/properties/property-map";

export default function PropertyPage({ params }: PropertyPageProps) {
  return (
    <div>
      <PropertyDetail />
      <PropertyMap property={property} />
    </div>
  );
}
```

### Adding a New API Endpoint

#### Step 1: Define TypeScript Types

```typescript
// lib/api/types.ts
export interface PropertyStats {
  totalProperties: number;
  averagePrice: number;
  totalValue: number;
}
```

#### Step 2: Create API Function

```typescript
// lib/api/properties.ts
export async function getPropertyStats(): Promise<ApiResponse<PropertyStats>> {
  return apiFetch<ApiResponse<PropertyStats>>(`/api/properties/stats`);
}
```

#### Step 3: Use in Component

```typescript
// components/properties/property-stats.tsx
"use client";

import { use } from "react";
import { getPropertyStats } from "@/lib/api/properties";

export function PropertyStats() {
  const { data: stats } = use(getPropertyStats());

  return (
    <div>
      <p>Total Properties: {stats.totalProperties}</p>
      <p>Average Price: ${stats.averagePrice.toLocaleString()}</p>
    </div>
  );
}
```

---

## Troubleshooting

### Problem 1: Cannot Connect to API

**Error:**

```
Failed to fetch: Network error
ECONNREFUSED 127.0.0.1:5208
```

**Solutions:**

1. **Verify backend is running:**

   ```bash
   curl http://localhost:5208/api/properties
   ```

2. **Check `.env.local` file:**

   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5208
   ```

3. **Restart frontend:**

   ```bash
   # Stop with Ctrl+C
   npm run dev
   ```

4. **Check browser console for CORS errors** - Backend should allow `http://localhost:3000`

### Problem 2: Port 3000 Already in Use

**Error:**

```
Port 3000 is already in use
```

**Solutions:**

1. **Kill process using port 3000:**

   **On macOS/Linux:**

   ```bash
   lsof -ti:3000 | xargs kill -9
   ```

   **On Windows:**

   ```cmd
   netstat -ano | findstr :3000
   taskkill /PID <PID> /F
   ```

2. **Use a different port:**
   ```bash
   npm run dev -- -p 3001
   ```

### Problem 3: Module Not Found

**Error:**

```
Module not found: Can't resolve '@/components/...'
```

**Solutions:**

1. **Reinstall dependencies:**

   ```bash
   rm -rf node_modules package-lock.json
   npm install
   ```

2. **Check import path:**

   ```typescript
   // Correct
   import { Button } from "@/components/ui/button";

   // Incorrect
   import { Button } from "components/ui/button";
   ```

3. **Verify tsconfig.json has path alias:**
   ```json
   {
     "compilerOptions": {
       "paths": {
         "@/*": ["./*"]
       }
     }
   }
   ```

### Problem 4: TypeScript Errors

**Error:**

```
Type 'string | undefined' is not assignable to type 'string'
```

**Solutions:**

1. **Add nullish coalescing:**

   ```typescript
   // Before
   value={field.state.value}

   // After
   value={field.state.value ?? ""}
   ```

2. **Check TypeScript version:**

   ```bash
   npm list typescript
   # Should be 5.x.x
   ```

3. **Run type check:**
   ```bash
   npm run type-check
   ```

### Problem 5: Styles Not Loading

**Error:**

- Page loads but looks unstyled
- Tailwind classes not working

**Solutions:**

1. **Verify Tailwind is configured:**

   ```bash
   # Check if tailwind.config.ts exists
   ls -la tailwind.config.ts
   ```

2. **Restart development server:**

   ```bash
   # Ctrl+C to stop
   npm run dev
   ```

3. **Clear Next.js cache:**

   ```bash
   rm -rf .next
   npm run dev
   ```

4. **Check globals.css is imported in layout:**
   ```typescript
   // app/layout.tsx
   import "@/styles/globals.css";
   ```

### Problem 6: Environment Variables Not Working

**Error:**

```
NEXT_PUBLIC_API_URL is undefined
```

**Solutions:**

1. **Verify file name is exactly `.env.local`:**

   ```bash
   ls -la | grep .env
   ```

2. **Restart development server** (required after changing env vars):

   ```bash
   # Ctrl+C to stop
   npm run dev
   ```

3. **Check variable has `NEXT_PUBLIC_` prefix:**

   ```env
   # Correct
   NEXT_PUBLIC_API_URL=http://localhost:5208

   # Incorrect (won't work in browser)
   API_URL=http://localhost:5208
   ```

4. **Verify in browser console:**
   ```javascript
   console.log(process.env.NEXT_PUBLIC_API_URL);
   ```

---

## Building for Production

### Step 1: Create Production Build

```bash
# From frontend directory
npm run build
```

**What this does:**

- Compiles TypeScript to JavaScript
- Bundles all code with tree-shaking
- Minifies JavaScript and CSS
- Optimizes images
- Generates static pages where possible
- Creates production-ready `.next/` directory

**Expected output:**

```
   ▲ Next.js 15.0.0

   Creating an optimized production build ...
 ✓ Compiled successfully
 ✓ Linting and checking validity of types
 ✓ Collecting page data
 ✓ Generating static pages (7/7)
 ✓ Collecting build traces
 ✓ Finalizing page optimization

Route (app)                                Size     First Load JS
┌ ○ /                                      5.2 kB         95.3 kB
├ ○ /_not-found                            871 B          85.7 kB
├ ○ /properties/[id]                       3.1 kB         98.2 kB

○  (Static)  prerendered as static content
```

### Step 2: Test Production Build Locally

```bash
# Start production server
npm start
```

### Step 3: Deploy to Production

#### Option 1: Vercel (Recommended for Next.js)

```bash
# Install Vercel CLI
npm install -g vercel

# Deploy
vercel
```

#### Option 2: Docker

```dockerfile
# Dockerfile
FROM node:20-alpine AS base

# Install dependencies
FROM base AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci

# Build
FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

# Production
FROM base AS runner
WORKDIR /app
ENV NODE_ENV=production
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/public ./public
COPY --from=builder /app/package.json ./package.json

EXPOSE 3000
CMD ["npm", "start"]
```

```bash
# Build Docker image
docker build -t million-luxury-frontend .

# Run container
docker run -p 3000:3000 million-luxury-frontend
```

#### Option 3: Static Export (if applicable)

```bash
# In next.config.ts
export default {
  output: 'export',
};

# Build static site
npm run build

# Deploy the 'out' directory to any static host
```

---

## Performance Optimization

### Lighthouse Scores

Run Lighthouse in Chrome DevTools:

1. Open DevTools (F12)
2. Go to "Lighthouse" tab
3. Click "Analyze page load"

**Target Scores:**

- Performance: 90+
- Accessibility: 95+
- Best Practices: 95+
- SEO: 100

### Image Optimization

Next.js Image component automatically:

- Serves images in modern formats (WebP, AVIF)
- Resizes images based on device
- Lazy loads images
- Prevents layout shift

```typescript
<Image
  src={property.mainImage}
  alt={property.name}
  width={400}
  height={300}
  priority={isFirstImage} // Preload first image
  loading="lazy" // Lazy load others
/>
```

### Code Splitting

Next.js automatically splits code by route:

- Each page only loads its required JavaScript
- Shared components are in a shared chunk
- Dynamic imports for heavy components

```typescript
// Dynamic import for heavy component
import dynamic from "next/dynamic";

const PropertyMap = dynamic(
  () => import("@/components/properties/property-map"),
  {
    loading: () => <p>Loading map...</p>,
    ssr: false, // Don't render on server
  }
);
```

---

## Next Steps

Congratulations! Your frontend is now set up and running. 🎉

### What's Next?

1. **[Explore the Application](http://localhost:3000)** - Browse properties and test features
2. **[API Architecture](../api/architecture.md)** - Understand how backend and frontend connect
3. **[Backend Setup](./backend-setup.md)** - Ensure backend is properly configured
4. **[Customize](../../frontend/)** - Add your own features and styling

### Learning Resources

- **Next.js Documentation:** https://nextjs.org/docs
- **React Documentation:** https://react.dev/
- **Tailwind CSS:** https://tailwindcss.com/docs
- **Shadcn/UI:** https://ui.shadcn.com/
- **TypeScript:** https://www.typescriptlang.org/docs/

### Need Help?

- Check [Troubleshooting](#troubleshooting) section
- Review browser console for errors
- Open an issue in the repository
- Contact the development team

---

**Happy Coding! 🚀**
