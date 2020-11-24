#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RabbitMQTutorial.Mailer_1/RabbitMQTutorial.Mailer_1.csproj", "RabbitMQTutorial.Mailer_1/"]
RUN dotnet restore "RabbitMQTutorial.Mailer_1/RabbitMQTutorial.Mailer_1.csproj"
COPY . .
WORKDIR "/src/RabbitMQTutorial.Mailer_1"
RUN dotnet build "RabbitMQTutorial.Mailer_1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQTutorial.Mailer_1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQTutorial.Mailer_1.dll"]