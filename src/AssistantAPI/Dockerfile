FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY AssistantAPI/AssistantAPI.csproj AssistantAPI/
RUN dotnet restore AssistantAPI/AssistantAPI.csproj
COPY . .
RUN dotnet publish AssistantAPI/AssistantAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "AssistantAPI.dll"]
