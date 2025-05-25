# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todos os arquivos do projeto
COPY . . 

# Restaura as dependências
RUN dotnet restore

# Publica o projeto em modo Release
RUN dotnet publish -c Release -o /app/out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos da publicação
COPY --from=build /app/out .

# Define variáveis de ambiente e expõe a porta
ENV ASPNETCORE_URLS=http://+:5091
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5091

# Executa a aplicação
ENTRYPOINT ["dotnet", "ProjetoAuvo.dll"]