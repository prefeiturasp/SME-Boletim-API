using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterBoletimEscolarPorUe
{
    public class ObterBoletimPorUeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimMock;
        private readonly Mock<IRepositorioCache> repositorioCacheMock;
        private readonly ObterBoletimPorUeQueryHandler handler;

        public ObterBoletimPorUeQueryHandlerTeste()
        {
            repositorioCacheMock = new Mock<IRepositorioCache>();
            repositorioBoletimMock = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterBoletimPorUeQueryHandler(repositorioBoletimMock.Object, repositorioCacheMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Boletim_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimDto();
            var boletimEsperado = new List<BoletimEscolar>
            {
                new BoletimEscolar { UeId = ueId, ProvaId = 10, ComponenteCurricular = "Matemática" }
            };

            var repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            var repositorioCacheMock = new Mock<IRepositorioCache>();

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterBoletinsPorUe(loteId, ueId, filtros))
                .ReturnsAsync(boletimEsperado);

            var handler = new ObterBoletimPorUeQueryHandler(repositorioBoletimEscolarMock.Object, repositorioCacheMock.Object);
            var query = new ObterBoletimEscolarPorUeQuery(loteId, ueId, filtros);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(ueId, ((List<BoletimEscolar>)resultado)[0].UeId);
            Assert.Equal("Matemática", ((List<BoletimEscolar>)resultado)[0].ComponenteCurricular);
        }

        [Fact]
        public async Task Deve_Retornar_Nulo_Quando_Nao_Existir_Boletim()
        {
            var loteId = 1L;
            var ueId = 999L;
            var filtros = new FiltroBoletimDto();

            repositorioBoletimMock
                .Setup(r => r.ObterBoletinsPorUe(loteId, ueId, filtros))
                .ReturnsAsync((IEnumerable<BoletimEscolar>)null);

            var query = new ObterBoletimEscolarPorUeQuery(loteId, ueId, filtros);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.Null(resultado);
        }
    }
}