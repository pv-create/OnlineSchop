FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["OnlineSchop.OrderService/OnlineSchop.OrderService.csproj", "OnlineSchop.OrderService/"]
RUN dotnet restore "OnlineSchop.OrderService/OnlineSchop.OrderService.csproj"
COPY . .
WORKDIR "/src/OnlineSchop.OrderService"
RUN dotnet build "OnlineSchop.OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineSchop.OrderService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "OnlineSchop.OrderService.dll"]