# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ./src/ ./
RUN dotnet restore UniversityManagement.API/UniversityManagement.API.csproj
RUN dotnet publish UniversityManagement.API/UniversityManagement.API.csproj -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "UniversityManagement.API.dll"]
