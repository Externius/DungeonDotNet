FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
COPY . ./

RUN dotnet restore ./MvcRDMG/MvcRDMG.csproj
RUN dotnet publish "MvcRDMG/MvcRDMG.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "MvcRDMG.dll"]