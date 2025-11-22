FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . ./

RUN dotnet restore ./src/RDMG.Web/RDMG.Web.csproj
RUN dotnet publish "src/RDMG.Web/RDMG.Web.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "RDMG.Web.dll"]