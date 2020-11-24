#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# misc
# 3.1-alpine
# mcr.microsoft.com/dotnet/sdk:3.1
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EFCoreTutorial.Lesson_01/EFCoreTutorial.Lesson_01.csproj", "EFCoreTutorial.Lesson_01/"]
RUN dotnet restore "EFCoreTutorial.Lesson_01/EFCoreTutorial.Lesson_01.csproj"
COPY . .
WORKDIR "/src/EFCoreTutorial.Lesson_01"
RUN dotnet build "EFCoreTutorial.Lesson_01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EFCoreTutorial.Lesson_01.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EFCoreTutorial.Lesson_01.dll"]