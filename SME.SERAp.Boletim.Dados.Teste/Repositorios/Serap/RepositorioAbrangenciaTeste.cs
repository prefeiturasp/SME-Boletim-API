using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios.Serap
{
    internal class RepositorioAbrangenciaFake : RepositorioAbrangencia
    {
        private readonly IDbConnection _conexaoLeitura;

        public RepositorioAbrangenciaFake(ConnectionStringOptions options, IDbConnection conexaoLeitura) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
    }

    public class RepositorioAbrangenciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly RepositorioAbrangenciaFake repositorio;

        public RepositorioAbrangenciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            repositorio = new RepositorioAbrangenciaFake(new ConnectionStringOptions(), conexaoLeitura.Object);
        }

        [Fact]
        public async Task Deve_Obter_Abrangencia_Por_Login()
        {
            var login = "joao.silva";
            var abrangenciaMock = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto { Id = 1, Login = login, Grupo = "Grupo 1", DreId = 10, UeId = 20 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbrangenciaDetalheDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(abrangenciaMock);

            var resultado = await repositorio.ObterAbrangenciaPorLogin(login);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(login, resultado.First().Login);
        }

        [Fact]
        public async Task Deve_Obter_Abrangencia_Por_Login_Grupo()
        {
            var login = "maria.souza";
            var perfil = Guid.NewGuid();
            var abrangenciaMock = new List<AbrangenciaDetalheDto>
            {
                new AbrangenciaDetalheDto { Id = 2, Login = login, Perfil = perfil, Grupo = "Grupo 2", DreId = 11, UeId = 22 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbrangenciaDetalheDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(abrangenciaMock);

            var resultado = await repositorio.ObterAbrangenciaPorLoginGrupo(login, perfil);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(login, resultado.First().Login);
            Assert.Equal(perfil, resultado.First().Perfil);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Por_Abrangencia_Dre_Sem_UeId()
        {
            var dreId = 1;
            long? ueId = null;
            var uesMock = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { DreId = dreId, UeId = 10, UeNome = "Escola A" },
                new AbrangenciaUeDto { DreId = dreId, UeId = 11, UeNome = "Escola B" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbrangenciaUeDto>(
                    It.Is<string>(q => !q.Contains("u.id = @ueId")),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(uesMock);

            var resultado = await repositorio.ObterUesPorAbrangenciaDre(dreId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal(dreId, resultado.First().DreId);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Por_Abrangencia_Dre_Com_UeId()
        {
            var dreId = 1;
            long? ueId = 10;
            var uesMock = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { DreId = dreId, UeId = 10, UeNome = "Escola A" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbrangenciaUeDto>(
                    It.Is<string>(q => q.Contains("u.id = @ueId")),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(uesMock);

            var resultado = await repositorio.ObterUesPorAbrangenciaDre(dreId, ueId);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(dreId, resultado.First().DreId);
            Assert.Equal(ueId, resultado.First().UeId);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Administrador()
        {
            var uesMock = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { DreId = 1, UeId = 10, UeNome = "Escola A" },
                new AbrangenciaUeDto { DreId = 2, UeId = 11, UeNome = "Escola B" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbrangenciaUeDto>(
                    It.IsAny<string>(),
                    null, null, null, null))
                .ReturnsAsync(uesMock);

            var resultado = await repositorio.ObterUesAdministrador();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task Deve_Obter_Dres_Abrangencia_Por_Login_Perfil()
        {
            var login = "ana.pereira";
            var perfil = Guid.NewGuid();
            var dresMock = new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 1, Codigo = "123", Abreviacao = "DRE A", Nome = "Diretoria Regional A" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DreAbragenciaDetalheDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dresMock);

            var resultado = await repositorio.ObterDresAbrangenciaPorLoginPerfil(login, perfil);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("DRE A", resultado.First().Abreviacao);
        }

        [Fact]
        public async Task Deve_Obter_Dres_Administrador()
        {
            var dresMock = new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 1, Codigo = "123", Abreviacao = "DRE A", Nome = "Diretoria Regional A" },
                new DreAbragenciaDetalheDto { Id = 2, Codigo = "456", Abreviacao = "DRE B", Nome = "Diretoria Regional B" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DreAbragenciaDetalheDto>(
                    It.IsAny<string>(),
                    null, null, null, null))
                .ReturnsAsync(dresMock);

            var resultado = await repositorio.ObterDresAdministrador();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }
    }
}