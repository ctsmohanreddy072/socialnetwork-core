FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/SimpleSocial/*.csproj ./SimpleSocial/
RUN dotnet restore
# copy everything else and build app
COPY src/SimpleSocial/. ./SimpleSocial/
WORKDIR /app/SimpleSocial
RUN dotnet publish -c Release -o out
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
