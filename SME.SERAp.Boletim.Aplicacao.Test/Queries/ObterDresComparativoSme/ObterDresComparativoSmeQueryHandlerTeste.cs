using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDresComparativoSme;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDresComparativoSmeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterDresComparativoSmeQueryHandler handler;

        public ObterDresComparativoSmeQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            handler = new ObterDresComparativoSmeQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dres_Comparativo_Sme_Com_Sucesso()
        {
            var anoAplicacao = 2025;
            var disciplinaId = 5;
            var anoEscolar = 5;

            var dresEsperadas = new List<DreDto>
            {
                new DreDto { DreId = 1, DreNome = "DRE Teste 1" },
                new DreDto { DreId = 2, DreNome = "DRE Teste 2" }
            };

            repositorio
                .Setup(r => r.ObterDresComparativoSmeAsync(anoAplicacao, disciplinaId, anoEscolar))
                .ReturnsAsync(dresEsperadas);

            var query = new ObterDresComparativoSmeQuery(anoAplicacao, disciplinaId, anoEscolar);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(dresEsperadas.Count, resultado.Count());
            Assert.Contains(resultado, x => x.DreId == 1 && x.DreNome == "DRE Teste 1");
            repositorio.Verify(r => r.ObterDresComparativoSmeAsync(anoAplicacao, disciplinaId, anoEscolar), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Existirem_Dres()
        {
            var query = new ObterDresComparativoSmeQuery(2025, 5, 5);

            repositorio
                .Setup(r => r.ObterDresComparativoSmeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<DreDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            repositorio.Verify(r => r.ObterDresComparativoSmeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterDresComparativoSmeQueryHandler(null));
        }

        [Fact]
        public async Task Deve_Chamar_Metodo_Do_Repositorio_Com_Parametros_Corretos()
        {
            var anoAplicacao = 2025;
            var disciplinaId = 5;
            var anoEscolar = 8;

            var query = new ObterDresComparativoSmeQuery(anoAplicacao, disciplinaId, anoEscolar);

            repositorio
                .Setup(r => r.ObterDresComparativoSmeAsync(anoAplicacao, disciplinaId, anoEscolar))
                .ReturnsAsync(new List<DreDto>());

            await handler.Handle(query, CancellationToken.None);

            repositorio.Verify(r =>
                r.ObterDresComparativoSmeAsync(
                    It.Is<int>(a => a == anoAplicacao),
                    It.Is<int>(d => d == disciplinaId),
                    It.Is<int>(e => e == anoEscolar)
                ), Times.Once);
        }
    }
}
