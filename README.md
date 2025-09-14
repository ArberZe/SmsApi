
---

## Design Decisions

- **Minimal API + Layered Architecture**  
  Using minimal APIs keeps the HTTP layer lightweight, while separating logic into repositories and services ensures maintainability. This is inspired by Uncle Bob's clean architecture, and
  is a way how I structure every one of my projects, and I think that every piece of code belongs to its place.

A few key things to mention that I used in this project:
- **Dependency Injection (DI)**  
  - `AddDbContext` for EF Core with an in-memory database.
  - `AddScoped` for repositories (per-request lifetime).
  - `AddHttpClient<IMessageService, MessageService>` for typed HTTP client with configuration validation.

- **Configuration Binding**  
  `SmsServiceOptions` is bound from `appsettings.json` (`SmsService:BaseUrl` section).  
  The app fails fast if the URL is missing or empty.

- **Global Error Handling**  
  A custom `ErrorHandlingMiddleware` catches unhandled exceptions and converts them to consistent JSON error responses with HTTP status codes and trace IDs.

- **Validation in Service Layer**  
  `MessageService` validates input (recipient number, message text) and throws `ArgumentException` when invalid — caught by middleware and returned as a `400 Bad Request`.

---

## 🔧 Running the App

```bash
dotnet run
