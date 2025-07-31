using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterAbaEstudanteGraficoPorUeIdUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_Dados_Quando_Usuario_Tem_Abrangencia()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimEstudanteDto
            {
                NomeEstudante = "João",
                NivelMinimo = 0,
                NivelMaximo = 10,
                NivelProficiencia = new List<int> { 1, 2 },
                Turma = new List<string> { "A", "B" }
            };

            var abrangenciasUsuarioLogado = new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto
                {
                    DreId = 10,
                    UeId = ueId,
                    DreAbreviacao = "DRE - Centro",
                    UeNome = "Escola Teste",
                    UeTipo = SME.SERAp.Boletim.Dominio.Enumerados.TipoEscola.Nenhum
                }
            };

            var abaEstudanteGraficoDtos = new List<AbaEstudanteGraficoDto>
            {
                new AbaEstudanteGraficoDto
                {
                    Turma = "A",
                    Disciplina = "Matemática",
                    Alunos = new List<AbaEstudanteGraficoAlunoDto>()
                }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasUsuarioLogado.AsEnumerable());

            mediatorMock.Setup(m => m.Send(It.Is<ObterAbaEstudanteGraficoPorUeIdQuery>(
                q => q.LoteId == loteId && q.UeId == ueId && q.Filtros == filtros), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abaEstudanteGraficoDtos);

            var useCase = new ObterAbaEstudanteGraficoPorUeIdUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(loteId, ueId, filtros);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("A", resultado.First().Turma);
            Assert.Equal("Matemática", resultado.First().Disciplina);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Usuario_Nao_Tem_Abrangencia()
        {
            
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimEstudanteDto();

            var abrangenciasVazias = new List<AbrangenciaUeDto>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangenciasVazias.AsEnumerable());

            var useCase = new ObterAbaEstudanteGraficoPorUeIdUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, ueId, filtros));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }
    }
}
