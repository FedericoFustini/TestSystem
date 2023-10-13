ARG build_container
ARG runtime_container
FROM $build_container as build-env
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
RUN dotnet publish ./TestSystem/TestSystem.csproj --no-self-contained -c Release -o /publish 

FROM $runtime_container as runtime
RUN apt-get update 
RUN apt-get --yes install curl
WORKDIR /app
COPY --from=build-env /publish .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
HEALTHCHECK  --interval=30s --timeout=30s --start-period=10s --retries=3 \
  CMD curl --silent --fail http://localhost:80/healthz || exit 1
ENTRYPOINT ["dotnet", "exec", "TestSystem.dll"]
