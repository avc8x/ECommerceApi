# ECommerce API

Multi-localization e-commerce backend built with **ASP.NET 10**, **EF Core**, **Redis**, **MediatR** (CQRS), and **FluentValidation** — following Domain-Driven Design.

---

## Architecture

```
ECommerceApi/
├── src/
│   ├── ECommerceApi.Domain/          # Entities, base types (no dependencies)
│   ├── ECommerceApi.Application/     # CQRS commands, queries, validators, DTOs
│   ├── ECommerceApi.Infrastructure/  # EF Core, Redis, Slug service
│   └── ECommerceApi.API/             # Controllers, middleware, Program.cs
```

### Key Patterns
| Pattern | Where |
|---|---|
| **DDD** | Domain entities with encapsulated logic |
| **CQRS** | Separate Command/Query classes per feature |
| **Mediator** | MediatR dispatches all commands/queries |
| **Result** | `Result<T>` / `Result` returned by all handlers — no thrown exceptions in business logic |
| **Pipeline Behavior** | `ValidationBehavior<T>` runs FluentValidation before every handler |
| **Redis Cache** | All read (GET) endpoints cache with prefix-based invalidation on writes |

---

## Setup

### Prerequisites
- .NET 10 SDK
- MS SQL Server (or any SQL Server-compatible engine)
- Redis (local or Docker)

### Quick start with Docker (Redis)
```bash
docker run -d -p 6379:6379 redis:alpine
```

### 1. Configure connection strings
Edit `src/ECommerceApi.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True",
    "Redis": "localhost:6379"
  }
}
```

### 2. Apply database migration
```bash
cd src/ECommerceApi.API
dotnet ef database update --project ../ECommerceApi.Infrastructure
```
Or in development mode, migrations auto-apply on startup.

### 3. Run
```bash
cd src/ECommerceApi.API
dotnet run
```

Swagger UI: `https://localhost:7000/swagger`

---

## API Endpoints

### Admin — Categories (`/api/admin/categories`)
| Method | Route | Description |
|---|---|---|
| GET | `/api/admin/categories` | Full category tree (all translations) |
| POST | `/api/admin/categories` | Create category |
| POST | `/api/admin/categories/{id}/{lang}` | Add/update translation |
| PUT | `/api/admin/categories` | Update category (image, parent) |
| DELETE | `/api/admin/categories/{id}` | Soft delete |

### Admin — Popular Categories (`/api/admin/popular-categories`)
| Method | Route | Description |
|---|---|---|
| GET | `/api/admin/popular-categories` | List all popular categories |
| POST | `/api/admin/popular-categories` | Add category (must be level-1) |
| PUT | `/api/admin/popular-categories/order` | Reorder |
| DELETE | `/api/admin/popular-categories/{id}` | Remove |

### Admin — Home Slides (`/api/admin/home-slides`)
| Method | Route | Description |
|---|---|---|
| GET | `/api/admin/home-slides` | All slides with all translations |
| POST | `/api/admin/home-slides` | Create slide |
| PUT | `/api/admin/home-slides` | Update slide (image, category) |
| PUT | `/api/admin/home-slides/{id}/{lang}` | Add/update translation |
| PUT | `/api/admin/home-slides/order` | Reorder slides |
| DELETE | `/api/admin/home-slides/{id}` | Soft delete |

### Client (public, language-scoped)
| Method | Route | Description |
|---|---|---|
| GET | `/api/categories/{lang}` | Localized category tree |
| GET | `/api/popular-categories/{lang}` | Localized popular categories |
| GET | `/api/home-slides/{lang}` | Localized home slides |

---

## Domain Details

### Category
- Hierarchical (unlimited depth via `ParentId`)
- SEO slug (unique, auto-generated from title)
- Soft delete (query filter hides deleted globally)
- Localized via `CategoryTranslation` (one row per language per category)

### PopularCategory
- Must be a **level-1** (root) category — validated in handler
- Ordered list (`DisplayOrder`)
- One entry per category (unique index on `CategoryId`)

### SwiperSlide
- Links to any category
- Manual ordering (`DisplayOrder`)
- 4 text fields per language: `TopText`, `BigTitle`, `HighlightedTitleNormal` (required), `HighlightedTitleColor` (required), `HighlightedTitleBold` (optional), `BottomText`

---

## Redis Caching Strategy
| Cache Key | Invalidated When |
|---|---|
| `categories:admin:tree` | Any category write |
| `categories:client:{lang}` | Any category write |
| `popular-categories:{lang}` | Any popular category write |
| `home-slides:admin` | Any slide write |
| `home-slides:{lang}` | Any slide write |

Cache TTL: **10 minutes** (configurable in handlers).

---

## Adding a New Language
No code changes needed — just POST to the translation endpoints with the new language code:
```
POST /api/admin/categories/{id}/ar
{ "title": "الإلكترونيات", "description": "..." }
```
