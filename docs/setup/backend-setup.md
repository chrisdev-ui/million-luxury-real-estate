# Backend Setup Guide

> Complete step-by-step guide to setting up the Million Luxury Backend API locally on your machine.

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Verification](#verification)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)
- [Development Workflow](#development-workflow)

---

## Prerequisites

Before starting, ensure you have the following installed on your machine:

### Required Software

| Software | Version | Download Link | Purpose |
|----------|---------|---------------|---------|
| **.NET SDK** | 9.0 or higher | [Download](https://dotnet.microsoft.com/download/dotnet/9.0) | Run and build the API |
| **Docker Desktop** | Latest | [Download](https://www.docker.com/products/docker-desktop) | Run MongoDB container |
| **Git** | Latest | [Download](https://git-scm.com/downloads) | Clone repository |
| **Code Editor** | Any | [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) | Edit code |

### Verify Installations

Open a terminal and run these commands to verify installations:

```bash
# Check .NET version
dotnet --version
# Expected output: 9.0.0 or higher

# Check Docker version
docker --version
# Expected output: Docker version 24.0.0 or higher

# Check Git version
git --version
# Expected output: git version 2.40.0 or higher
```

If any command fails, install the missing software from the links above.

---

## Installation Steps

### Step 1: Clone the Repository

Open a terminal and navigate to where you want to store the project:

```bash
# Clone the repository
git clone <repository-url>

# Navigate to the project root
cd MillionTest

# Navigate to the backend directory
cd backend
```

### Step 2: Restore .NET Dependencies

The backend uses NuGet packages that need to be downloaded:

```bash
# From the backend directory
dotnet restore
```

**What this does:**
- Downloads all required NuGet packages (MongoDB.Driver, AutoMapper, etc.)
- Restores project dependencies
- Prepares the project for building

**Expected output:**
```
Restore succeeded.
```

---

## Database Setup

The backend uses MongoDB as the database. We'll run it using Docker for easy setup.

### Step 1: Start MongoDB Container

From the `backend` directory:

```bash
# Start MongoDB using Docker Compose
docker-compose up -d
```

**What this does:**
- Downloads MongoDB Docker image (if not already downloaded)
- Starts MongoDB container in detached mode (-d flag)
- Creates a MongoDB instance accessible at `localhost:27017`
- Sets up authentication with username: `admin`, password: `admin123`

**Expected output:**
```
[+] Running 1/1
 ✔ Container mongodb-milliontest  Started
```

### Step 2: Verify MongoDB is Running

Check that the container is running:

```bash
# List running Docker containers
docker ps
```

**Expected output:**
```
CONTAINER ID   IMAGE         PORTS                      NAMES
abc123def456   mongo:latest  0.0.0.0:27017->27017/tcp   mongodb-milliontest
```

You should see `mongodb-milliontest` in the list.

### Step 3: Wait for MongoDB to Be Ready

MongoDB takes a few seconds to initialize. Wait 5-10 seconds before proceeding.

```bash
# Optional: Check MongoDB logs
docker logs mongodb-milliontest
```

Look for a line like: `Waiting for connections`

### Step 4: Seed the Database

Load sample data into MongoDB:

```bash
# From the backend directory
docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < seed-data-heavy.js
```

**What this does:**
- Executes the `seed-data-heavy.js` script inside the MongoDB container
- Creates the `MillionTestDB` database
- Creates collections: `owners`, `properties`, `propertyImages`, `propertyTraces`
- Inserts sample data (owners, properties, images, transaction history)

**Expected output:**
```
Current Mongosh Log ID: ...
Connecting to: mongodb://localhost:27017/...
Using MongoDB: 7.0.5
Using Mongosh: 2.1.1

Database created: MillionTestDB
Collections created successfully
Sample data inserted successfully
```

### Step 5: Verify Data Was Loaded

Connect to MongoDB and check the data:

```bash
# Connect to MongoDB shell
docker exec -it mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin

# Switch to the MillionTestDB database
use MillionTestDB

# Count properties
db.properties.countDocuments()
# Should return a number greater than 0 (e.g., 100)

# Count owners
db.owners.countDocuments()
# Should return a number greater than 0 (e.g., 20)

# Exit MongoDB shell
exit
```

---

## Configuration

### Step 1: Verify Connection String

The connection string is already configured in `appsettings.json`. Let's verify it:

```bash
# From the backend directory
cd src/MillionBackend.API
cat appsettings.json
```

**Expected content:**
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://admin:admin123@localhost:27017"
  },
  "MongoDBSettings": {
    "DatabaseName": "MillionTestDB"
  },
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Configuration Explained:**

| Setting | Value | Purpose |
|---------|-------|---------|
| `MongoDB` | `mongodb://admin:admin123@localhost:27017` | Connection string for MongoDB |
| `DatabaseName` | `MillionTestDB` | Name of the database |
| `AllowedHosts` | `*` | Allow requests from any host |

### Step 2: Environment-Specific Configuration (Optional)

For production, create `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://user:password@cluster.mongodb.net/"
  },
  "MongoDBSettings": {
    "DatabaseName": "MillionTestDB"
  }
}
```

**Do not commit production credentials to Git!**

---

## Running the Application

### Step 1: Build the Project

From the API directory (`backend/src/MillionBackend.API`):

```bash
# Build the project
dotnet build
```

**What this does:**
- Compiles the C# code
- Checks for syntax errors
- Prepares binaries for execution

**Expected output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Step 2: Run the API

```bash
# Run the API
dotnet run
```

**What this does:**
- Starts the Kestrel web server
- Loads configuration from `appsettings.json`
- Connects to MongoDB
- Creates database indexes
- Starts listening for HTTP requests

**Expected output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5208
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Step 3: Alternative - Watch Mode (Auto-Reload)

For development, use watch mode to automatically reload on code changes:

```bash
# Run with auto-reload
dotnet watch run
```

**Benefits:**
- Automatically rebuilds when you save code changes
- No need to manually restart the server
- Faster development workflow

---

## Verification

### Step 1: Access Swagger Documentation

Open your web browser and navigate to:

```
http://localhost:5208/swagger
```

**What you should see:**
- Interactive API documentation
- List of all endpoints
- Ability to test endpoints directly from the browser

### Step 2: Test the API

#### Test 1: Get All Properties

In Swagger:
1. Expand `GET /api/properties`
2. Click "Try it out"
3. Click "Execute"

**Expected response:**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "idProperty": "...",
        "name": "Luxury Villa",
        "address": "123 Ocean Drive",
        "price": 1500000,
        ...
      }
    ],
    "currentPage": 1,
    "totalPages": 10,
    "pageSize": 10,
    "totalCount": 100
  },
  "message": "Properties retrieved successfully"
}
```

#### Test 2: Filter Properties

1. Expand `GET /api/properties/filter`
2. Click "Try it out"
3. Enter parameters:
   - name: `Villa`
   - minPrice: `500000`
   - maxPrice: `2000000`
4. Click "Execute"

**Expected response:**
- Success: true
- Data: Array of properties matching the filter
- Results should only include properties with "Villa" in the name and price between $500k-$2M

#### Test 3: Get Single Property

1. Copy an `idProperty` from the previous response
2. Expand `GET /api/properties/{id}`
3. Click "Try it out"
4. Paste the property ID
5. Click "Execute"

**Expected response:**
- Success: true
- Data: Full property details including owner, images, and sales history

### Step 3: Test with cURL (Optional)

From a terminal:

```bash
# Get all properties
curl http://localhost:5208/api/properties

