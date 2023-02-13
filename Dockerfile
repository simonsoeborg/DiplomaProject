#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GroenlundAPI/GroenlundAPI.csproj", "GroenlundAPI/"]
RUN dotnet restore "GroenlundAPI/GroenlundAPI.csproj"
COPY . .
WORKDIR "/src/GroenlundAPI"
RUN dotnet build "GroenlundAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GroenlundAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GroenlundAPI.dll"]