# 📸 Instagram Clone API

A scalable and secure RESTful API inspired by Instagram, built with **ASP.NET Core** using modern backend development practices.

## 🚀 Features

- 🔐 Authentication & Authorization with JWT
- 👤 ASP.NET Core Identity Integration
- 📧 Email OTP Verification
- 🔄 Refresh Token Support
- ⚡ In-Memory Caching
- 📝 CRUD Operations for Posts
- ❤️ Like & Unlike Posts
- 💬 Comment Management
- 👥 Follow & Unfollow Users
- 🔍 User Search
- 🖼️ Image Upload Support
- 📑 DTO-Based API Responses
- 📚 Swagger API Documentation

---

## 🛠️ Tech Stack

- ASP.NET Core Web API
- C#
- ASP.NET Core Identity
- Entity Framework Core
- My SQL
- JWT Authentication
- Memory Cache
- AutoMapper
- DTO Pattern
- Swagger / OpenAPI

---

## 📂 Project Structure

```
InstagramAPI
│
├── Controllers
├── DTOs
├── Entities
├── Services
├── Repositories
├── Interfaces
├── Data
├── Migrations
├── Helpers
├── Middleware
├── Program.cs
└── appsettings.json
```

---

## ⚙️ Getting Started

### Clone the repository

```bash
git clone https://github.com/l797l/instagram.git
```

### Navigate to the project

```bash
cd instagram
```

### Configure the database

Update the connection string in:

```json
appsettings.json
```

### Apply migrations

```bash
dotnet ef database update
```

### Run the application

```bash
dotnet run
```

The API will be available at:

```
https://localhost:5001
```

Swagger:

```
https://localhost:5001/swagger
```

---

## 📖 API Modules

- Authentication
- User Management
- Posts
- Comments
- Likes
- Follow System
- OTP Verification

---

## 🔒 Security

- JWT Bearer Authentication
- ASP.NET Core Identity
- Password Hashing
- Email OTP Verification
- Authorization Policies

---

## ⚡ Performance

- In-Memory Caching
- DTO Mapping
- Optimized Entity Framework Core Queries

---

## 🚧 Future Improvements

- Cloud Image Storage
- Real-time Notifications
- Direct Messaging
- Redis Cache
- Docker Support
- CI/CD Pipeline

---

## 👨‍💻 Author

**Ali Abdul Mahdi**

- GitHub: https://github.com/l797l
- LinkedIn: https://www.linkedin.com/in/ali-abdul-al-mahdi-a5b9a2354
