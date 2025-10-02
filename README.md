# Product API Demo

This repository contains a .NET 8 Web API demo project.
It showcases JWT authentication with roles, API versioning, and Swagger UI documentation.
The application is designed purely as a backend API and demonstrates several concepts such as lazy loading with pagination, filtering, and sorting, as well as asynchronous processing using in-memory queues.

# Features

**API with Swagger UI** documentation

**JWT authentication** with role-based authorization

**API versioning** (v1 and v2)

**Product management** endpoints (list, create, update quantity)

**Seeder** for initial database setup:

- Roles

- Products

- One admin user (Admin / Admin@123)

**Version 2 extras:**

- Lazy loading product list with pagination, filtering, and sorting

- Asynchronous product quantity update using Channel (in-memory queue)

⚠️ Note:
This demo is not a full user management system. It does not provide UI or endpoints for role/user administration — it only demonstrates JWT authentication/authorization.

# Prerequisites

Before running the application, ensure you have installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server (local or remote instance)](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022 or Visual Studio Code (optional, but recommended)](https://visualstudio.microsoft.com/)

The connection string to the database can be configured in appsettings.json.

# How to Run
**Option 1 – Using Visual Studio**

1. Open the solution in Visual Studio.

2. Set the API project as the startup project.

3. Press **F5** to run.

**Option 2 – Using .NET CLI**

`dotnet build`

`dotnet run --project ./ProductApiDemo`

# Running Unit Tests
**Option 1 – Visual Studio Test Explorer**

- Open the solution and run tests directly from the Test Explorer.

**Option 2 – .NET CLI**

`dotnet test`

# Testing

**Authorization**

Some endpoints are public (`GetProductsList`, `GetProductDetail`).

Others require authentication (`UpdateProductQty`, `CreateProduct`).

Use the seeded admin user credentials:

Login: `Admin`  

Password: `Admin@123`  

**Lazy Loading (API v2)**

The API provides lazy loading with pagination, filtering, and sorting.

- Required parameters:

  - first → starting index

  - rows → number of records per page

- Optional parameters:

  - sortField → property name to sort by

  - sortOrder → 1 (asc) or -1 (desc)

  - multiSort → secondary sorting rules

  - filters → object with field-specific filter definitions

  - globalFilter → applies search across all available properties

**Supported filter match modes:**

- `startsWith`

- `contains`

- `notContains`

- `endsWith`

- `equals / is`

- `notEquals`

- `dateIs, dateIsNot, dateAfter, dateBefore`

- `gt, lt`

- `in`

**Example Request Body**

```
{
  "first": 1,
  "last": 5,
  "rows": 5,
  "sortField": "name",
  "sortOrder": -1,
  "multiSort": [
    {
      "field": "guid",
      "order": 1
    }
  ],
  "filters": {
    "name": {
      "value": "Produ",
      "matchMode": "contains"
    }
  },
  "globalFilter": "www"
}
