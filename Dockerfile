FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY src/*.sln ./
COPY src/TestSystem/*.csproj ./TestSystem/
COPY src/TestSystem.BusinessLogic/*.csproj ./TestSystem.BusinessLogic/
COPY src/TestSystem.Database.Cosmos/*.csproj ./TestSystem.Database.Cosmos/
COPY src/TestSystem.Database.SqlServer/*.csproj ./TestSystem.Database.SqlServer/
COPY src/TestSystem.BusinessLogic.Tests/*.csproj ./TestSystem.BusinessLogic.Tests/
COPY src/TestSystem.Tests/*.csproj ./TestSystem.Tests/
RUN dotnet restore
COPY src .
RUN dotnet test ./TestSystem.Tests/ --no-restore 
RUN dotnet test ./TestSystem.BusinessLogic.Tests/ --no-restore 
RUN dotnet publish ./TestSystem/TestSystem.csproj --no-self-contained -c Release -o /publish 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build-env /publish .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "exec", "TestSystem.dll"]
