#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Checkout.PaymentGateway.API/Checkout.PaymentGateway.API.csproj", "Checkout.PaymentGateway.API/"]
RUN dotnet restore "Checkout.PaymentGateway.API/Checkout.PaymentGateway.API.csproj"
COPY . .
WORKDIR "/src/Checkout.PaymentGateway.API"
RUN dotnet build "Checkout.PaymentGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.API.dll"]