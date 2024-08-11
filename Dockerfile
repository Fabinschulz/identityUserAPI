# FROM: Define a imagem base para o contêiner
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app

# WORKDIR: É o diretório de trabalho para trabalhar dentro do contêiner
WORKDIR /app

# EXPOSE: Informa ao Docker que o contêiner escuta as portas em tempo de execução - 8080 e 8081
EXPOSE 8080
EXPOSE 8081

# Estágio de construção do aplicativo
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityUser.csproj", "."]

# RUN: executa um comando no contêiner
RUN dotnet restore "./IdentityUser.csproj"

# COPY: Copie todos os arquivos do diretório atual para o contêiner
COPY . . 

WORKDIR "/src/."
RUN dotnet build "./IdentityUser.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de publicação do aplicativo
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityUser.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio final do aplicativo - Imagem de produção
FROM base AS final
WORKDIR /app

# COPY --from: Significa que você deseja copiar arquivos de outro estágio de construção exemplo: "publish"
COPY --from=publish /app/publish .

# ENTRYPOINT: Define o comando que será executado quando o contêiner for iniciado
ENTRYPOINT ["dotnet", "IdentityUser.dll"]