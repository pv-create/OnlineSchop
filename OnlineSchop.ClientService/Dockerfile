FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OnlineSchop.ClientService/OnlineSchop.ClientService.csproj", "OnlineSchop.ClientService/"]
RUN dotnet restore "OnlineSchop.ClientService/OnlineSchop.ClientService.csproj"
COPY . .
WORKDIR "/src/OnlineSchop.ClientService"
RUN dotnet build "OnlineSchop.ClientService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OnlineSchop.ClientService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineSchop.ClientService.dll"]
