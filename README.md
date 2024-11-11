blulibrary/
├── BluLibrary.sln
├── BluLibrary.Api/
│ ├── Controllers/
│ │ └── V1/
│ │ ├── BaseController.cs
│ │ └── BlurayController.cs
│ ├── Extensions/
│ │ ├── ApplicationBuilderExtensions.cs
│ │ └── ServiceCollectionExtensions.cs
│ ├── Middleware/
│ │ ├── Authentication/
│ │ └── ExceptionHandling/
│ ├── Configurations/
│ │ ├── ApiOptions/
│ │ ├── Launch/
│ │ │ └── launchSettings.json
│ │ ├── Swagger/
│ │ ├── appsettings.json
│ │ └── appsettings.Development.json
│ ├── Program.cs
│ ├── BluLibrary.Api.http
│ └── BluLibrary.Api.csproj
├── BluLibrary.Core/
│ ├── Entities/
│ │ ├── Base/
│ │ │ └── BaseEntity.cs
│ │ └── Bluray.cs
│ ├── Interfaces/
│ │ ├── Repositories/
│ │ └── Services/
│ ├── Services/ # Uniquement les interfaces de service
│ ├── DTOs/
│ │ ├── Requests/
│ │ └── Responses/
│ ├── Exceptions/
│ │ └── DomainException.cs
│ └── BluLibrary.Core.csproj
├── BluLibrary.Infrastructure/
│ ├── Data/
│ │ ├── Context/
│ │ │ └── BluLibraryContext.cs
│ │ └── Configurations/
│ │ └── BlurayConfiguration.cs
│ ├── Repositories/
│ │ └── BaseRepository.cs
│ ├── Services/ # Implémentations des services
│ │ └── External/
│ ├── Migrations/
│ └── BluLibrary.Infrastructure.csproj
└── BluLibrary.Tests/
│ ├── Common/
│ │ └── TestData/
│ ├── IntegrationTests/
│ │ ├── Api/
│ │ └── Infrastructure/
│ ├── UnitTests/
│ │ ├── Api/
│ │ ├── Core/
│ │ └── Infrastructure/
│ └── BluLibrary.Tests.csproj
└── .gitignore
