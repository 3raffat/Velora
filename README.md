# Velora — E-Commerce Backend

Velora is a modular e-commerce platform built with a **microservices architecture** using **.NET 10**, **Clean Architecture**, and **Domain-Driven Design (DDD)** principles. The backend is composed of two independently deployable services that communicate over HTTP, each owning its own database and domain logic.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Technology Stack](#technology-stack)
- [Services](#services)
  - [OrderService](#orderservice)
  - [DeliveryService](#deliveryservice)
- [Project Structure](#project-structure)
- [API Reference](#api-reference)
  - [OrderService API Endpoints](#orderservice-api-endpoints)
  - [DeliveryService API Endpoints](#deliveryservice-api-endpoints)
- [Domain Models](#domain-models)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Setup](#database-setup)
  - [Running the Services](#running-the-services)
- [Configuration](#configuration)
- [Authentication & Authorization](#authentication--authorization)
- [Key Design Decisions](#key-design-decisions)

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                      API Gateway / Client                │
└────────────────┬────────────────────────┬────────────────┘
                 │                        │
       ┌─────────▼─────────┐    ┌────────▼────────────┐
       │   OrderService    │    │  DeliveryService     │
       │   (Port 5001)     │◄──►│  (Port 7165)         │
       └─────────┬─────────┘    └────────┬────────────┘
                 │                        │
       ┌─────────▼─────────┐    ┌────────▼────────────┐
       │   SQL Server      │    │   SQL Server         │
       │   Velora DB       │    │   Velora.Delivery DB │
       └───────────────────┘    └─────────────────────┘
```

Each service follows **Clean Architecture** with four layers:

```
Service/
├── Service.Api              → Controllers, Middleware, DI, Swagger
├── Service.Application      → CQRS Handlers, Validators, DTOs
├── Service.Domain           → Entities, Value Objects, Enums, Domain Events
└── Service.Infrastructure   → EF Core, Identity, JWT, Email, External APIs
```

---

## Technology Stack

| Category              | Technology                                               |
|-----------------------|----------------------------------------------------------|
| **Framework**         | .NET 10 / ASP.NET Core 10                                |
| **Language**          | C# 13                                                    |
| **Architecture**      | Clean Architecture, Domain-Driven Design (DDD)           |
| **CQRS / Mediator**   | MediatR 14.x                                             |
| **Validation**        | FluentValidation 12.x                                    |
| **ORM**              | Entity Framework Core 10                                 |
| **Database**         | Microsoft SQL Server                                     |
| **Authentication**   | ASP.NET Core Identity + JWT Bearer Tokens                |
| **API Versioning**   | Asp.Versioning (URL Segment)                             |
| **API Documentation** | Swagger / Swashbuckle                                    |
| **Background Jobs**  | Hangfire (OrderService)                                  |
| **Email**            | MailKit / MimeKit (SMTP)                                 |
| **Payments**         | PayPal REST API (Sandbox)                                |
| **Containerization** | Docker (multi-stage build)                               |

---

## Services

### OrderService

The **OrderService** is the core commerce engine. It manages the full lifecycle of products, customers, shopping carts, orders, payments, cancellations, and refunds.

**Responsibilities:**
- Customer registration, login, email confirmation, and profile management
- Product catalog (CRUD) and category management
- Shopping cart operations (add, remove, clear)
- Order checkout with promo code / coupon support
- Payment processing (PayPal integration + Cash on Delivery)
- Order status transitions: `Pending → Confirmed → Shipped → Delivered`
- Order cancellation requests (request → approve / reject workflow)
- Refund management (approve → complete / reject workflow)
- Admin seeding on startup
- Background job scheduling via Hangfire

**Domain Entities:**
- `Order` — with `OrderItem`, `Payment`, `Cancellation`, `Refund`
- `Customer` — with `Address` (shipping/billing)
- `Product` — with `Category`
- `ShoppingCart` — with `CartItem`
- `Coupon`

---

### DeliveryService

The **DeliveryService** handles last-mile delivery logistics. It is a standalone service with its own identity system, database, and user roles.

**Responsibilities:**
- Shipment creation (triggered by OrderService when an order is shipped)
- Driver assignment by Dispatchers/Admins
- Step-by-step delivery status progression by Drivers
- Delivery attempt tracking with a **3-attempt-per-driver limit**
- Automatic driver unassignment after 3 failed attempts (reset to Pending)
- User management (register, activate, deactivate) by DeliveryAdmin
- Cross-service communication with OrderService (mark order as delivered)

**Domain Entities:**
- `Shipment` — core aggregate with tracking number, address, status, driver assignment
- `DeliveryAttempt` — tracks each failed delivery with driver reference and reason

**Shipment Status Flow:**
```
Pending → Assigned → PickedUp → InTransit → Delivered
                                          ↘ Failed → Retry (≤3) → Assigned
                                                   → Retry (>3) → Pending (new driver)
                                                                  ↘ Cancelled
```

---

## Project Structure

```
Velora/
├── services/
│   ├── OrderService/
│   │   ├── OrderService.slnx
│   │   └── src/
│   │       ├── OrderService.Api/
│   │       │   ├── Controllers/         # REST API Controllers
│   │       │   ├── Contracts/           # Request/Response DTOs
│   │       │   ├── Middleware/          # Global Exception Handler
│   │       │   ├── Services/           # CurrentUser, etc.
│   │       │   ├── Program.cs          # Application entry point
│   │       │   ├── DependencyInjection.cs
│   │       │   ├── Dockerfile
│   │       │   └── appsettings.*.json
│   │       ├── OrderService.Application/
│   │       │   ├── Features/           # CQRS Commands & Queries
│   │       │   │   ├── Auth/
│   │       │   │   ├── Products/
│   │       │   │   ├── Categories/
│   │       │   │   ├── Customers/
│   │       │   │   ├── Addresses/
│   │       │   │   ├── ShoppingCarts/
│   │       │   │   └── Orders/
│   │       │   └── Common/             # Behaviors, Interfaces, DTOs
│   │       ├── OrderService.Domain/
│   │       │   └── Entities/           # Aggregates, Value Objects, Events
│   │       │       ├── Orders/
│   │       │       ├── Products/
│   │       │       ├── Customers/
│   │       │       ├── ShoppingCart/
│   │       │       └── Coupons/
│   │       └── OrderService.Infrastructure/
│   │           ├── Data/               # EF Core DbContext, Migrations
│   │           ├── Services/           # JWT, Email, PayPal, Identity
│   │           └── Repositories/
│   │
│   └── DeliveryService/
│       └── src/
│           ├── DeliveryService.Api/
│           │   ├── Controllers/
│           │   ├── Contracts/
│           │   ├── Middleware/
│           │   ├── Services/
│           │   ├── Program.cs
│           │   ├── DependencyInjection.cs
│           │   └── appsettings.*.json
│           ├── DeliveryService.Application/
│           │   └── Features/
│           │       ├── Auth/
│           │       ├── Shipments/
│           │       ├── DeliveryAttempts/
│           │       └── Users/
│           ├── DeliveryService.Domain/
│           │   └── Entities/
│           │       └── Shipments/      # Shipment, DeliveryAttempt
│           └── DeliveryService.Infrastructure/
│               ├── Data/
│               ├── Services/
│               └── Repositories/
```

---

## API Reference

All endpoints are versioned under `/api/v1/` using URL segment versioning.

### OrderService API Endpoints

#### Auth (`/api/v1/auth`)

| Method | Endpoint                          | Auth     | Description                          |
|--------|-----------------------------------|----------|--------------------------------------|
| POST   | `/auth/register`                  | Public   | Register a new customer account      |
| POST   | `/auth/login`                     | Public   | Authenticate and receive JWT token   |
| GET    | `/auth/confirm-email/{userId}`    | Public   | Confirm email with token             |

#### Products (`/api/v1/products`)

| Method | Endpoint                 | Auth     | Description                              |
|--------|--------------------------|----------|------------------------------------------|
| GET    | `/products`              | Public   | List products (filter by category/search)|
| GET    | `/products/{id}`         | Public   | Get product by ID                        |
| POST   | `/products`              | Admin    | Create a new product                     |
| PUT    | `/products/{id}`         | Admin    | Update product details                   |
| PATCH  | `/products/{id}/price`   | Admin    | Update product price                     |
| PATCH  | `/products/{id}/stock`   | Admin    | Adjust product stock quantity            |
| DELETE | `/products/{id}`         | Admin    | Delete a product                         |

#### Categories (`/api/v1/categories`)

| Method | Endpoint              | Auth     | Description               |
|--------|-----------------------|----------|---------------------------|
| GET    | `/categories`         | Public   | List all categories       |
| GET    | `/categories/{id}`    | Public   | Get category by ID        |
| POST   | `/categories`         | Auth     | Create a new category     |
| PUT    | `/categories/{id}`    | Auth     | Update a category         |

#### Customers (`/api/v1/customers`)

| Method | Endpoint                         | Auth     | Description                     |
|--------|----------------------------------|----------|---------------------------------|
| POST   | `/customers/me/complete-profile` | Auth     | Complete customer profile       |
| GET    | `/customers/me`                  | Auth     | Get current customer profile    |
| GET    | `/customers/{id}`                | Auth     | Get customer by ID              |
| GET    | `/customers/addresses`           | Auth     | List customer addresses         |
| GET    | `/customers/addresses/{id}`      | Auth     | Get address by ID               |
| POST   | `/customers/addresses`           | Auth     | Add a new address               |
| PUT    | `/customers/{id}/addresses`      | Auth     | Update an address               |
| DELETE | `/customers/{id}/addresses`      | Auth     | Delete an address               |

#### Shopping Cart (`/api/v1/carts`)

| Method | Endpoint                     | Auth     | Description               |
|--------|------------------------------|----------|---------------------------|
| GET    | `/carts/my-cart`             | Auth     | Get active shopping cart   |
| POST   | `/carts/items`               | Auth     | Add item to cart           |
| DELETE | `/carts/{cartId}/items`      | Auth     | Remove item from cart      |
| PUT    | `/carts/{cartId}/clear`      | Auth     | Clear all cart items       |

#### Orders (`/api/v1/orders`)

| Method | Endpoint                                          | Auth       | Description                        |
|--------|---------------------------------------------------|------------|------------------------------------|
| POST   | `/orders/checkout`                                | Auth       | Checkout cart → create order       |
| POST   | `/orders/{orderId}/payments/paypal/authorize`     | Auth       | Authorize PayPal payment           |
| GET    | `/orders/{orderId}`                               | Auth       | Get order by ID                    |
| GET    | `/orders/{orderId}/shipment`                      | User/Admin | Get shipment tracking for order    |
| GET    | `/orders`                                         | User/Admin | Get current customer's orders      |
| GET    | `/orders/all`                                     | Admin      | Get all orders (admin)             |
| PUT    | `/orders/{orderId}/ship`                          | Admin      | Mark order as shipped              |
| PUT    | `/orders/{orderId}/deliver`                       | Admin      | Mark order as delivered            |

#### Cancellations (`/api/v1/cancellations`)

| Method | Endpoint                                        | Auth     | Description                          |
|--------|-------------------------------------------------|----------|--------------------------------------|
| POST   | `/cancellations/{orderId}/cancellation/request` | Auth     | Request order cancellation           |
| GET    | `/cancellations/{orderId}/cancellation`         | Auth     | Get cancellation details             |
| PUT    | `/cancellations/{orderId}/cancellation/approve` | Auth     | Approve cancellation request         |
| PUT    | `/cancellations/{orderId}/cancellation/reject`  | Auth     | Reject cancellation request          |

#### Refunds (`/api/v1/refunds`)

| Method | Endpoint                                | Auth     | Description                    |
|--------|-----------------------------------------|----------|--------------------------------|
| PUT    | `/refunds/{orderId}/refund/approve`     | Auth     | Approve refund                 |
| PUT    | `/refunds/{orderId}/refund/complete`    | Auth     | Complete refund (transaction)  |
| PUT    | `/refunds/{orderId}/refund/reject`      | Auth     | Reject refund                  |

---

### DeliveryService API Endpoints

#### Auth (`/api/v1/auth`)

| Method | Endpoint                            | Auth           | Description                       |
|--------|-------------------------------------|----------------|-----------------------------------|
| POST   | `/auth/register`                    | DeliveryAdmin  | Register a new delivery user      |
| POST   | `/auth/login`                       | Public         | Authenticate and receive JWT      |
| GET    | `/auth/users`                       | Admin/Dispatch | List users (filter by role)       |
| PATCH  | `/auth/users/{userId}/activate`     | DeliveryAdmin  | Activate a user account           |
| PATCH  | `/auth/users/{userId}/deactivate`   | DeliveryAdmin  | Deactivate a user account         |

#### Shipments (`/api/v1/shipments`)

| Method | Endpoint                              | Auth           | Description                              |
|--------|---------------------------------------|----------------|------------------------------------------|
| POST   | `/shipments`                          | Public         | Create a shipment (called by OrderService)|
| GET    | `/shipments`                          | Auth           | List shipments (with filters)            |
| GET    | `/shipments/order/{orderId}`          | Public         | Get shipment by order ID                 |
| GET    | `/shipments/mine`                     | Driver         | Get driver's assigned shipments          |
| GET    | `/shipments/{shipmentId}/attempts`    | Auth           | Get delivery attempts for a shipment     |
| PATCH  | `/shipments/{shipmentId}/driver`      | Dispatch/Admin | Assign a driver to a shipment            |
| PATCH  | `/shipments/{shipmentId}/status`      | Driver         | Update shipment status                   |

---

## Domain Models

### OrderService — Order Lifecycle

```
                      ┌──────────┐
                      │ Pending  │ ← Cart Checkout
                      └────┬─────┘
                           │ Confirm()
                      ┌────▼─────┐
                      │Confirmed │
                      └────┬─────┘
                           │ Ship()
                      ┌────▼─────┐
                      │ Shipped  │ ← Creates Shipment in DeliveryService
                      └────┬─────┘
                           │ Deliver()
                      ┌────▼─────┐
                      │Delivered │ ← COD Payment auto-completed
                      └──────────┘

         Cancel() can be called from Pending or Confirmed
```

**Payment Methods:**
- `PayPal` — Online payment via PayPal REST API (Sandbox)
- `CashOnDelivery` — Payment collected upon delivery, auto-completed on `Deliver()`

### DeliveryService — Shipment Lifecycle

```
  Pending ──► Assigned ──► PickedUp ──► InTransit ──► Delivered ✓
     ▲                                      │
     │                                      ▼
     │                                    Failed
     │                                      │
     │                    ┌─────────────────┤
     │                    │ attempts < 3    │ attempts >= 3
     │                    ▼                 ▼
     │               Assigned          Pending (DriverId = null)
     │             (same driver)       (awaiting new driver)
     └──────────────────────────────────────┘
```

**3-Attempt Driver Limit:**
When a driver accumulates 3 failed delivery attempts on a single shipment, the system automatically:
1. Unassigns the current driver (`DriverId = null`)
2. Resets shipment status to `Pending`
3. The shipment returns to the dispatcher queue for reassignment to a new driver

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/) (LocalDB, Express, or Developer Edition)
- A code editor (Visual Studio 2022+, VS Code, or Rider)

### Database Setup

The services use **Entity Framework Core** with SQL Server. Update the connection strings in the respective `appsettings.Local.json` files:

**OrderService** (`services/OrderService/src/OrderService.Api/appsettings.Local.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=Velora;User ID=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

**DeliveryService** (`services/DeliveryService/src/DeliveryService.Api/appsettings.Local.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Velora.DeliveryService;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Apply EF Core migrations:

```bash
# OrderService
cd services/OrderService/src/OrderService.Api
dotnet ef database update

# DeliveryService
cd services/DeliveryService/src/DeliveryService.Api
dotnet ef database update
```

### Running the Services

Run each service independently:

```bash
# Terminal 1 — OrderService
cd services/OrderService/src/OrderService.Api
dotnet run

# Terminal 2 — DeliveryService
cd services/DeliveryService/src/DeliveryService.Api
dotnet run
```

Once running, access the Swagger UI:
- **OrderService**: `https://localhost:{port}/swagger`
- **DeliveryService**: `https://localhost:7165/swagger`
- **Hangfire Dashboard** (OrderService): `https://localhost:{port}/jobs`

---

## Configuration

### OrderService (`appsettings.Local.json`)

| Section              | Description                                       |
|----------------------|---------------------------------------------------|
| `ConnectionStrings`  | SQL Server connection string                      |
| `JwtSettings`        | JWT signing key, issuer, audience, expiration      |
| `EmailSettings`      | SMTP host, port, credentials for email delivery   |
| `DeliveryService`    | Base URL of the DeliveryService for HTTP calls    |
| `PayPal`             | PayPal Sandbox API credentials                    |
| `SeedAdmin`          | Default admin account seeded on first startup     |

### DeliveryService (`appsettings.Local.json`)

| Section              | Description                                       |
|----------------------|---------------------------------------------------|
| `ConnectionStrings`  | SQL Server connection string                      |
| `JwtSettings`        | JWT signing key, issuer, audience, expiration      |
| `IdentitySeed`       | Default driver account seeded on first startup    |
| `OrderService`       | Base URL of the OrderService for HTTP callbacks   |

---

## Authentication & Authorization

Both services use **ASP.NET Core Identity** with **JWT Bearer** token authentication.

### OrderService Roles

| Role    | Capabilities                                                                                 |
|---------|----------------------------------------------------------------------------------------------|
| `User`  | Browse products, manage cart, checkout, manage profile/addresses, view orders, request cancellation |
| `Admin` | All User capabilities + manage products/categories, view all orders, ship orders, manage cancellations/refunds |

### DeliveryService Roles

| Role             | Capabilities                                                                        |
|------------------|-------------------------------------------------------------------------------------|
| `Driver`         | View assigned shipments (`/mine`), update shipment status (pick up → transit → deliver/fail) |
| `Dispatcher`     | View all shipments, assign drivers to shipments, view user directory                 |
| `DeliveryAdmin`  | All Dispatcher capabilities + register/activate/deactivate user accounts             |

---

## Key Design Decisions

1. **Microservices Separation**: OrderService and DeliveryService are fully independent with separate databases, identity systems, and deployment pipelines. They communicate via HTTP REST calls.

2. **Clean Architecture**: Each service strictly separates concerns across four layers (Api → Application → Domain → Infrastructure) with dependencies pointing inward.

3. **CQRS with MediatR**: Commands and Queries are separated using the MediatR library, with a `ValidationBehavior` pipeline for automatic FluentValidation.

4. **Rich Domain Model (DDD)**: Business rules are encapsulated in domain entities (e.g., `Order.Ship()`, `Shipment.MarkFailed()`). State transitions are enforced with guard clauses and domain exceptions.

5. **Domain Events**: Key state changes raise domain events (e.g., `ShipmentDeliveredEvent`) for cross-cutting concerns and inter-service communication.

6. **3-Attempt Driver Limit**: The delivery domain enforces a maximum of 3 failed attempts per driver per shipment. On the 3rd failure + retry, the driver is automatically unassigned and the shipment returns to the dispatch queue.

7. **Standardized API Responses**: All endpoints return a consistent `StandardSuccessResponse<T>` wrapper with status code and message.

8. **URL-Segment API Versioning**: APIs are versioned via URL (`/api/v1/...`) for explicit, clear versioning.

---

## License

This project is for educational and portfolio purposes.
