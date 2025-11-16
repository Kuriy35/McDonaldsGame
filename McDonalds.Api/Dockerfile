FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY McDonalds.Api/McDonalds.Api.csproj McDonalds.Api/
COPY McDonalds.Data/McDonalds.Data.csproj McDonalds.Data/
COPY McDonalds.Contracts/McDonalds.Contracts.csproj McDonalds.Contracts/

WORKDIR /src/McDonalds.Api
RUN dotnet restore

WORKDIR /src
COPY McDonalds.Api/ McDonalds.Api/
COPY McDonalds.Data/ McDonalds.Data/
COPY McDonalds.Contracts/ McDonalds.Contracts/

WORKDIR /src/McDonalds.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "McDonalds.Api.dll"]