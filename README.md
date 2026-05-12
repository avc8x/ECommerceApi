# ECommerce API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Redis](https://img.shields.io/badge/Redis-Cache-DC382D?style=for-the-badge&logo=redis)
![React](https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

A full-stack multi-localization e-commerce platform built with **ASP.NET 10**, **React**, **Redis**, and **SQL Server** — following **Domain-Driven Design**, **CQRS**, and **Clean Architecture** principles.

</div>

---

## Screenshots

> Swagger API Documentation

![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat&logo=swagger&logoColor=black)

> React Frontend — matches the e-commerce reference design with swiper, popular categories, navbar, and footer.

---

## Architecture

```
ECommerceApi/
├── src/
│   ├── ECommerceApi.Domain/          # Entities, base types — zero dependencies
│   ├── ECommerceApi.Application/     # CQRS, MediatR handlers, DTOs, validators
│   ├── ECommerceApi.Infrastructure/  # EF Core, Redis, Slug service
│   └── ECommerceApi.API/             # Controllers, Swagger, middleware
└── frontend/                         # React 18 frontend
    ├── src/
    │   ├── components/               # TopBar, Navbar, Swiper, Categories, Footer
    │   ├── api.js                    # API calls to backend
    │   └── App.js
    └── public/
```

### Patterns Used
| Pattern | Implementation |
|---|---|
| **DDD** | Domain entities with no external dependencies |
| **CQRS** | Separate Command / Query classes per feature |
| **Mediator** | MediatR dispatches all commands and queries |
| **Result** | `Result<T>` returned by all handlers — no thrown exceptions |
| **Pipeline Behavior** | FluentValidation runs automatically before every handler |
| **Dependency Injection** | Interfaces for DbContext, Cache, Slug service |
| **Soft Delete** | Global query filters hide deleted records |

---

## Tech Stack

### Backend
- **ASP.NET 10** — Web API framework
- **Entity Framework Core 9** — ORM with SQL Server
- **MediatR** — CQRS + Mediator pattern
- **FluentValidation** — Request validation
- **StackExchange.Redis** — Caching layer
- **Swashbuckle** — Swagger / OpenAPI documentation

### Frontend
- **React 18** — UI framework
- **CSS Modules** — Component-scoped styling
- **Fetch API** — HTTP calls to backend

### Infrastructure
- **SQL Server** — Primary database
- **Redis** — Caching (with NoOp fallback if unavailable)
- **Docker** — Local development containers

---

## Domain Models

### Category
- Hierarchical structure (unlimited depth via `ParentId`)
- SEO-friendly unique slug (auto-generated)
- Soft delete with global query filter
- Multi-language via `CategoryTranslation`

### PopularCategory
- Selected ordered list of root-level (level 1) categories
- Shown on the home page
- Manual ordering via `DisplayOrder`

### SwiperSlide
- Home page banner slides
- Links to any category
- Manual ordering
- 6 text fields per language: `TopText`, `BigTitle`, `HighlightedTitleNormal`, `HighlightedTitleColor`, `HighlightedTitleBold` (optional), `BottomText`

---

## API Endpoints

### Admin — Categories
| Method | Route | Description |
|---|---|---|
| `GET` | `/api/admin/categories` | Full category tree (all translations) |
| `POST` | `/api/admin/categories` | Create category |
| `POST` | `/api/admin/categories/{id}/translations/{lang}` | Add/update translation |
| `PUT` | `/api/admin/categories` | Update category |
| `DELETE` | `/api/admin/categories/{id}` | Soft delete |

### Admin — Popular Categories
| Method | Route | Description |
|---|---|---|
| `GET` | `/api/admin/popular-categories` | List all |
| `POST` | `/api/admin/popular-categories` | Add (root-level only) |
| `PUT` | `/api/admin/popular-categories/order` | Reorder |
| `DELETE` | `/api/admin/popular-categories/{id}` | Remove |

### Admin — Home Slides
| Method | Route | Description |
|---|---|---|
| `GET` | `/api/admin/home-slides` | All slides |
| `POST` | `/api/admin/home-slides` | Create slide |
| `PUT` | `/api/admin/home-slides/{id}` | Update slide |
| `PUT` | `/api/admin/home-slides/{id}/translations/{lang}` | Update translation |
| `PUT` | `/api/admin/home-slides/order` | Reorder |
| `DELETE` | `/api/admin/home-slides/{id}` | Delete |

### Client (Public)
| Method | Route | Description |
|---|---|---|
| `GET` | `/api/categories/{lang}` | Localized category tree |
| `GET` | `/api/popular-categories/{lang}` | Localized popular categories |
| `GET` | `/api/home-slides/{lang}` | Localized home slides |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org)
- [Docker](https://docker.com) (for SQL Server & Redis)

### 1. Start Database & Redis
```bash
# Redis
docker run -d -p 6379:6379 redis:alpine

# SQL Server
docker run -d -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Configure Connection Strings
Edit `src/ECommerceApi.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ECommerceDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True",
    "Redis": "localhost:6379"
  }
}
```

### 3. Run the Backend
```bash
cd src/ECommerceApi.API
dotnet run
```
API runs at: `https://localhost:5001`  
Swagger UI: `https://localhost:5001/swagger`

### 4. Run the Frontend
```bash
cd frontend
npm install
npm start
```
Frontend runs at: `http://localhost:3000`

---

## Caching Strategy

| Cache Key | Invalidated When |
|---|---|
| `categories:admin:tree` | Any category write |
| `categories:client:{lang}` | Any category write |
| `popular-categories:{lang}` | Any popular category write |
| `home-slides:admin` | Any slide write |
| `home-slides:{lang}` | Any slide write |

Cache TTL: **10 minutes**. Falls back to **NoOpCacheService** if Redis is unavailable.

---

## Adding a New Language

No code changes needed. Just call the translation endpoints:

```bash
# Add Arabic translation for a category
POST /api/admin/categories/{id}/translations/ar
{
  "title": "الإلكترونيات",
  "description": "..."
}
```

---

## Author

**Muhammad Khairallah**  
[GitHub](https://github.com/avc8x) · [LinkedIn](https://linkedin.com/in/muhammad-khairallah-a94562338)
