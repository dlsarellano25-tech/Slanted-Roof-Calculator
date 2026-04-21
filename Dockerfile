# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore as a separate layer for better caching.
COPY RoofBlockCalculator.csproj ./
RUN dotnet restore RoofBlockCalculator.csproj

# Copy the rest and publish.
COPY . .
RUN dotnet publish RoofBlockCalculator.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Run as a non-root user.
RUN useradd --uid 1001 --create-home --shell /bin/bash appuser
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget -qO- http://localhost:8080/healthz || exit 1

ENTRYPOINT ["dotnet", "RoofBlockCalculator.dll"]
