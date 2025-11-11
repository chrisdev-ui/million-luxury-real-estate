<div align="center">
  <img src="docs/assets/logo.webp" alt="Million Luxury Logo" width="200"/>

  # Million Luxury - Real Estate Management System

  > A production-ready, full-stack real estate property management application showcasing modern web development practices with .NET 9 and Next.js 16.

  [![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
  [![Next.js](https://img.shields.io/badge/Next.js-15-000000?logo=next.js)](https://nextjs.org/)
  [![MongoDB](https://img.shields.io/badge/MongoDB-Latest-47A248?logo=mongodb)](https://www.mongodb.com/)
  [![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?logo=typescript)](https://www.typescriptlang.org/)
  [![Tests](https://img.shields.io/badge/Tests-18%2F18%20Passing-success)](https://github.com/)
  [![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/chrisdev-ui/million-luxury-real-estate)
</div>

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Tech Stack](#-tech-stack)
- [Quick Start](#-quick-start)
- [Documentation](#-documentation)
- [Architecture](#-architecture)
- [API Endpoints](#-api-endpoints)
- [Testing](#-testing)
- [Performance](#-performance)
- [Contributing](#-contributing)

## 🌟 Overview

Million Luxury is a comprehensive real estate management platform that enables users to browse, filter, and explore luxury properties. Built with enterprise-grade architecture and modern web technologies, it demonstrates best practices in full-stack development.

### 🚀 Live Demo

- **Frontend:** [https://millionluxury.vercel.app](https://millionluxury.vercel.app)
- **Backend API:** [https://millionluxury-api.azurewebsites.net](https://millionluxury-api.azurewebsites.net)
- **API Documentation (Swagger):** [https://millionluxury-api.azurewebsites.net/swagger](https://millionluxury-api.azurewebsites.net/swagger)

### Key Highlights

- **Production Ready** - Deployed on Azure (Backend) and Vercel (Frontend)
- **Clean Architecture** - Separation of concerns with maintainable, testable code
- **Modern UI/UX** - Responsive design with dark mode support
- **Type Safety** - End-to-end TypeScript implementation
- **Performance** - Optimized with server-side rendering, code splitting, and database indexing
- **Real-time Filtering** - Debounced search with URL state synchronization
- **Comprehensive Testing** - 18/18 unit tests passing
- **CI/CD Pipeline** - Automated testing and build validation
- **Cloud Database** - MongoDB Atlas for production-ready data storage

## ✨ Features

### Backend Features

- ✅ **RESTful API** - Complete CRUD operations with Swagger documentation
- ✅ **Advanced Filtering** - Search by name, address, and price range
- ✅ **Pagination** - Efficient handling of large datasets
- ✅ **Clean Architecture** - Layered approach with dependency injection
- ✅ **MongoDB Integration** - NoSQL database with optimized indexes
- ✅ **Error Handling** - Global exception middleware with detailed logging
- ✅ **Unit Testing** - NUnit + Moq with 100% passing tests
- ✅ **AutoMapper** - DTO mapping for clean data transfer
- ✅ **CORS Support** - Configured for frontend integration

### Frontend Features

- ✅ **Property Listing** - Grid layout with responsive design (1/2/3 columns)
- ✅ **Real-time Search** - Debounced filters with URL state management
- ✅ **Property Details** - Modal with intercepting routes + full page view
- ✅ **Image Gallery** - Multiple property images with optimization
- ✅ **Owner Information** - Full relationship data with avatars
- ✅ **Sales History** - Complete property trace timeline
- ✅ **Pagination** - Navigate through property results
- ✅ **Loading States** - Professional skeleton loaders
- ✅ **Error Handling** - Graceful error boundaries with retry
- ✅ **404 Page** - Custom not found page
- ✅ **Dark Mode** - Theme switching support
- ✅ **Mobile-First** - Touch-friendly responsive design
- ✅ **SEO Optimized** - Server-side rendering with metadata
- ✅ **Accessibility** - ARIA labels and semantic HTML

## 🏗️ Project Structure

```
million-luxury-real-estate/
├── backend/                          # .NET 9 Web API
│   ├── src/
│   │   ├── MillionBackend.API/      # Controllers, DTOs, Program.cs
│   │   ├── MillionBackend.Application/ # Services (Business Logic)
│   │   ├── MillionBackend.Core/     # Domain Models, Interfaces
│   │   └── MillionBackend.Infrastructure/ # Repository Implementations
│   ├── tests/
│   │   └── MillionBackend.Tests/    # Unit Tests (NUnit)
│   ├── postman/
│   │   └── postman_collection.json  # Postman API collection
│   ├── scripts/
│   │   ├── seed-data.js             # Database seed (light)
│   │   ├── seed-data-heavy.js       # Database seed (100 properties)
│   │   └── clean-db.js              # Database cleanup script
│   └── docker-compose.yml           # MongoDB container
│
├── frontend/                         # Next.js 16 Application
│   ├── app/                         # App Router pages
│   │   ├── @modal/                  # Parallel route for modals
│   │   ├── properties/[id]/         # Property detail page
│   │   ├── layout.tsx               # Root layout
│   │   ├── page.tsx                 # Home page
│   │   ├── error.tsx                # Error boundary
│   │   └── not-found.tsx            # 404 page
│   ├── components/                  # React components
│   │   ├── properties/              # Property-specific components
│   │   ├── ui/                      # Shadcn/UI components
│   │   ├── layout/                  # Layout components
│   │   └── footer.tsx               # Site footer
│   ├── lib/                         # Utilities
│   │   ├── api/                     # API client functions
│   │   └── validations/             # Zod schemas
│   └── public/                      # Static assets
│
└── docs/                            # Comprehensive Documentation
    ├── api/                         # API documentation
    ├── setup/                       # Setup guides
    └── technical-requirements.md    # Original requirements
```

## 🛠️ Tech Stack

### Backend

| Technology         | Version | Purpose              |
| ------------------ | ------- | -------------------- |
| **.NET**           | 9.0     | Web API framework    |
| **C#**             | 12      | Programming language |
| **MongoDB**        | 7.0+    | NoSQL database       |
| **MongoDB.Driver** | Latest  | Database driver      |
| **AutoMapper**     | Latest  | DTO mapping          |
| **NUnit**          | Latest  | Testing framework    |
| **Moq**            | Latest  | Mocking library      |
| **Swashbuckle**    | Latest  | Swagger/OpenAPI      |

### Frontend

| Technology        | Version | Purpose              |
| ----------------- | ------- | -------------------- |
| **Next.js**       | 16      | React framework      |
| **React**         | 19      | UI library           |
| **TypeScript**    | 5.0     | Type safety          |
| **Tailwind CSS**  | 4.0     | Styling              |
| **Shadcn/UI**     | Latest  | UI components        |
| **Zod**           | Latest  | Schema validation    |
| **TanStack Form** | Latest  | Form management      |
| **nuqs**          | Latest  | URL state management |
| **Lucide React**  | Latest  | Icons                |

### Development Tools

- **Docker** - MongoDB containerization
- **ESLint** - Code linting
- **Prettier** - Code formatting
- **Git** - Version control

## 🚀 Quick Start

### Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js 20+** - [Download](https://nodejs.org/) (LTS recommended)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **Git** - [Download](https://git-scm.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/chrisdev-ui/million-luxury-real-estate.git
cd million-luxury-real-estate
```

### 2. Backend Setup

```bash
# Navigate to backend directory
cd backend

# Start MongoDB with Docker
docker-compose up -d

# Wait for MongoDB to be ready (5-10 seconds)
# Verify MongoDB is running
docker ps

# Seed the database with sample data
docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < seed-data-heavy.js

# Navigate to API project
cd src/MillionBackend.API

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

**Backend URLs:**

- API: `http://localhost:5208`
- Swagger: `http://localhost:5208/swagger`

### 3. Frontend Setup

```bash
# Open new terminal and navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Create environment file
cp .env.example .env.local

# Edit .env.local and add:
# NEXT_PUBLIC_API_URL=http://localhost:5208

# Run development server
npm run dev
```

**Frontend URL:** `http://localhost:3000`

### 4. Verify Installation

1. Open `http://localhost:5208/swagger` - API documentation should load
2. Open `http://localhost:3000` - Property listing should appear
3. Try filtering properties by name, address, or price
4. Click on a property card to view details

## 📚 Documentation

### Complete Guides

- **[API Architecture Documentation](docs/api/architecture.md)** - Detailed explanation of backend architecture
- **[Backend Setup Guide](docs/setup/backend-setup.md)** - Step-by-step backend installation
- **[Frontend Setup Guide](docs/setup/frontend-setup.md)** - Step-by-step frontend installation
- **[Technical Requirements](docs/technical-requirements.md)** - Original project specifications

### API Documentation

- **Swagger UI:** `http://localhost:5208/swagger` (when API is running)
- **Interactive:** Test all endpoints directly from the browser
- **Postman Collection:** `backend/postman/postman_collection.json` - Import into Postman for API testing

### Database Scripts

Located in `backend/scripts/`:

- **`seed-data.js`** - Light seed data (basic properties)
- **`seed-data-heavy.js`** - Heavy seed data (100 properties for testing pagination)
- **`clean-db.js`** - Clean/reset database

**Usage:**
```bash
# Seed database with heavy data
docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < backend/scripts/seed-data-heavy.js

# Clean database
docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < backend/scripts/clean-db.js
```

## 🏛️ Architecture

### Backend - Clean Architecture

```
Controllers (API Layer)
    ↓
Services (Business Logic Layer)
    ↓
Repositories (Data Access Layer)
    ↓
MongoDB (Database)
```

**Layers:**

- **API** - Controllers, DTOs, AutoMapper profiles, Middleware
- **Application** - Services with business logic
- **Core** - Domain models, interfaces, specifications
- **Infrastructure** - Repository implementations, MongoDB configuration

### Frontend - Modern Next.js Architecture

```
App Router Pages (Route Handlers)
    ↓
Server Components (Data Fetching)
    ↓
Client Components (Interactivity)
    ↓
API Client (HTTP Requests)
    ↓
Backend API
```

**Patterns:**

- **Server Components** - Default for data fetching and performance
- **Client Components** - Interactive UI elements ("use client")
- **Parallel Routes** - Modal with intercepting routes
- **Suspense Boundaries** - Streaming with skeleton loaders
- **Error Boundaries** - Graceful error handling

## 🌐 API Endpoints

### Properties

| Method   | Endpoint                 | Description              | Query Parameters                                                    |
| -------- | ------------------------ | ------------------------ | ------------------------------------------------------------------- |
| `GET`    | `/api/properties`        | Get paginated properties | `pageNumber`, `pageSize`                                            |
| `GET`    | `/api/properties/filter` | Filter properties        | `name`, `address`, `minPrice`, `maxPrice`, `pageNumber`, `pageSize` |
| `GET`    | `/api/properties/{id}`   | Get property by ID       | -                                                                   |
| `POST`   | `/api/properties`        | Create new property      | Body: `CreatePropertyDto`                                           |
| `PUT`    | `/api/properties/{id}`   | Update property          | Body: `UpdatePropertyDto`                                           |
| `DELETE` | `/api/properties/{id}`   | Delete property          | -                                                                   |

### Owners

| Method | Endpoint           | Description     |
| ------ | ------------------ | --------------- |
| `GET`  | `/api/owners`      | Get all owners  |
| `GET`  | `/api/owners/{id}` | Get owner by ID |
| `POST` | `/api/owners`      | Create owner    |

### Response Format

All responses follow this structure:

```json
{
  "success": true,
  "data": {
    /* response data */
  },
  "message": "Success message"
}
```

## 🧪 Testing

### Backend Tests

```bash
cd backend
dotnet test
```

**Test Coverage:**

- ✅ Service layer tests (business logic)
- ✅ Controller tests (API endpoints)
- ✅ Repository tests (data access)

**Current Status:** 18/18 tests passing ✅

**Test Categories:**

- Property services (CRUD operations)
- Filtering logic (name, address, price range)
- Pagination functionality
- Error handling scenarios

## 🔄 CI/CD Pipeline

The project includes automated CI/CD workflows that run on every push and pull request:

### Workflows

**1. Full Stack CI** (`.github/workflows/ci.yml`)
- Runs on every push to `main` or `develop`
- Tests both backend and frontend
- Provides summary of all checks

**2. Backend CI** (`.github/workflows/backend-ci.yml`)
- Triggers on backend file changes
- Runs .NET build
- Executes all unit tests (18 tests)
- Publishes test results
- **Fails the build if tests fail**

**3. Frontend CI** (`.github/workflows/frontend-ci.yml`)
- Triggers on frontend file changes
- Runs ESLint
- Builds Next.js application
- **Fails the build if linting or build fails**

### Protection Rules

To enforce code quality, configure branch protection rules:

1. Go to: **Settings → Branches → Add rule**
2. Branch name pattern: `main`
3. Enable: **Require status checks to pass before merging**
4. Select: `Backend CI`, `Frontend CI`, `CI Summary`

This ensures:
- ✅ All tests must pass
- ✅ Linting must pass
- ✅ Build must succeed
- ✅ No broken code reaches main branch

### Local CI Testing

Run the same checks locally before pushing:

```bash
# Backend: Build + Test
cd backend
dotnet build
dotnet test

# Frontend: Lint + Build
cd frontend
npm run lint
npm run build
```

### Frontend Testing

```bash
cd frontend
npm run lint        # ESLint checks
npm run type-check  # TypeScript validation
npm run build       # Production build test
```

## ⚡ Performance

### Backend Optimizations

- **MongoDB Indexes** - Indexed on `name`, `address`, `price`, `enabled`
- **Async/Await** - All database operations are asynchronous
- **Pagination** - Default 10 items per page, configurable
- **Connection Pooling** - Automatic via MongoDB driver
- **Response Compression** - Automatic via .NET middleware

**Benchmarks:**

- Database query time: < 100ms (typical dataset)
- API response time: < 200ms
- Concurrent requests: 100+ per second

### Frontend Optimizations

- **Server-Side Rendering** - Faster initial page load
- **Code Splitting** - Automatic with Next.js
- **Image Optimization** - Next.js Image component with lazy loading
- **Debounced Search** - 500ms delay reduces API calls
- **Skeleton Loaders** - Perceived performance improvement
- **Bundle Size** - Optimized with tree shaking

**Metrics:**

- First Contentful Paint: < 1.5s
- Lighthouse Score: 90+ (Performance)
- Bundle Size: < 200KB (gzipped)

## 📊 Database

### MongoDB Collections

**Production Database:** MongoDB Atlas

**owners**

- Stores property owner information
- Fields: name, address, photo, birthday

**properties**

- Main property data
- Fields: name, address, price, codeInternal, year, mainImage, enabled
- Indexes: name, address, price, enabled, idOwner

**propertyImages**

- Additional property images
- Relationship: Many-to-One with properties

**propertyTraces**

- Sales history and transactions
- Relationship: Many-to-One with properties

### Production Infrastructure

- **Frontend:** Vercel (Next.js 16)
- **Backend:** Azure App Service (B1 Basic tier, .NET 9)
- **Database:** MongoDB Atlas (M0 Free tier)
- **CI/CD:** GitHub Actions

## 🔧 Configuration

### Backend Configuration

File: `backend/src/MillionBackend.API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://admin:admin123@localhost:27017"
  },
  "MongoDBSettings": {
    "DatabaseName": "MillionTestDB"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Frontend Configuration

File: `frontend/.env.local`

```env
# API Base URL
NEXT_PUBLIC_API_URL=http://localhost:5208

# Optional: Analytics, Monitoring
# NEXT_PUBLIC_GA_ID=your-google-analytics-id
```

## 📝 Development Workflow

### Daily Development

1. **Start MongoDB:**

   ```bash
   cd backend && docker-compose up -d
   ```

2. **Run Backend:**

   ```bash
   cd backend/src/MillionBackend.API
   dotnet watch run  # Auto-reload on changes
   ```

3. **Run Frontend:**

   ```bash
   cd frontend
   npm run dev  # Auto-reload on changes
   ```

4. **Access Applications:**
   - Frontend: `http://localhost:3000`
   - API: `http://localhost:5208`
   - Swagger: `http://localhost:5208/swagger`
   - MongoDB: `mongodb://admin:admin123@localhost:27017`

### Stopping Services

```bash
# Stop frontend (Ctrl+C in terminal)

# Stop backend (Ctrl+C in terminal)

# Stop MongoDB
cd backend
docker-compose down
```

## 🤝 Contributing

This is a technical assessment project. For production use:

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a pull request

## 📄 License

Private - Technical Assessment Project

---

## 🙏 Acknowledgments

- **Next.js Team** - Excellent framework and documentation
- **Shadcn/UI** - Beautiful component library
- **MongoDB** - Flexible NoSQL database
- **Vercel** - Inspiration for modern web practices

---

**Built with ❤️**
