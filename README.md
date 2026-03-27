Built as a learning project to master backend  using .NET.

#  GitHub Clone Backend (ASP.NET Core)

    A GitHub-like backend system built using **ASP.NET Core Web API**, **Dapper**, and **Clean Architecture**.  
    This project simulates core GitHub features such as repositories, branches, commits, pull requests, issues, and social interactions.



##  Features

###  Authentication
    - User registration with email verification & login
    - JWT-based authentication
    - Secure password hashing
### Email Verification

        To ensure only valid users can access the system, email verification is implemented during registration.

        Flow
        User registers with email and password
        A unique verification token is generated (GUID)
        Token and expiry time (24 hours) are stored in the database
        A verification link is sent to the user's email
        User clicks the link -> /api/auth/verify-email?token=...
        Backend validates the token and marks the email as verified
        User can now log in successfully

Rate Limiting

        Rate limiting is implemented to protect the API from abuse and excessive requests.
        To prevent brute-force attacks (especially on login)

Real-Time Notifications with SignalR

    This project uses SignalR to enable real-time communication between the server and clients. It allows the backend to instantly 
    push notifications to users without requiring them to refresh the page.
     
###  Repository Management
    - Create, update, delete repositories
    - Pagination support

###  Branch Management
    - Default `main` branch creation
    - Multiple branches per repository

###  Commit System
    - Create commits
    - View commits by repository
    - View commits by branch
    - Tracks commit history

###  Pull Requests
    - Create pull requests
    - Add comments
    - Close / Merge pull requests

###  Issues
    - Create issues
    - Add comments
    - Close issues

###  Social Features
    - Follow / Unfollow users
    - Star / Unstar repositories
    - Activity feed system
    
###  Transaction Management

    - Ensures multiple database operations execute as a single unit
    - Maintains data consistency and integrity
    - Prevents partial updates during critical operations like Pull Request merge
    
    Example:
    - When merging a pull request:
      - PR status is updated
      - Commit is created
      - Changes are applied

If any step fails, all changes are rolled back automaticall

###  Performance
    - Dapper for fast database operations
    - Optimized queries with pagination



##  Architecture

    This project follows **Clean Architecture**:

    
---

##  Tech Stack

    - ASP.NET Core Web API
    - Dapper (Micro ORM)
    - SQL Server
    - JWT Authentication
    - Email Verification
    - AutoMapper
    - Transactions

---

##  Key Concepts Implemented

    - Clean Architecture
    - Repository Pattern
    - Dependency Injection
    - JWT Authentication
    - Transactions
    - Pagination (OFFSET-FETCH)
    - API Response Wrapper
    - Async/Await Best Practices

---

##  API Endpoints (Sample)

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/verify-email`

### Repository
- `GET /api/repository/all?pageNumber=1&pageSize=10`
- `GET /api/repository/mine`
- `POST /api/repository`
- `PUT /api/repository/{id}`
- `DELETE /api/repository/{id}`

### Branch
- `POST /api/branch`
- `GET /api/branch/{repoId}`

### Commit
- `POST /api/commit`
- `GET /api/commit/{repoId}`
- `GET /api/commit/branch/{branchId}`

### Pull Request
- `POST /api/pullrequest`
- `GET /api/pullrequest/{repoId}`
- `POST /api/pullrequest/comment`
- `POST /api/pullrequest/{id}/merge`

### Issue
- `POST /api/issue`
- `GET /api/issue/{repositoryId}`
- `POST /api/issue/comment`
- `POST /api/issue/{id}/close`

### Social
- `POST /api/social/follow/{userId}`
- `POST /api/social/unfollow/{userId}`
- `POST /api/social/star/{repoId}`
- `POST /api/social/unstar/{repoId}`
- `GET /api/social/feed`

---

##  How to Run

    1. Clone the repository
    ```bash
      git clone https://github.com/your-username/github-clone-backend.git


    2.Configure database connection in appsettings.json
    Run the project
    dotnet run
    Open Swagger UI
    https://localhost:xxxx/swagger
