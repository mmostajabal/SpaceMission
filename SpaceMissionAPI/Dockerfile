#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the ASP.NET Core base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj files and restore dependencies
COPY ["SpaceMissionAPI/SpaceMissionAPI.csproj", "SpaceMissionAPI/"]
COPY ["SpaceMissionModels/SpaceMissionModels.csproj", "SpaceMissionModels/"]
COPY ["SpaceMissionServices/SpaceMissionServices.csproj", "SpaceMissionServices/"]
COPY ["SpaceMissionShared/SpaceMissionShared.csproj", "SpaceMissionShared/"]
RUN dotnet restore "SpaceMissionAPI/SpaceMissionAPI.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/SpaceMissionAPI"
RUN dotnet build "SpaceMissionAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "SpaceMissionAPI.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceMissionAPI.dll"]

