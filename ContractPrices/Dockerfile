FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ContractPrices/ContractPrices.csproj", "ContractPrices/"]
RUN dotnet restore "ContractPrices/ContractPrices.csproj"
COPY . .
WORKDIR "/src/ContractPrices"
RUN dotnet build "ContractPrices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContractPrices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractPrices.dll"]
