FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5110

ENV ASPNETCORE_URLS=http://+:5110

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["sda-online-2-csharp-backend_teamwork1.csproj", "./"]
RUN dotnet restore "sda-online-2-csharp-backend_teamwork1.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "sda-online-2-csharp-backend_teamwork1.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "sda-online-2-csharp-backend_teamwork1.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sda-online-2-csharp-backend_teamwork1.dll"]
