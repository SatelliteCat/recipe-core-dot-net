#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["recipe-core-dotnet.csproj", "recipe-core-dotnet/"]
#RUN dotnet restore "recipe-core-dotnet/recipe-core-dotnet.csproj"
#COPY . .
#WORKDIR "/src/recipe-core-dotnet"
#RUN dotnet build "recipe-core-dotnet.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "recipe-core-dotnet.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "recipe-core-dotnet.dll"]





# Используйте образ .NET 7 SDK в качестве базового образа для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Установите порт, на котором будет работать приложение
EXPOSE 80
EXPOSE 443

# Копируйте файлы проекта и восстановите зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируйте остальные файлы проекта и выполните сборку приложения
COPY . ./
RUN dotnet publish -c Release -o out

# Используйте образ .NET 7 Runtime в качестве базового образа для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Копируйте результат сборки из предыдущего этапа в текущую директорию
COPY --from=build-env /app/out .

# Укажите точку входа для запуска приложения
ENTRYPOINT ["dotnet", "recipe-core-dotnet.dll"]

#docker run --hostname=234a0c218d83 --mac-address=02:42:ac:11:00:02 --env=ASPNETCORE_ENVIRONMENT=Development --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=ASPNETCORE_URLS= --env=DOTNET_RUNNING_IN_CONTAINER=true --env=DOTNET_VERSION=7.0.5 --env=ASPNET_VERSION=7.0.5 --env=DOTNET_GENERATE_ASPNET_CERTIFICATE=false --env=DOTNET_NOLOGO=true --env=DOTNET_SDK_VERSION=7.0.203 --env=DOTNET_USE_POLLING_FILE_WATCHER=true --env=NUGET_XMLDOC_MODE=skip --env=POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Debian-11 --volume=C:\Users\dmnin\Documents\projects\cs\recipe-core-dotnet\recipe-core-dotnet:/app:rw --workdir=/app -p 3001:80 --runtime=runc -d 70907d17e071
