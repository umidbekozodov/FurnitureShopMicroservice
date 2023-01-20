FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /sourse
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build-env /publish /app
EXPOSE 84
ENTRYPOINT ["dotnet", "DashboardApi.dll"]