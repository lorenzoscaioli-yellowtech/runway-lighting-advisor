# Runway Lighting Advisor

Piccola applicazione dimostrativa in C#/.NET: suggerisce il gradino di luminosità di ogni circuito
di una pista in base alla visibilità. Dominio e dati sono inventati.

## Struttura

```
RunwayLighting.sln
src/RunwayLighting.Core/    logica di dominio (regole in docs/requisiti.md)
src/RunwayLighting.Web/     interfaccia Blazor Server
tests/RunwayLighting.Tests/ test xUnit
docs/requisiti.md           requisiti funzionali
```

Accessibile in

```
http://localhost:5113
```

## Requisiti

.NET SDK 10. La solution si apre in Visual Studio 2022 (17.12+) o Visual Studio 2026,
oppure in VS Code con l'estensione C# Dev Kit.

## Comandi

```bash
dotnet build
dotnet test
dotnet run --project src/RunwayLighting.Web
```

La CI su GitHub Actions (`.github/workflows/ci.yml`) esegue build e test a ogni push e pull request.
