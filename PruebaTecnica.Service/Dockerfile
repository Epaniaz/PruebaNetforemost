FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./ /src/
RUN dotnet restore "./PruebaTecnica.Service/PruebaTecnica.Service.csproj"
COPY . .
WORKDIR "/src/PruebaTecnica.Service"
RUN dotnet build "PruebaTecnica.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PruebaTecnica.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruebaTecnica.Service.dll"]
