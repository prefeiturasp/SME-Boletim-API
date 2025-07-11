using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterAnosEscolaresPorLoteIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAnosEscolaresPorLoteIdUseCase obterAnosEscolaresPorLoteIdUseCase;
        public ObterAnosEscolaresPorLoteIdUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.obterAnosEscolaresPorLoteIdUseCase = new ObterAnosEscolaresPorLoteIdUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_AnoEscolar()
        {
            var anosEscolares = new List<AnoEscolarDto>
            {
                new AnoEscolarDto { Ano = 5, Modalidade = Dominio.Enumerados.Modalidade.Fundamental },
                new AnoEscolarDto { Ano = 9, Modalidade = Dominio.Enumerados.Modalidade.Fundamental },
                new AnoEscolarDto { Ano = 1, Modalidade = Dominio.Enumerados.Modalidade.Medio },
                new AnoEscolarDto { Ano = 2, Modalidade = Dominio.Enumerados.Modalidade.Medio },
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(anosEscolares);

            var resultado = await obterAnosEscolaresPorLoteIdUseCase.Executar(1);

            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            Assert.NotNull(resultado);
            Assert.Equal(anosEscolares.Count, resultado.Count());
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_AnoEscolar_Somente_Ano_Ensino_Fundamental()
        {
            var anosEscolares = new List<AnoEscolarDto>
            {
                new AnoEscolarDto { Ano = 5, Modalidade = Dominio.Enumerados.Modalidade.Fundamental },
                new AnoEscolarDto { Ano = 9, Modalidade = Dominio.Enumerados.Modalidade.Fundamental }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(anosEscolares);

            var resultado = await obterAnosEscolaresPorLoteIdUseCase.Executar(1);

            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            Assert.NotNull(resultado);
            Assert.Equal(anosEscolares.Count, resultado.Count());
            Assert.Collection(resultado,
                anoEscolar => Assert.Equal("5º Ano", anoEscolar.Descricao),
                anoEscolar => Assert.Equal("9º Ano", anoEscolar.Descricao));
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_AnoEscolar_Somente_Ano_Ensino_Medio()
        {
            var anosEscolares = new List<AnoEscolarDto>
            {
                new AnoEscolarDto { Ano = 1, Modalidade = Dominio.Enumerados.Modalidade.Medio },
                new AnoEscolarDto { Ano = 2, Modalidade = Dominio.Enumerados.Modalidade.Medio }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(anosEscolares);

            var resultado = await obterAnosEscolaresPorLoteIdUseCase.Executar(1);

            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            Assert.NotNull(resultado);
            Assert.Equal(anosEscolares.Count, resultado.Count());
            Assert.Collection(resultado,
                anoEscolar => Assert.Equal("1ª Serie", anoEscolar.Descricao),
                anoEscolar => Assert.Equal("2ª Serie", anoEscolar.Descricao));
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Lista_De_AnoEscolar()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult<IEnumerable<AnoEscolarDto>>(new List<AnoEscolarDto>()));

            var resultado = await obterAnosEscolaresPorLoteIdUseCase.Executar(1);

            mediator.Verify(m => m.Send(It.IsAny<ObterAnosEscolaresPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            Assert.Empty(resultado);
        }
    }
}