# Filter properties
curl "http://localhost:5208/api/properties/filter?name=Villa&minPrice=500000"

# Get property by ID (replace {id} with actual ID)
curl http://localhost:5208/api/properties/{id}
```

---

## Testing

### Run Unit Tests

From the backend directory:

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity detailed

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

**Expected output:**
```
Passed!  - Failed:     0, Passed:    18, Skipped:     0, Total:    18
```

**Test Categories:**
- ✅ Service tests (PropertyService, OwnerService)
- ✅ Controller tests (PropertiesController, OwnersController)
- ✅ Repository tests (PropertyRepository)

### Run Specific Test

```bash
# Run a specific test class
dotnet test --filter "FullyQualifiedName~PropertyServiceTests"

# Run a specific test method
dotnet test --filter "FullyQualifiedName~GetPropertyByIdAsync_ValidId_ReturnsProperty"
```

---

## Troubleshooting

### Problem 1: MongoDB Connection Failed

**Error:**
```
MongoDB.Driver.MongoAuthenticationException: Unable to authenticate
```

**Solutions:**

1. **Verify MongoDB is running:**
   ```bash
   docker ps | grep mongodb-milliontest
   ```

2. **Check MongoDB logs:**
   ```bash
   docker logs mongodb-milliontest
   ```

3. **Restart MongoDB:**
   ```bash
   docker-compose down
   docker-compose up -d
   ```

4. **Verify connection string in appsettings.json:**
   ```json
   {
     "ConnectionStrings": {
       "MongoDB": "mongodb://admin:admin123@localhost:27017"
     }
   }
   ```

### Problem 2: Port 5208 Already in Use

**Error:**
```
Failed to bind to address http://localhost:5208: address already in use
```

**Solutions:**

1. **Kill the process using the port:**

   **On macOS/Linux:**
   ```bash
   lsof -ti:5208 | xargs kill -9
   ```

   **On Windows:**
   ```cmd
   netstat -ano | findstr :5208
   taskkill /PID <PID> /F
   ```

2. **Change the port in `Properties/launchSettings.json`:**
   ```json
   {
     "applicationUrl": "http://localhost:5209"
   }
   ```

### Problem 3: .NET SDK Not Found

**Error:**
```
The command 'dotnet' could not be found
```

**Solution:**

1. Install .NET 9 SDK from https://dotnet.microsoft.com/download/dotnet/9.0
2. Verify installation:
   ```bash
   dotnet --version
   ```

### Problem 4: Docker Not Running

**Error:**
```
Cannot connect to the Docker daemon
```

**Solution:**

1. Start Docker Desktop
2. Wait for Docker to fully start (check Docker icon in system tray)
3. Verify Docker is running:
   ```bash
   docker ps
   ```

### Problem 5: Database Not Seeded

**Error:**
```
Properties collection is empty
```

**Solution:**

1. **Re-run the seed script:**
   ```bash
   cd backend
   docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < seed-data-heavy.js
   ```

2. **Verify data:**
   ```bash
   docker exec -it mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin
   use MillionTestDB
   db.properties.countDocuments()
   ```

### Problem 6: NuGet Restore Failed

**Error:**
```
Unable to load the service index for source
```

**Solution:**

1. **Clear NuGet cache:**
   ```bash
   dotnet nuget locals all --clear
   ```

2. **Restore again:**
   ```bash
   dotnet restore
   ```

3. **Check internet connection** - NuGet needs internet to download packages

---

## Development Workflow

### Daily Development Routine

#### 1. Start Your Day

```bash
# 1. Navigate to backend directory
cd backend

