FROM mcr.microsoft.com/dotnet/sdk:6.0.101-bullseye-slim AS build
WORKDIR /app
COPY . ./

RUN dotnet restore ./RDMG/RDMG.csproj
RUN dotnet publish "RDMG/RDMG.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0.1-bullseye-slim
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "RDMG.dll"]