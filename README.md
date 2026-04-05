# 🌍 NZWalks API

## 📌 Overview
NZWalks is an ASP.NET Web API that provides endpoints for managing walking tracks, regions, and difficulty levels in New Zealand. It allows users to perform CRUD operations and explore walking data efficiently.

## 🚀 Features
- CRUD operations for Walks
- Manage Regions
- Manage Difficulty levels
- RESTful API design
- Authentication & Authorization 
- Pagination / Filtering / Sorting
- Upload Files


## 🛠️ Tech Stack
- **Backend:** ASP.NET Core Web API  
- **Language:** C#  
- **Database:** SQL Server  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT  
- **Tools:** Swagger, Postman

# 📡 API Endpoints
### 🔹 Auth
#### POST: /api/Auth/Register
#### POST: /api/Auth/Login

### 🔹 Walks
#### GET: /api/Walks?filterOn=Name&FilterQuery=Track&sortBy=Name&isAscending=true&PageNumber=1&pageSaze=5
#### GET /api/walks/{id}
#### POST /api/walks
#### PUT /api/walks/{id}
#### DELETE /api/walks/{id}

### 🔹 Regions
#### GET /api/regions
#### GET /api/regions/{id}
#### POST /api/regions
#### PUT: /api/Regions/{id}
#### DELETE: /api/Regions/{id}

### 🔹 Images
#### POST: api/Image/Upload

# 🔐 Authentication

This API uses JWT authentication. Include the token in the Authorization header:
```bash
Authorization: Bearer <token>
```

---

## ⚙️ Setup & Installation

### 1. Clone the repository
```bash
git clone https://github.com/ammar-333/NZWalks.git
cd NZWalks
```

###  2. Configure Database

Update your connection string in:
```bash
appsettings.json
```

### 3. Apply Migrations

Using Package Manager Console (PMC):
```bash
Add-Migration InitialCreate
Update-Database
```

### 4. Run the project
```bash
dotnet run
```

---

# 👨‍💻 Author

#### Ammar Shaban
#### GitHub: https://github.com/ammar-333
