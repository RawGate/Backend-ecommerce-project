# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["sda-online-2-csharp-backend_teamwork1.csproj", "./"]
RUN dotnet restore "sda-online-2-csharp-backend_teamwork1.csproj"

# Copy the remaining files and build the project
COPY . .
RUN dotnet build "sda-online-2-csharp-backend_teamwork1.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "sda-online-2-csharp-backend_teamwork1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 5110
ENV ASPNETCORE_URLS=http://+:5110

# Copy the published application from the build stage
COPY --from=publish /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "sda-online-2-csharp-backend_teamwork1.dll"]
