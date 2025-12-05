# ===========================
#   BUILD STAGE
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only solution + csproj files first
COPY DailyNotes.sln ./
COPY DailyNotes/DailyNotes.csproj ./DailyNotes/
COPY DailyNotes.API/DailyNotes.API.csproj ./DailyNotes.API/

# Restore dependencies
RUN dotnet restore DailyNotes.API/DailyNotes.API.csproj

# Copy full source
COPY . .

# Publish API + Blazor client (because of ProjectReference)
WORKDIR /src/DailyNotes.API
RUN dotnet publish -c Release -o /app/publish


# ===========================
#   RUNTIME STAGE
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Copy published output (API + Blazor files)
COPY --from=build /app/publish ./

# Render requirement: bind to dynamic PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE 10000

ENTRYPOINT ["dotnet", "DailyNotes.API.dll"]
