# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todos os arquivos do projeto
COPY . . 

# Restaura as depend�ncias
RUN dotnet restore

# Publica o projeto em modo Release
RUN dotnet publish -c Release -o /app/out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos da publica��o
COPY --from=build /app/out .

# Define vari�veis de ambiente e exp�e a porta
ENV ASPNETCORE_URLS=http://+:5091
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5091

# Executa a aplica��o
ENTRYPOINT ["dotnet", "ProjetoAuvo.dll"]