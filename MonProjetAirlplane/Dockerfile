# --- Étape de build ---
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copier les fichiers projet
COPY ["MonProjetAirlplane.csproj", "./"]

# Désactiver la recherche en ligne et forcer l'utilisation des packages locaux si disponibles,
# ou effectuer une restauration rapide
RUN dotnet restore "MonProjetAirlplane.csproj" --disable-parallel

# Copier le reste du code source
COPY . .
RUN dotnet publish "MonProjetAirlplane.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Étape finale (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MonProjetAirlplane.dll"]