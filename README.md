# BluLibrary

BluLibrary est une application de gestion de Blu-ray développée avec .NET 8 suivant les principes de Clean Architecture.

## 🚀 Fonctionnalités

- Gestion complète des Blu-ray (CRUD)
- Recherche par titre, réalisateur ou ISBN
- Validation des données selon les règles métier
- API REST avec versioning
- Documentation Swagger/OpenAPI

## 🛠 Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- xUnit pour les tests

## 🏗 Architecture

Le projet suit les principes de Clean Architecture avec une séparation claire des responsabilités :

- **BluLibrary.Core** : Logique métier et entités
- **BluLibrary.Infrastructure** : Persistence et services externes
- **BluLibrary.Api** : Interface REST API
- **BluLibrary.Tests** : Tests unitaires et d'intégration

## 🚦 Prérequis

- .NET 8 SDK
- SQL Server (LocalDB ou instance complète)
- Visual Studio 2022 ou VS Code

## 🏃‍♂️ Installation

1. Cloner le repository

```bash
git clone https://github.com/votre-username/blulibrary.git
cd blulibrary
```

2. Restaurer les packages

```bash
dotnet restore
```

3. Mettre à jour la base de données

```bash
cd BluLibrary.Api
dotnet ef database update
```

4. Lancer l'application

```bash
dotnet run
```

L'API sera disponible sur `https://localhost:7074` et la documentation Swagger sur `https://localhost:7074/swagger`.

## 🧪 Tests

Pour exécuter les tests :

```bash
dotnet test
```

## 📝 Documentation API

La documentation complète de l'API est disponible via Swagger UI lorsque l'application est en cours d'exécution.

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à ouvrir une issue ou soumettre une pull request.

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.
