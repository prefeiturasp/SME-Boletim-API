FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN if ! dotnet nuget list source | grep -q "https://api.nuget.org/v3/index.json"; then dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org; fi

COPY ["SME.SERAp.Boletim.Api/SME.SERAp.Boletim.Api.csproj", "SME.SERAp.Boletim.Api/"]
COPY ["SME.SERAp.Boletim.Aplicacao/SME.SERAp.Boletim.Aplicacao.csproj", "SME.SERAp.Boletim.Aplicacao/"]
COPY ["SME.SERAp.Boletim.Dados/SME.SERAp.Boletim.Dados.csproj", "SME.SERAp.Boletim.Dados/"]
COPY ["SME.SERAp.Boletim.Dominio/SME.SERAp.Boletim.Dominio.csproj", "SME.SERAp.Boletim.Dominio/"]
COPY ["SME.SERAp.Boletim.Infra/SME.SERAp.Boletim.Infra.csproj", "SME.SERAp.Boletim.Infra/"]
COPY ["SME.SERAp.Boletim.IoC/SME.SERAp.Boletim.IoC.csproj", "SME.SERAp.Boletim.IoC/"]
RUN dotnet restore "SME.SERAp.Boletim.Api/SME.SERAp.Boletim.Api.csproj"
COPY . .
WORKDIR "/src/SME.SERAp.Boletim.Api"
RUN dotnet build "SME.SERAp.Boletim.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SERAp.Boletim.Api.csproj" -c Release -o /app/publish

FROM base AS final 
WORKDIR /app
ENV TZ=America/Sao_Paulo
RUN apt-get update \
    && apt-get install -yq tzdata locales -y \
    && dpkg-reconfigure --frontend noninteractive tzdata \
	  && locale-gen en_US.UTF-8
	  
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SME.SERAp.Boletim.Api.dll"]
