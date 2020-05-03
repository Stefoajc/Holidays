#
#docker build -t holiday .
#docker run -p 5000:80 --name holidaysApi holiday
#     host port^    ^container port

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY "*.sln" "./"
COPY ["Holidays.API/*.csproj", "./Holidays.API/"]

RUN dotnet restore
COPY . .

WORKDIR "/src/Holidays.API"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Holidays.API.dll"]