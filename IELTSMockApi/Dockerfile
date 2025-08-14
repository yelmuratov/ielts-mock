# Use .NET 9 SDK for build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY IELTSMockApi.csproj .
RUN dotnet restore ./IELTSMockApi.csproj
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Use .NET 9 runtime for runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IELTSMockApi.dll"]
