FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Web/Web.csproj Core/

RUN dotnet restore Web/Web.csproj

COPY . .

WORKDIR "/src/Web"

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app/Web

COPY --from=build /app/publish .

EXPOSE 5151

ENTRYPOINT ["dotnet", "Web.dll"]