using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCases
{
    public class ObterAbaEstudanteBoletimEscolarPorUeIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAbaEstudanteBoletimEscolarPorUeIdUseCase _useCase;

        public ObterAbaEstudanteBoletimEscolarPorUeIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterAbaEstudanteBoletimEscolarPorUeIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Boletim_Com_Disciplinas_Quando_Abrangencia_Valida()
        {
            // Arrange
            var ueId = 123;
            var loteId = 1;
            var filtros = new FiltroBoletimEstudantePaginadoDto { PageNumber = 1, PageSize = 10 };

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId }
            };

            var estudantes = new List<AbaEstudanteListaDto>
            {
                new AbaEstudanteListaDto { Disciplina = "Matemática" },
                new AbaEstudanteListaDto { Disciplina = "Português" },
                new AbaEstudanteListaDto { Disciplina = "Matemática" } // Repetido para testar Distinct
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbaEstudanteBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((estudantes, estudantes.Count));

            // Act
            var resultado = await _useCase.Executar(loteId, ueId, filtros);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Disciplinas.Should().BeEquivalentTo(new[] { "Matemática", "Português" });
            resultado.Estudantes.TotalRegistros.Should().Be(3);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_UeId_Nao_Estiver_Nas_Abrangencias()
        {
            // Arrange
            var loteId = 1;
            var ueId = 999;
            var filtros = new FiltroBoletimEstudantePaginadoDto();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>
                {
                    new AbrangenciaUeDto { UeId = 111 } // Diferente do ueId
                });

            // Act & Assert
            await Assert.ThrowsAsync<NaoAutorizadoException>(() => _useCase.Executar(loteId, ueId, filtros));
        }

        [Fact]
        public async Task Deve_Retornar_Listas_Vazias_Quando_Nao_Houver_Estudantes()
        {
            // Arrange
            var loteId = 1;
            var ueId = 123;
            var filtros = new FiltroBoletimEstudantePaginadoDto();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>
                {
                    new AbrangenciaUeDto { UeId = ueId }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbaEstudanteBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enumerable.Empty<AbaEstudanteListaDto>(), 0));

            // Act
            var resultado = await _useCase.Executar(loteId, ueId, filtros);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Disciplinas.Should().BeEmpty();
            resultado.Estudantes.Itens.Should().BeEmpty();
            resultado.Estudantes.TotalRegistros.Should().Be(0);
        }
    }
}