using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProva;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries
{
    public class ObterLotesProvaQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lotes_Quando_Existir()
        {
            var lotesEsperados = new List<LoteProvaDto>
            {
                new LoteProvaDto
                {
                    Id = 1,
                    Nome = "Lote 1",
                    TipoTai = true,
                    ExibirNoBoletim = true,
                    DataInicioLote = new DateTime(2024, 5, 1)
                },
                new LoteProvaDto
                {
                    Id = 2,
                    Nome = "Lote 2",
                    TipoTai = false,
                    ExibirNoBoletim = false,
                    DataInicioLote = new DateTime(2024, 6, 1)
                }
            };

            var repositorioLoteProvaMock = new Mock<IRepositorioLoteProva>();
            repositorioLoteProvaMock
                .Setup(r => r.ObterLotesProva())
                .ReturnsAsync(lotesEsperados);

            var handler = new ObterLotesProvaQueryHandler(repositorioLoteProvaMock.Object);
            var query = new ObterLotesProvaQuery();

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<LoteProvaDto>)resultado).Count);
            Assert.Equal("Lote 1", ((List<LoteProvaDto>)resultado)[0].Nome);
            Assert.Equal("Lote 2", ((List<LoteProvaDto>)resultado)[1].Nome);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_Lotes()
        {
            var repositorioLoteProvaMock = new Mock<IRepositorioLoteProva>();
            repositorioLoteProvaMock
                .Setup(r => r.ObterLotesProva())
                .ReturnsAsync(new List<LoteProvaDto>());

            var handler = new ObterLotesProvaQueryHandler(repositorioLoteProvaMock.Object);
            var query = new ObterLotesProvaQuery();

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}