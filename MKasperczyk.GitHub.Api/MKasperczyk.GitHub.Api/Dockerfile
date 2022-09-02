#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MKasperczyk.GitHub.Api/MKasperczyk.GitHub.Api.csproj", "MKasperczyk.GitHub.Api/"]
RUN dotnet restore "MKasperczyk.GitHub.Api/MKasperczyk.GitHub.Api.csproj"
COPY . .
WORKDIR "/src/MKasperczyk.GitHub.Api"
RUN dotnet build "MKasperczyk.GitHub.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MKasperczyk.GitHub.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MKasperczyk.GitHub.Api.dll"]