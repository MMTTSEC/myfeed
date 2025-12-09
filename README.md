# myFeed

A lightweight, modern social network application inspired by Twitter/X. Users can create posts, follow other users, like posts, send direct messages, and manage their profiles in a clean, intuitive interface.

## Table of Contents

- [Technologies & Languages](#technologies--languages)
- [Project Overview](#project-overview)
- [Frontend](#frontend)
- [Backend](#backend)
- [API & Integration](#api--integration)
- [Project Structure](#project-structure)
- [Clean Code Principles](#clean-code-principles)
- [Testing](#testing)
- [Getting Started](#getting-started)

---

## Technologies & Languages

### Frontend

- **Framework:** React 18+ with TypeScript
- **Build Tool:** Vite
- **Styling:** CSS3 (modular, component-scoped)
- **Icons:** Bootstrap Icons
- **State Management:** React hooks (useState, useRef, useEffect)
- **Linting:** ESLint

### Backend

- **Language:** C#
- **Framework:** .NET (ASP.NET Core)
- **Database:** Entity Framework Core with SQL Server / PostgreSQL
- **Architecture:** Layered (Api, Application, Domain, Infrastructure, Tests)
- **Dependency Injection:** Built-in .NET DI container
- **Authentication:** JWT-based (planned/in development)

### Development & DevOps

- **Version Control:** Git
- **Package Managers:** npm (frontend), NuGet (backend)
- **IDE:** Visual Studio / VS Code

---

## Project Overview

**myFeed** is a Twitter-like social platform with the following core features:

- **User Accounts:** Registration, login, profile management
- **Posts:** Create, read, and browse posts in multiple feed types (home, followed users, user profile)
- **Interactions:** Like posts, follow/unfollow users
- **Messaging:** Send and receive direct messages between users
- **Responsive Design:** Desktop and mobile UI

---

## Frontend

The frontend is built with **React + TypeScript** and emphasizes clean, reusable components and type safety.

### Key Features

- **Component-Based Architecture:** Modular components (DisplayFeed, WritePost, DisplayMessages, etc.)
- **Type Safety:** All components use TypeScript interfaces
- **Responsive Layout:** Mobile navigation (MobileHeader) and column-based layouts
- **Real-Time Updates:** Dynamic post sorting (newest first) and like counts
- **Clean Styling:** Scoped CSS files matching component names

### Main Components

- `DisplayFeed.tsx` – Renders feed with sorted posts
- `WritePost.tsx` – Post creation with validation and auto-resize
- `DisplayMessages.tsx` – Displays conversations with formatted timestamps
- `Navigation.tsx` – Main app navigation
- `MobileHeader.tsx` – Mobile-specific header with toggle sections
- `LoginRegisterPage.tsx` – Authentication forms with client-side validation

### Running the Frontend

```bash
cd frontend
npm install
npm run dev
```

---

## Backend

The backend uses a **layered architecture** with clear separation of concerns: Domain → Application → Infrastructure → API.

### Architecture Layers

1. **MyFeed.Domain** – Core entities and interfaces (User, Post, Like, Follow, DM)
2. **MyFeed.Application** – Business logic and services (UserService, PostService, etc.)
3. **MyFeed.Infrastructure** – Data access, repositories, and database configuration
4. **MyFeed.Api** – HTTP endpoints and request/response handling
5. **MyFeed.Tests** – Unit and integration tests

### Key Features

- **Repository Pattern:** Abstracts data access behind interfaces
- **Service Layer:** Encapsulates business logic
- **Dependency Injection:** Loosely coupled, testable design
- **Entity Framework Core:** ORM for database operations
- **API Controllers:** RESTful endpoints for all major features

### Main Endpoints

- `/api/users` – User management and profiles
- `/api/posts` – Post CRUD operations
- `/api/likes` – Like/unlike posts
- `/api/follows` – Follow/unfollow users
- `/api/messages` – Direct messaging

### Running the Backend

```bash
cd backend
dotnet restore
dotnet run --project MyFeed.Api
```

---

## API & Integration

The frontend and backend communicate via **RESTful HTTP APIs**. The API contracts use JSON for data exchange.

### API Communication Flow

1. **Frontend Action:** User creates a post, clicks like, or sends a message
2. **HTTP Request:** React component calls API utility (e.g., `postsApi.ts`)
3. **Backend Processing:** Controller routes to appropriate service
4. **Database Operation:** Service/Repository queries or updates data
5. **Response:** JSON response returned to frontend
6. **UI Update:** Component updates state and re-renders

### Example: Creating a Post

```
React Component → WritePost.tsx
  ↓
API Utility → postsApi.ts (POST /api/posts)
  ↓
Backend Controller → PostsController.cs
  ↓
Service Layer → PostService.cs (validates, creates post)
  ↓
Repository → PostRepository.cs (saves to database)
  ↓
Response → 200 OK + Post object
  ↓
Frontend State Update → Display new post
```

### API Utilities (Frontend)

Located in `frontend/src/utils/`:

- `api.ts` – Base HTTP client configuration
- `postsApi.ts` – Post endpoints
- `likesApi.ts` – Like endpoints
- `followsApi.ts` – Follow endpoints
- `messagesApi.ts` – Message endpoints

---

## Project Structure

### Frontend

```
frontend/
├── src/
│   ├── components/         # Reusable React components
│   ├── pages/             # Page-level components
│   ├── interfaces/        # TypeScript interfaces (Post, Message, User)
│   ├── styles/            # Component-scoped CSS
│   ├── utils/             # API utilities and helpers
│   ├── App.tsx            # Root component
│   ├── main.tsx           # Entry point
│   └── routes.ts          # Route definitions
├── public/                # Static assets
├── package.json
├── tsconfig.json
└── vite.config.ts         # Vite build configuration
```

### Backend

```
backend/
├── MyFeed.Api/            # API layer (Controllers, HTTP handling)
├── MyFeed.Application/    # Business logic (Services, DTOs, Validation)
├── MyFeed.Domain/         # Core entities and interfaces
├── MyFeed.Infrastructure/ # Data access (Repositories, DbContext, Migrations)
├── MyFeed.Tests/          # Unit and integration tests
└── MyFeed.sln            # Solution file
```

---

## Clean Code Principles

myFeed adheres to several Clean Code best practices:

### 1. **Meaningful Names**

- Component names describe their purpose (e.g., `DisplayFeed`, `WritePost`)
- Variables and functions use clear, descriptive naming
- Avoid abbreviations unless widely understood

### 2. **Small, Focused Functions**

- Services have single responsibilities (UserService, PostService, etc.)
- Components are lightweight and handle one feature
- Utility functions are concise and reusable

### 3. **DRY (Don't Repeat Yourself)**

- Shared logic extracted into helpers (e.g., `formatDate`, `isProfileOrMessagesPage`)
- API utilities centralize HTTP communication
- Component styles use CSS classes, avoiding inline styles

### 4. **Type Safety**

- TypeScript throughout the frontend prevents runtime errors
- Interfaces for all data structures (Post, Message, User)
- Strict null checks enabled in tsconfig

### 5. **Separation of Concerns**

- Frontend: Components, utilities, styles are separate
- Backend: Layered architecture isolates domain, application, and infrastructure
- API contracts define clear boundaries between frontend and backend

### 6. **Error Handling**

- Backend services validate inputs and return meaningful error messages
- Frontend components gracefully handle empty states and errors
- Form validation provides immediate user feedback

### 7. **Code Organization**

- One component per file (except interfaces/utilities)
- Consistent folder structure across the project
- Clear naming conventions for CSS classes (BEM-like approach)

### 8. **Testability**

- Backend services are injectable and mockable
- Repositories abstract data access
- Frontend components use props for dependency injection

---

## Testing

Testing is a critical part of the myFeed development process. The backend includes comprehensive unit and integration tests, while the frontend is built with testability in mind.

### Backend Testing

**Framework:** xUnit (or NUnit)

**Test Structure:**
The `MyFeed.Tests` project mirrors the main project structure for easy navigation:

```
MyFeed.Tests/
├── Application/       # Service and business logic tests
├── Domain/           # Entity and domain logic tests
├── Controllers/      # API endpoint tests
└── MyFeed.Tests.csproj
```

**Test Coverage:**
Current test coverage targets:

- **Services (Application Layer):** ~80-90% coverage
  - UserService, PostService, LikeService, FollowService, DMService
  - Validates business rules, input validation, error handling
- **Repositories (Infrastructure Layer):** ~75% coverage
  - Data access logic, query correctness, edge cases
- **Domain Entities:** ~85% coverage
  - Entity behavior and invariants
- **Controllers (API Layer):** ~60-70% coverage
  - Endpoint routing, HTTP status codes, request/response handling

**Overall Backend Coverage:** ~75-80% of codebase

**Running Tests:**

```bash
cd backend
dotnet test                           # Run all tests
dotnet test --logger:console          # Run with detailed output
dotnet test --filter "FullyQualifiedName~ServiceTests"  # Run specific test class
```

### Test Examples

**Service Test (UserService):**

```csharp
[Fact]
public void CreateUser_ValidInput_ReturnsUser()
{
  // Arrange
  var service = new UserService(_userRepositoryMock.Object);
  var username = "testuser";

  // Act
  var result = service.CreateUser(username);

  // Assert
  Assert.NotNull(result);
  Assert.Equal(username, result.Username);
}
```

**Repository Test (PostRepository):**

```csharp
[Fact]
public void GetPostsByAuthor_ValidAuthor_ReturnsPostList()
{
  // Arrange
  var repository = new PostRepository(_dbContext);
  var authorId = 1;

  // Act
  var posts = repository.GetPostsByAuthor(authorId);

  // Assert
  Assert.NotEmpty(posts);
  Assert.All(posts, p => Assert.Equal(authorId, p.AuthorId));
}
```

### Frontend Testing

While the frontend focuses on component integration and manual testing during development, the architecture supports unit testing:

**Recommended Frameworks:**

- **Vitest** – Fast unit test framework compatible with Vite
- **React Testing Library** – Component testing best practices
- **MSW (Mock Service Worker)** – Mock API responses for integration tests

**Testing Strategy:**

- **Unit Tests:** Utility functions (e.g., `formatDate`, `isProfileOrMessagesPage`)
- **Integration Tests:** Component interactions with state and hooks
- **E2E Tests:** Critical user flows (login, post creation, messaging)

### Continuous Integration

Tests should be run automatically before deployment:

- On every commit (pre-commit hook)
- On pull requests (GitHub Actions / CI/CD pipeline)
- Before releases

---

## Getting Started

### Prerequisites

- Node.js 16+ (frontend)
- .NET 6+ SDK (backend)
- SQL Server or PostgreSQL (database)

### Installation

**Frontend:**

```bash
cd frontend
npm install
npm run dev
```

Access the app at `http://localhost:5173` (or as shown in terminal).

**Backend:**

```bash
cd backend
dotnet restore
dotnet ef database update        # Apply migrations
dotnet run --project MyFeed.Api
```

API runs on `http://localhost:5000` (or as configured in launchSettings.json).

### Configuration

- **Frontend:** Update API base URL in `src/utils/api.ts` if needed
- **Backend:** Configure database connection in `appsettings.json`
- **Authentication:** JWT setup in `Program.cs` and `JwtService.cs`
