using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalAlunosPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalUesPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarResumoDreUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_BoletimEscolarResumoDreDto_Com_Dados_Esperados()
        {
            // Arrange
            var loteId = 1L;
            var dreId = 10L;
            var anoEscolar = 5;

            var totalUes = 12;
            var totalAlunos = 345;
            var proficiencias = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto { DisciplinaId = 1, DisciplinaNome = "Matemática", MediaProficiencia = 215.5m },
                new MediaProficienciaDisciplinaDto { DisciplinaId = 2, DisciplinaNome = "Português", MediaProficiencia = 205.2m }
            };

            var mediatorMock = new Mock<IMediator>();

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador_DRE;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotalUesPorDreQuery>(
                        q => q.LoteId == loteId && q.DreId == dreId && q.AnoEscolar == anoEscolar),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalUes);

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotalAlunosPorDreQuery>(
                        q => q.LoteId == loteId && q.DreId == dreId && q.AnoEscolar == anoEscolar),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalAlunos);

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterMediaProficienciaPorDreQuery>(
                        q => q.LoteId == loteId && q.DreId == dreId && q.AnoEscolar == anoEscolar),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficiencias);

            var useCase = new ObterBoletimEscolarResumoDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(loteId, dreId, anoEscolar);

            // Assert
            Assert.Equal(totalUes, resultado.TotalUes);
            Assert.Equal(totalAlunos, resultado.TotalAlunos);
            Assert.NotNull(resultado.ProficienciaDisciplina);

            var proficienciaMatematica = Assert.Single(resultado.ProficienciaDisciplina, x => x.DisciplinaNome == "Matemática");
            Assert.Equal(1, proficienciaMatematica.DisciplinaId);
            Assert.Equal(215.5m, proficienciaMatematica.MediaProficiencia);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Dto_Vazio_Quando_Mediator_Retorna_Valores_Default()
        {
            // Arrange
            var loteId = 2L;
            var dreId = 20L;
            var anoEscolar = 9;

            var mediatorMock = new Mock<IMediator>();

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador_DRE;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotalUesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotalAlunosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterMediaProficienciaPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MediaProficienciaDisciplinaDto>());

            var useCase = new ObterBoletimEscolarResumoDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(loteId, dreId, anoEscolar);

            // Assert
            Assert.Equal(0, resultado.TotalUes);
            Assert.Equal(0, resultado.TotalAlunos);
            Assert.Empty(resultado.ProficienciaDisciplina);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_DreId_Nao_Esta_Na_Abrangencia()
        {
            var loteId = 1L;
            var dreId = 999L; // não existe na lista de abrangência
            var anoEscolar = 5;

            var mediatorMock = new Mock<IMediator>();

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);

            var useCase = new ObterBoletimEscolarResumoDreUseCase(mediatorMock.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, dreId, anoEscolar));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Perfil_Usuario_For_Nulo()
        {
            var loteId = 1L;
            var dreId = 10L; // existe na abrangência
            var anoEscolar = 5;

            var mediatorMock = new Mock<IMediator>();

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null); // perfil nulo

            var useCase = new ObterBoletimEscolarResumoDreUseCase(mediatorMock.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, dreId, anoEscolar));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Perfil_Nao_Pode_Visualizar_Dre()
        {
            var loteId = 1L;
            var dreId = 10L;
            var anoEscolar = 5;

            var mediatorMock = new Mock<IMediator>();

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor); // supondo que Professor não pode visualizar DRE

            var useCase = new ObterBoletimEscolarResumoDreUseCase(mediatorMock.Object);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(loteId, dreId, anoEscolar));
        }


        private IEnumerable<DreAbragenciaDetalheDto> ObterDresAbrangenciaUsuarioLogado()
        {
            return new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 10, Abreviacao = "DT1", Codigo = "111", Nome = "Dre teste 1"},
                new DreAbragenciaDetalheDto { Id = 20, Abreviacao = "DT2", Codigo = "112", Nome = "Dre teste 2"}
            };
        }
    }
}