# 2. Start MongoDB
docker-compose up -d

# 3. Navigate to API project
cd src/MillionBackend.API

# 4. Run with auto-reload
dotnet watch run
```

#### 2. Make Changes

1. Edit code in your IDE (Visual Studio, VS Code, Rider)
2. Save the file
3. Watch mode automatically rebuilds and restarts the server
4. Test changes in Swagger: `http://localhost:5208/swagger`

#### 3. Run Tests

```bash
# In a new terminal, from backend directory
dotnet test
```

Run tests after every significant change to ensure nothing broke.

#### 4. End Your Day

```bash
# Stop the API (Ctrl+C in the terminal running dotnet watch)

# Stop MongoDB
cd backend
docker-compose down
```

### Git Workflow

#### Before Committing

```bash
# 1. Run tests
dotnet test

# 2. Build to check for errors
dotnet build

# 3. Format code (if using a formatter)
dotnet format

# 4. Stage changes
git add .

# 5. Commit
git commit -m "feat: add property filtering by price range"

# 6. Push
git push origin main
```

### Adding a New Endpoint

Follow these steps to add a new endpoint:

#### 1. Define the Interface (Core Layer)

```csharp
// Core/Repositories/IPropertyRepository.cs
Task<Property?> GetByCodeInternalAsync(string codeInternal);
```

#### 2. Implement the Repository (Infrastructure Layer)

