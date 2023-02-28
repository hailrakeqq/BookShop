FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dotnet-build
WORKDIR /src
EXPOSE 5013
ENV ASPNETCORE_URLS=http://+:5045
COPY . /src
RUN dotnet restore "BookShop.API.csproj"
RUN dotnet build "BookShop.API.csproj" -c Release -o /app/build

FROM dotnet-build AS dotnet-publish
RUN dotnet publish "BookShop.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
RUN mkdir /app/wwwroot
COPY --from=dotnet-publish /app/publish .

ENTRYPOINT ["dotnet", "BookShop.API.dll", "urls=http://localhost:5045"]
