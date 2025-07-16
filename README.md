# Bankify API Project

> **NOTE:**  
> 📘 For better review, click on **`readme.md`** → click on the **`Code`** tab next to **`Preview`**

---

This is the **Bankify API** project structure.  
It follows a **Clean Architecture** and **CQRS system design**, making it suitable for complex business scenarios and easy to maintain.

Please review the project and 🌟 **star it if you like it**, or leave a comment with your feedback!

---

## 📁 Project Structure

├── .gitattributes
├── .gitignore
├── Bankify.Api
│ ├── Bankify.Api.csproj
│ ├── Bankify.Api.http
│ ├── Controllers
│ │ └── V1.0
│ │ ├── Accounts
│ │ │ ├── AccountTypesController.cs
│ │ │ └── AccountsController.cs
│ │ ├── BaseController.cs
│ │ └── Users
│ │ └── UsersController.cs
│ ├── Extensions
│ │ └── RegistrarExtensions.cs
│ ├── Filters
│ │ ├── AuthorizationHandler.cs
│ │ └── ExceptionHandler.cs
│ ├── MappingProfiles
│ │ └── MappingProfiles.cs
│ ├── Options
│ │ └── ConfigureSwaggerOptions.cs
│ ├── Program.cs
│ ├── Properties
│ │ └── launchSettings.json
│ ├── Registrars
│ │ ├── ApplicationInsightsRegistrar.cs
│ │ ├── ApplicationLayerRegistrar.cs
│ │ ├── DbRegistrar.cs
│ │ ├── IRegistrar.cs
│ │ ├── IWebApplicationBuilderRegistrar.cs
│ │ ├── IWebApplicationRegistrar.cs
│ │ ├── MigrationManager.cs
│ │ ├── MvcRegistrar.cs
│ │ ├── MvcWebAppRegistrar.cs
│ │ ├── RepositoryRegistrar.cs
│ │ ├── ServicesRegistrar.cs
│ │ └── SwaggerRegistrar.cs
│ ├── Uploads
│ │ ├── 358e3ed9-e1cf-4e0d-9e24-3e4b0ad60aeb.JPG
│ │ ├── 75d39cdf-7453-4be3-99b3-34b143ae579b.JPG
│ │ ├── 847fd5a6-c156-4bb7-ba6a-3b9ce49c46d1.JPG
│ │ └── 99599c03-5e8d-4642-a2d0-8de77c7f2c9d.JPG
│ ├── appsettings.Development.json
│ └── appsettings.json
├── Bankify.Application
│ ├── Bankify.Application.csproj
│ ├── Common
│ │ ├── DTOs
│ │ │ └── Users
│ │ │ └── Request
│ │ │ ├── CreateUserRequest.cs
│ │ │ └── UpdateUserRequest.cs
│ │ └── Helpers
│ │ ├── Error.cs
│ │ ├── ErrorCode.cs
│ │ ├── HttpService.cs
│ │ ├── HttpService
│ │ │ └── Models
│ │ │ ├── ApiRequest.cs
│ │ │ ├── ApiType.cs
│ │ │ └── Response.cs
│ │ ├── IHttpService.cs
│ │ ├── OperationalResult.cs
│ │ └── Settings.cs
│ ├── Features
│ │ ├── Commands
│ │ │ └── User
│ │ │ ├── CreateUser.cs
│ │ │ ├── DeleteUser.cs
│ │ │ └── UpdateUser.cs
│ │ └── Queries
│ │ ├── Accounts
│ │ │ └── GetAllAccounts.cs
│ │ └── Users
│ │ ├── GetAllUsers.cs
│ │ └── GetUserById.cs
│ ├── Repository
│ │ ├── IRepositoryBase.cs
│ │ └── RepositoryBase.cs
│ └── Services
│ └── IFileStorageService.cs
├── Bankify.Domain
│ ├── Bankify.Domain.csproj
│ ├── Common
│ │ ├── NotValidException.cs
│ │ └── Shared
│ │ ├── BaseEntity.cs
│ │ ├── Enums.cs
│ │ └── ErrorResponse.cs
│ ├── Models
│ │ ├── Accounts
│ │ │ ├── Account.cs
│ │ │ └── AccountType.cs
│ │ ├── Loans
│ │ │ ├── Loan.cs
│ │ │ └── LoanType.cs
│ │ └── Users
│ │ └── BUser.cs
│ └── Validators
│ └── UserDataValidator.cs
├── Bankify.Infrastructure
│ ├── Bankify.Infrastructure.csproj
│ ├── Context
│ │ └── BankifyDbContext.cs
│ └── Migrations
│ ├── 20250205052859_initialMigrationForUsers.Designer.cs
│ ├── 20250205052859_initialMigrationForUsers.cs
│ ├── 20250205053733_MigrationForUsers.Designer.cs
│ ├── 20250205053733_MigrationForUsers.cs
│ └── BankifyDbContextModelSnapshot.cs
├── Bankify.sln
└── LICENSE

