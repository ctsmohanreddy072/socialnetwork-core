FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
RUN dotnet restore
# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/SimpleSocial/*.csproj ./SimpleSocial/
RUN dotnet restore
# copy everything else and build app
COPY src/SimpleSocial/. ./SimpleSocial/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out
