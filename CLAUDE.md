# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a full-stack real estate property management system for Million Luxury, consisting of:

- **Backend**: .NET 9 Web API with MongoDB
- **Frontend**: Next.js 15 with TypeScript and Tailwind CSS

The project follows a monorepo structure with both applications in a single repository.

## Repository Structure

```
MillionTest/
├── backend/                          # .NET 9 API (Clean Architecture)
│   ├── src/
│   │   ├── MillionBackend.API/
│   │   ├── MillionBackend.Application/
│   │   ├── MillionBackend.Core/
│   │   └── MillionBackend.Infrastructure/
│   └── tests/
├── frontend/                         # Next.js 15 Application
│   ├── app/
│   ├── components/
│   └── lib/
└── docs/                            # Documentation
```

## Backend (.NET API)

### Architecture

Clean Architecture with:
- **API Layer**: Controllers, DTOs, AutoMapper
- **Application Layer**: Business logic services
- **Core Layer**: Domain models, interfaces
- **Infrastructure Layer**: Repository implementations, MongoDB

### Key Patterns

- Repository Pattern for data access
- Service Layer for business logic
- Dependency Injection throughout
- AutoMapper for DTO mapping

### MongoDB Collections

- `properties` - Main property data with embedded structure consideration
- `owners` - Property owner information (referenced)
- `propertyImages` - Separate collection (FK: IdProperty)
- `propertyTraces` - Sales history (FK: IdProperty)

### Common Commands

```bash
# Navigate to backend
cd backend

# Build
dotnet build

# Run API
cd src/MillionBackend.API && dotnet run

# Run tests
dotnet test

# Start MongoDB
docker-compose up -d

# Seed database
docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < seed-data-heavy.js
```

### API Endpoints

- `GET /api/properties` - Paginated list
- `GET /api/properties/filter` - Filter by name/address/price
- `GET /api/properties/{id}` - Single property with related data
- `POST /api/properties` - Create property
- `PUT /api/properties/{id}` - Update property
- `DELETE /api/properties/{id}` - Delete property

### Testing

- Framework: NUnit + Moq
- Current: 17/17 tests passing
- Location: `backend/tests/MillionBackend.Tests/`

## Frontend (Next.js)

### Architecture

- **App Router**: Server Components by default
- **Pages**: Dynamic routes in `app/` directory
- **Components**: Reusable UI in `components/`
- **API Integration**: Server-side fetching in `lib/api/`

### Tech Stack

- Next.js 15 (App Router)
- TypeScript
- Tailwind CSS
- Shadcn/UI components
- Zod validation

### Key Patterns

- Server Components for data fetching
- Client Components for interactivity
- Server Actions for mutations
- Suspense boundaries for loading states
- Error boundaries for error handling

### Common Commands

```bash
# Navigate to frontend
cd frontend

# Install dependencies
npm install

# Run dev server
npm run dev

# Build
npm run build

# Run production
npm start
```

### Environment Variables

Create `frontend/.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:5208
```

## Integration Points

### API Communication

Frontend connects to backend via:

- Base URL: `http://localhost:5208`
- Response format: `ApiResponse<T>` wrapper
- CORS configured on backend for `http://localhost:3000`

### Data Flow

1. Frontend Server Component calls `lib/api/properties.ts`
2. API client fetches from backend endpoints
3. Backend returns `ApiResponse<T>` wrapper
4. Frontend extracts `data` property
5. Props passed to Client Components

## Development Workflow

1. Start MongoDB: `cd backend && docker-compose up -d`
2. Start Backend: `cd backend/src/MillionBackend.API && dotnet run`
3. Start Frontend: `cd frontend && npm run dev`
4. Access Swagger: http://localhost:5208/swagger
5. Access App: http://localhost:3000

## Important Notes

### Backend

- MongoDB connection requires authentication: `admin/admin123`
- Database name: `MillionTestDB`
- Indexes created automatically on startup
- CORS configured for frontend origin

### Frontend

- Server Components fetch data at build/request time
- Client Components marked with `"use client"`
- Dynamic routes use `Promise<params>` in Next.js 15
- Images from Unsplash configured in `next.config.ts`

## Common Tasks

### Add Backend Endpoint

1. Create/update model in `Core/Models`
2. Add method to repository interface in `Core/Repositories`
3. Implement in `Infrastructure/Repositories`
4. Add service method in `Application/Services`
5. Create DTO in `API/DTOs`
6. Add AutoMapper mapping
7. Create controller action in `API/Controllers`
8. Write tests in `Tests/`

### Add Frontend Page

1. Create route in `app/[route]/page.tsx`
2. Create components in `components/[feature]/`
3. Add API calls in `lib/api/`
4. Add types in `types/` or `lib/api/types.ts`
5. Add validation schemas in `lib/validations/`

## Performance Considerations

### Backend

- MongoDB indexes on: name, address, price, enabled, idOwner
- Pagination implemented (default 10 items)
- Async/await throughout
- Connection pooling via MongoDB driver

### Frontend

- Server-side rendering for initial load
- Image optimization with Next.js Image
- Code splitting automatic
- Static generation where possible

## Testing Strategy

### Backend Tests

- Unit tests for services (mock repositories)
- Unit tests for controllers (mock services)
- Repository tests for data access
- Run: `dotnet test` from backend directory

### Frontend Tests

- Component tests (to be added)
- E2E tests (to be added)
- Run: `npm test` from frontend directory

## Deployment Notes

- Backend: Deploy as standalone .NET application
- Frontend: Deploy to Vercel/Netlify or as Docker container
- MongoDB: Use MongoDB Atlas for production
- Environment variables must be configured in deployment platform

For detailed setup instructions, see `docs/setup/` directory.
