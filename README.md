Built as a learning project to master backend  using .NET.

#  GitHub Clone Backend (ASP.NET Core)

    A GitHub-like backend system built using **ASP.NET Core Web API**, **Dapper**, and **Clean Architecture**.  
    This project simulates core GitHub features such as repositories, branches, commits, pull requests, issues, and social interactions.



##  Features

###  Authentication
    - User registration & login
    - JWT-based authentication
    - Secure password hashing

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
