FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Backend
COPY DailyNotes.API/*.csproj ./DailyNotes.API/
RUN dotnet restore DailyNotes.API/DailyNotes.API.csproj

# Frontend
COPY DailyNotes/*.csproj ./DailyNotes/
RUN dotnet restore DailyNotes/DailyNotes.csproj

COPY . .

RUN dotnet publish DailyNotes/DailyNotes.csproj -c Release -o /app/DailyNotes.API/wwwroot

RUN dotnet publish DailyNotes.API/DailyNotes.API.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080
ENTRYPOINT ["dotnet", "DailyNotes.API.dll"]
