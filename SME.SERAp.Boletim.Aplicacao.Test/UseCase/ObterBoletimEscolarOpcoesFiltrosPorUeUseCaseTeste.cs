using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesAnoEscolarBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterOpcoesTurmaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterValoresNivelProficienciaBoletimEscolarPorUeId;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarOpcoesFiltrosPorUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Opcoes_Filtros_Quando_Usuario_Tem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 100L;

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = ueId, DreId = 1, UeNome = "Escola X", UeTipo = TipoEscola.Nenhum, DreAbreviacao = "DRE" }
            };

            var niveis = new List<OpcaoFiltroDto<int>>
            {
                new() { Texto = "Nível 1", Valor = 1 },
                new() { Texto = "Nível 2", Valor = 2 }
            };

            var anosEscolares = new List<OpcaoFiltroDto<int>>
            {
                new() { Texto = "Ano 1", Valor = 1 },
                new() { Texto = "Ano 2", Valor = 2 }
            };

            var componentesCurriculares = new List<OpcaoFiltroDto<int>>
            {
                new() { Texto = "Matemática", Valor = 101 },
                new() { Texto = "Português", Valor = 102 }
            };

            var turmas = new List<OpcaoFiltroDto<string>>
            {
                new() { Texto = "Turma A", Valor = "A" },
                new() { Texto = "Turma B", Valor = "B" }
            };

            var valoresProficiencia = new BoletimEscolarValoresNivelProficienciaDto
            {
                ValorMinimo = 18,
                ValorMaximo = 86
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasUsuarioLogado.AsEnumerable());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterOpcoesNiveisProficienciaBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveis);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterOpcoesAnoEscolarBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosEscolares);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterOpcoesComponenteCurricularBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesCurriculares);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterOpcoesTurmaBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterValoresNivelProficienciaBoletimEscolarPorUeIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(valoresProficiencia);

            var useCase = new ObterBoletimEscolarOpcoesFiltrosPorUeUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(niveis.Count, resultado.Niveis.Count());
            Assert.Equal(anosEscolares.Count, resultado.AnosEscolares.Count());
            Assert.Equal(componentesCurriculares.Count, resultado.ComponentesCurriculares.Count());
            Assert.Equal(turmas.Count, resultado.Turmas.Count());

            
            Assert.Equal(0, resultado.NivelMinimo);
            Assert.Equal(100, resultado.NivelMaximo);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Quando_Usuario_Nao_Tem_Abrangencia()
        {

            var loteId = 1L;
            var ueId = 100L;

            var abrangenciasVazias = new List<AbrangenciaUeDto>();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasVazias.AsEnumerable());

            var useCase = new ObterBoletimEscolarOpcoesFiltrosPorUeUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }
    }
}
