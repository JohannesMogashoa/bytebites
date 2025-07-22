# Solution Breakdown

## Tech Stack

-   **Frontend:** Next.js (React-based)
-   **Backend:** .NET Web API
-   **Database:** SQL (accessed via EF Core)
-   **Authentication:** Auth0 (OAuth 2.0 / OpenID Connect)

---

### 🏗 Architecture Overview

1. **User Flow**

    - The user initiates an **initial request** on the Next.js frontend.
    - The frontend triggers the **Auth0 Authentication Flow** using credentials (e.g., email/password) or external providers (e.g., Google).
    - On success, Auth0 returns a **Session** with:

        - `access_token` (API access)
        - `id_token` (user identity)
        - `user` metadata

    - The frontend sends an **API Request** to the .NET backend, attaching the `JWT` in the Authorization header.
    - The backend **validates the JWT** with Auth0.
    - On success, the backend uses **EF Core** to query or update the **SQL database**, returning data to the frontend.

2. **Key Components**

    - **Next.js**: Handles UI rendering, user session state, and API callouts.
    - **Auth0**: Abstracts the authentication and authorization logic (OpenID Connect).
    - **.NET API**: Handles secured data operations, verifies tokens.
    - **SQL (Relational DB)**: Stores persistent business data.
    - **EF Core**: ORM for managing database operations in .NET.

---

### ⚖️ Trade-Offs

| Area         | Benefit                                              | Trade-Off                                                    |
| ------------ | ---------------------------------------------------- | ------------------------------------------------------------ |
| **Auth0**    | Fast to implement, handles security & social login   | External dependency, pricing grows with usage                |
| **Next.js**  | Server/client hybrid for performance and flexibility | Extra complexity in managing auth state across SSR and CSR   |
| **.NET API** | Strong typing, scalable backend logic                | Dev experience might require more configuration than Node.js |
| **EF Core**  | Productivity via ORM abstractions                    | Less fine-grained control over SQL compared to raw queries   |
| **JWT**      | Stateless, scalable                                  | No built-in token revocation (unless manually implemented)   |

---

### 💸 Cost Structures

| Component            | Free Tier                                  | Cost Considerations                                                                |
| -------------------- | ------------------------------------------ | ---------------------------------------------------------------------------------- |
| **Auth0**            | \~7,000 users/mo on free tier              | Paid tiers scale with Monthly Active Users (MAUs) and features like RBAC, SSO, MFA |
| **Next.js** (Vercel) | Generous free tier                         | Vercel pricing scales with bandwidth, serverless functions                         |
| **.NET API**         | Free on self-hosted                        | Hosting (e.g., Azure App Service or containers) incurs compute/storage cost        |
| **SQL Database**     | Varies (SQL Server, Azure SQL, PostgreSQL) | Pay per storage, compute units, backups                                            |
| **EF Core**          | Free                                       | No direct cost                                                                     |

---

### 🔒 Security Considerations

| Area                                  | Risk                                   | Mitigation                                                            |
| ------------------------------------- | -------------------------------------- | --------------------------------------------------------------------- |
| **Access Token Theft**                | Can impersonate users                  | Use HTTPS, store tokens securely (httpOnly cookies or secure storage) |
| **JWT Expiry**                        | Long-lived tokens may be risky         | Keep short expiry + implement refresh tokens or silent auth           |
| **CSRF (Cross-Site Request Forgery)** | API may be vulnerable if using cookies | Use Authorization headers with Bearer tokens                          |
| **SQL Injection**                     | If raw SQL used improperly             | EF Core handles most cases; always parameterize queries               |
| **Session Management**                | JWTs are stateless                     | Store refresh tokens securely; use Auth0 session TTL policies         |
| **Overexposed API**                   | Open endpoints                         | Secure all endpoints with JWT middleware, use claims for RBAC         |

---
