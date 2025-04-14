# Base image olarak .NET SDK'yı kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala
COPY ["TıbbiTakipSistem/TıbbiTakipSistem.csproj", "TıbbiTakipSistem/"]
RUN dotnet restore "TıbbiTakipSistem/TıbbiTakipSistem.csproj"

# Tüm kaynak kodları kopyala
COPY . .
WORKDIR "/src/TıbbiTakipSistem"

# Uygulamayı derle
RUN dotnet build "TıbbiTakipSistem.csproj" -c Release -o /app/build

# Yayınlama
FROM build AS publish
RUN dotnet publish "TıbbiTakipSistem.csproj" -c Release -o /app/publish

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# SSL sertifikası oluştur
RUN apt-get update && \
    apt-get install -y openssl && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /app/server.key -out /app/server.crt \
    -subj "/CN=localhost" && \
    chmod 644 /app/server.crt && \
    chmod 644 /app/server.key

ENV ASPNETCORE_URLS="http://+:80;https://+:443"
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/server.crt
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/app/server.key
ENV ASPNETCORE_Kestrel__Endpoints__Http__Url=http://+:80
ENV ASPNETCORE_Kestrel__Endpoints__Https__Url=https://+:443

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "TıbbiTakipSistem.dll"] 