FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RabbitMQReiverCoreAPI/RabbitMQReiverCoreAPI.csproj", "RabbitMQReiverCoreAPI/"]
RUN dotnet restore "RabbitMQReiverCoreAPI/RabbitMQReiverCoreAPI.csproj"
COPY . .
WORKDIR "/src/RabbitMQReiverCoreAPI"
RUN dotnet build "RabbitMQReiverCoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQReiverCoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQReiverCoreAPI.dll"]