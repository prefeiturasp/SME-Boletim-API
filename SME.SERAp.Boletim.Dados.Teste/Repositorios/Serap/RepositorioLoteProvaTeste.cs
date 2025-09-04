using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios.Serap
{
    internal class RepositorioLoteProvaFake : RepositorioLoteProva
    {
        private readonly IDbConnection _conexaoLeitura;

        public RepositorioLoteProvaFake(ConnectionStringOptions options, IDbConnection conexaoLeitura) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
    }

    public class RepositorioLoteProvaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly RepositorioLoteProvaFake repositorio;

        public RepositorioLoteProvaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            repositorio = new RepositorioLoteProvaFake(new ConnectionStringOptions(), conexaoLeitura.Object);
        }

        [Fact]
        public async Task Deve_Obter_Lotes_Prova()
        {
            var lotesProvaMock = new List<LoteProvaDto>
            {
                new LoteProvaDto { Id = 1, Nome = "Lote 1", TipoTai = true, ExibirNoBoletim = true, DataInicioLote = new System.DateTime(2023, 1, 1) },
                new LoteProvaDto { Id = 2, Nome = "Lote 2", TipoTai = true, ExibirNoBoletim = false, DataInicioLote = new System.DateTime(2023, 2, 1) }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<LoteProvaDto>(
                    It.IsAny<string>(),
                    null, null, null, null))
                .ReturnsAsync(lotesProvaMock);

            var resultado = await repositorio.ObterLotesProva();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Lote 1", resultado.First().Nome);
            Assert.Equal("Lote 2", resultado.Last().Nome);
        }

        [Fact]
        public async Task Deve_Obter_Anos_Escolares_Por_LoteId()
        {
            var loteId = 123;

            var anosEscolaresMock = new List<AnoEscolarDto>
            {
                new AnoEscolarDto { Ano = 5, Modalidade = Modalidade.Fundamental },
                new AnoEscolarDto { Ano = 6, Modalidade = Modalidade.Fundamental }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AnoEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(anosEscolaresMock);

            var resultado = await repositorio.ObterAnosEscolaresPorLoteId(loteId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal(5, resultado.First().Ano);
            Assert.Equal(6, resultado.Last().Ano);
        }
    }
}