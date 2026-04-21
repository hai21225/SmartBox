FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Core/Core.csproj Core/

RUN dotnet restore Core/Core.csproj

COPY . .

WORKDIR "/src/Core"

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app/Core

COPY --from=build /app/publish .

EXPOSE 5033

ENTRYPOINT ["dotnet", "Core.dll"]