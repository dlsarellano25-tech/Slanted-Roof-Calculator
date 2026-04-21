# RoofBlockCalculator

A web-based calculator for the volume of a **Pentahedral Slanted-Roof Block Solid**, built with HTML/CSS/JavaScript on the front-end and ASP.NET Core (.NET 10) Minimal API on the back-end. Ships with a Dockerfile so it runs anywhere.

## The Solid

The Pentahedral Slanted-Roof Block is composed of:

1. A **rectangular prism base** &mdash; length `L`, width `W`, height `h_base`.
2. A **slanted-roof wedge** on top whose two opposite long edges have **different** heights `h₁` (front) and `h₂` (back).

### Formula

```
V = L × W × ( h_base + (h₁ + h₂) / 2 )
```

## Project Layout

```
RoofBlockCalculator/
├── RoofBlockCalculator.csproj   # .NET 10 web project
├── Program.cs                   # Minimal API: POST /api/calculate, GET /healthz
├── VolumeCalculator.cs          # Pure calculation + validation logic
├── appsettings.json
├── Properties/
│   └── launchSettings.json
├── wwwroot/
│   ├── index.html
│   ├── styles.css
│   └── script.js
├── Dockerfile
├── .dockerignore
├── .gitignore
├── README.md
└── TermPaper.md                 # Term paper explaining the project
```

## Run with Docker

```bash
docker build -t roof-block-calculator .
docker run --rm -p 8080:8080 roof-block-calculator
```

Then open <http://localhost:8080>.

## Run with .NET (no Docker)

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

## API

### `POST /api/calculate`

Request:
```json
{
  "length": 10,
  "width": 6,
  "baseHeight": 4,
  "roofHeight1": 3,
  "roofHeight2": 1.5
}
```

Response:
```json
{
  "basePrismVolume": 240,
  "slantedRoofVolume": 135,
  "totalVolume": 375,
  "formula": "V = L * W * (h_base + (h1 + h2) / 2)"
}
```

### `GET /healthz`

Returns `{ "status": "ok" }`. Used by the Docker `HEALTHCHECK`.

## Deploy on Render

1. Push this folder to a GitHub repo.
2. On [Render](https://render.com/), create a new **Web Service** and point it at the repo.
3. Choose **Docker** as the runtime &mdash; Render will detect the `Dockerfile` automatically.
4. Render injects the `PORT` env var; the app already reads it.
