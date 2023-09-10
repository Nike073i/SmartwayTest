FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY Data/SmartwayTest.DAL/*.csproj Data/SmartwayTest.DAL/
COPY Data/SmartwayTest.DAL.PostgreSQL/*.csproj Data/SmartwayTest.DAL.PostgreSQL/
COPY Business/SmartwayTest.Application/*.csproj Business/SmartwayTest.Application/
COPY Presentation/SmartwayTest.FileWebService/*.csproj Presentation/SmartwayTest.FileWebService/

RUN dotnet restore Presentation/SmartwayTest.FileWebService/SmartwayTest.FileWebService.csproj

COPY . .
RUN dotnet publish --no-restore -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
EXPOSE 80
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "SmartwayTest.FileWebService.dll"]
