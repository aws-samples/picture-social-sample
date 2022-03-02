FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Pictures.csproj", "Pictures/"]
RUN dotnet restore "Pictures/Pictures.csproj"
WORKDIR "/src/Pictures"
COPY . .
RUN dotnet build "Pictures.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pictures.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 5003
ENV ASPNETCORE_URLS=http://+:5003
# ENV AWS_ACCESS_KEY_ID=
# ENV AWS_SECRET_ACCESS_KEY=
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pictures.dll"]