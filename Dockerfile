FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN sed -i 's/SECLEVEL=2/SECLEVEL=1/' /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "forex-app-service.dll"]