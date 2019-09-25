FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

# copy sln&csproj and restore as distinct layers
COPY src/SimpleSocial/*.sln .
COPY src/SimpleSocial/Data/SimpleSocial.Data.Common/*.csproj ./SimpleSocial/Data/SimpleSocial.Data.Common/
COPY src/SimpleSocial/Data/SimpleSocial.Data.Models/*.csproj ./SimpleSocial/Data/SimpleSocial.Data.Models/
COPY src/SimpleSocial/Data/SimpleSocial.Data/*.csproj ./SimpleSocial/Data/SimpleSocial.Data/
COPY src/SimpleSocial/Services/SimpleSocia.Services.Models/*.csproj ./SimpleSocial/Services/SimpleSocia.Services.Models/
COPY src/SimpleSocial/Services/SimpleSocial.Services.DataServices/*.csproj ./SimpleSocial/DServices/SimpleSocial.Services.DataServices/
COPY src/SimpleSocial/Services/SimpleSocial.Services.Mapping/*.csproj ./SimpleSocial/Services/SimpleSocial.Services.Mapping/
COPY src/SimpleSocial/Tests/SandBox/*.csproj ./SimpleSocial/Tests/SandBox/
COPY src/SimpleSocial/Tests/SimpleSocial.Services.DataServices.Tests/*.csproj ./SimpleSocial/Tests/SimpleSocial.Services.DataServices.Tests/
COPY src/SimpleSocial/Web/SimpleSocial.Web.Chat/*.csproj ./SimpleSocial/Web/SimpleSocial.Web.Chat/
COPY src/SimpleSocial/Web/SimpleSocial.Web/*.csproj ./SimpleSocial/Web/SimpleSocial.Web/
#RUN dotnet restore

# copy everything else and build each folder app
COPY src/SimpleSocial/. ./SimpleSocial/
WORKDIR /src/SimpleSocial/Data/SimpleSocial.Data.Common
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Data/SimpleSocial.Data.Models
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Data/SimpleSocial.Data
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Services/SimpleSocia.Services.Models
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Services/SimpleSocial.Services.DataServices
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Services/SimpleSocial.Services.Mapping
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Tests/SandBox
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Tests/SimpleSocial.Services.DataServices.Tests
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Web/SimpleSocial.Web.Chat
RUN dotnet build -c Release -o /bld

WORKDIR /src/SimpleSocial/Web/SimpleSocial.Web
RUN dotnet build -c Release -o /bld

FROM build AS publish
RUN dotnet publish -c Release -o /out

#FROM base AS final
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /out .
ENTRYPOINT ["dotnet", "SimpleSocial.dll"]

#RUN dotnet publish -c Release -o out


#FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
#WORKDIR /app
#COPY --from=build /app/SimpleSocial/out ./
#ENTRYPOINT ["dotnet", "SimpleSocial.dll"]
