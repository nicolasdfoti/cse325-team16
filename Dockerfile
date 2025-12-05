# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Backend
COPY DailyNotes.API/*.csproj ./DailyNotes.API/
RUN dotnet restore DailyNotes.API/DailyNotes.API.csproj

# Copy backend code including wwwroot con Blazor
COPY DailyNotes.API/. ./DailyNotes.API/

# Publish backend
RUN dotnet publish DailyNotes.API/DailyNotes.API.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DailyNotes.API.dll"]
