# Base image used to create the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image which builds the project and prepares the assets for publishing
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./testapp-app .
COPY ./testapp-shared ../testapp-shared
RUN dotnet restore "./Api.csproj"
WORKDIR "/src/."
RUN dotnet build "./Api.csproj" -c Release -o /app/build

# Publish image which sets up the optimized version of the app into a folder
FROM build AS publish
RUN dotnet publish "./Api.csproj" -c Release -o /app/publish

# Final image which only contains the published content of the project
# This is where the resulting files of the published app are moved to
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]