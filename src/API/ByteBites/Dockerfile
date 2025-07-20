# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file first and restore dependencies
# This leverages Docker cache if only code changes, not dependencies
COPY ["ByteBites.csproj", "./"]
RUN dotnet restore "ByteBites.csproj"

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish "ByteBites.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Set environment variables for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "ByteBites.dll"]