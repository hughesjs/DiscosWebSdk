FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY . .
WORKDIR "src/DiscosWebSdk/DiscosWebSdk.Tests"
ARG DISCOS_API_KEY
ARG DISCOS_API_URL
ENV DISCOS_API_KEY=$DISCOS_API_KEY
ENV DISCOS_API_URL=$DISCOS_API_URL
ENTRYPOINT dotnet test DiscosWebSdk.Tests.csproj
