#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["10/MinimalApiTest.WebApi/MinimalApiTest.WebApi.csproj", "10/MinimalApiTest.WebApi/"]
RUN dotnet restore "10/MinimalApiTest.WebApi/MinimalApiTest.WebApi.csproj"
COPY . .
WORKDIR "/src/10/MinimalApiTest.WebApi"
RUN dotnet build "MinimalApiTest.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApiTest.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApiTest.WebApi.dll"]