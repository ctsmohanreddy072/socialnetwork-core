FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# copy sln and restore as distinct layers
COPY src/SimpleSocial/*.sln .
#RUN dotnet restore

# copy everything else and build app
COPY src/SimpleSocial/. ./SimpleSocial/
WORKDIR /app/SimpleSocial
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/SimpleSocial/out ./
ENTRYPOINT ["dotnet", "SimpleSocial.dll"]
