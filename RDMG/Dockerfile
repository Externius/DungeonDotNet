FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . ./

RUN dotnet restore ./RDMG/RDMG.csproj
RUN dotnet publish "RDMG/RDMG.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "RDMG.dll"]