```csharp
// Infrastructure/Repositories/PropertyRepository.cs
public async Task<Property?> GetByCodeInternalAsync(string codeInternal)
{
    return await _properties
        .Find(p => p.CodeInternal == codeInternal && p.Enabled)
        .FirstOrDefaultAsync();
}
```

#### 3. Add Service Method (Application Layer)

```csharp
// Application/Services/PropertyService.cs
public async Task<Property?> GetByCodeInternalAsync(string codeInternal)
{
    return await _propertyRepository.GetByCodeInternalAsync(codeInternal);
}
```

#### 4. Create Controller Endpoint (API Layer)

```csharp
// API/Controllers/PropertiesController.cs
[HttpGet("code/{codeInternal}")]
public async Task<ActionResult<ApiResponse<PropertyDto>>> GetByCodeInternal(string codeInternal)
{
    var property = await _propertyService.GetByCodeInternalAsync(codeInternal);

    if (property == null)
    {
        return NotFound(ApiResponse<object>.ErrorResult("Property not found"));
    }

    var propertyDto = _mapper.Map<PropertyDto>(property);
    return Ok(ApiResponse<PropertyDto>.SuccessResult(propertyDto));
}
```

#### 5. Write Tests

```csharp
// Tests/Services/PropertyServiceTests.cs
[Test]
public async Task GetByCodeInternalAsync_ValidCode_ReturnsProperty()
{
    // Arrange
    var property = new Property { CodeInternal = "LUX-001" };
    _mockRepository
        .Setup(r => r.GetByCodeInternalAsync("LUX-001"))
        .ReturnsAsync(property);

    // Act
    var result = await _service.GetByCodeInternalAsync("LUX-001");

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.CodeInternal, Is.EqualTo("LUX-001"));
}
```

#### 6. Test in Swagger

1. Run the API: `dotnet run`
2. Open Swagger: `http://localhost:5208/swagger`
3. Test the new endpoint

---

## Environment Variables

### Using Environment Variables (Production)

Instead of hardcoding connection strings, use environment variables:

#### 1. Set Environment Variables

**On macOS/Linux:**
```bash
export ConnectionStrings__MongoDB="mongodb+srv://user:password@cluster.mongodb.net/"
export MongoDBSettings__DatabaseName="MillionTestDB"
```

**On Windows:**
```cmd
set ConnectionStrings__MongoDB=mongodb+srv://user:password@cluster.mongodb.net/
set MongoDBSettings__DatabaseName=MillionTestDB
```

#### 2. Access in appsettings.json

The double underscore (`__`) in environment variables maps to nested JSON:

```
ConnectionStrings__MongoDB → ConnectionStrings:MongoDB
MongoDBSettings__DatabaseName → MongoDBSettings:DatabaseName
```

---

## Performance Monitoring

### Enable Detailed Logging

In `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "MongoDB.Driver": "Debug"
    }
  }
}
```

### Monitor MongoDB Queries

```bash
# Watch MongoDB logs in real-time
docker logs -f mongodb-milliontest
```

### Profile API Performance

Use browser DevTools Network tab or tools like:
- **Postman** - API testing with timing
- **curl with timing:**
  ```bash
  curl -w "@curl-format.txt" -o /dev/null -s http://localhost:5208/api/properties
  ```

---

## Next Steps

Congratulations! Your backend is now set up and running. 🎉

### What's Next?

1. **[Setup Frontend](./frontend-setup.md)** - Connect the React/Next.js frontend
2. **[API Architecture](../api/architecture.md)** - Understand the backend design
3. **[Explore Swagger](http://localhost:5208/swagger)** - Test all API endpoints
4. **[Run Tests](../../backend/)** - Ensure everything works: `dotnet test`

### Learning Resources

- **.NET Documentation:** https://docs.microsoft.com/en-us/dotnet/
- **MongoDB C# Driver:** https://www.mongodb.com/docs/drivers/csharp/
- **Clean Architecture:** https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- **REST API Best Practices:** https://restfulapi.net/

### Need Help?

- Check [Troubleshooting](#troubleshooting) section
- Review error logs in the terminal
- Open an issue in the repository
- Contact the development team

---

**Happy Coding! 🚀**
