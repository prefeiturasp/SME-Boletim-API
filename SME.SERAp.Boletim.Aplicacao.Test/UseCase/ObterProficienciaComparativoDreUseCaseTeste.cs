using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaComparativoDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoDreUseCase useCase;

        public ObterProficienciaComparativoDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProficienciaComparativoDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaComparativaComDados()
        {
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            var proficienciasPsa = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    DreId = dreId,
                    DisciplinaId = disciplinaId,
                    RealizaramProva = 100,
                    QuantidadeUes = 10,
                    DreAbreviacao = "DRE1",
                    DreNome = "Diretoria 1",
                    AnoEscolar = anoEscolar.ToString(),
                    MediaProficiencia = 250,
                    NomeAplicacao = "PSA",
                    Periodo = "Março"
                }
            };

            var proficienciasPsp = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    DreId = dreId,
                    DisciplinaId = disciplinaId,
                    RealizaramProva = 90,
                    QuantidadeUes = 9,
                    DreAbreviacao = "DRE1",
                    DreNome = "Diretoria 1",
                    AnoEscolar = (anoEscolar - 1).ToString(),
                    MediaProficiencia = 230,
                    NomeAplicacao = "PSP",
                    Periodo = "Março"
                }
            };

            var niveisProficiencia = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = anoEscolar, ValorReferencia = 200 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsa);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsp);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Adequado");

            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.Aplicacao);
            Assert.True(resultado.Variacao != 0);
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaVazia_QuandoNaoHouverDados()
        {
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Aplicacao);
            var aplicacao = resultado.Aplicacao.First();
            Assert.Equal(0, aplicacao.QtdeEstudante);
            Assert.Equal(0, aplicacao.ValorProficiencia);
            Assert.Equal(string.Empty, aplicacao.NivelProficiencia);
        }

        [Fact]
        public async Task Executar_DeveRetornarVariacaoZero_QuandoNaoHouverProficienciaPsp()
        {
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            var proficienciasPsa = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    MediaProficiencia = 250,
                    NomeAplicacao = "PSA",
                    Periodo = "Março",
                    RealizaramProva = 100,
                    QuantidadeUes = 10
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsa);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Adequado");

            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Variacao);
        }

        [Fact]
        public async Task Executar_DeveRetornarNivelProficienciaVazio_QuandoNiveisNaoExistirem()
        {
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            var psa = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { MediaProficiencia = 250, NomeAplicacao = "PSA", Periodo = "Março", RealizaramProva = 100, QuantidadeUes = 10 }
            };

            var psp = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { MediaProficiencia = 230, NomeAplicacao = "PSP", Periodo = "Março", RealizaramProva = 90, QuantidadeUes = 9 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(psa);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(psp);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

            Assert.All(resultado.Aplicacao, x => Assert.True(string.IsNullOrEmpty(x.NivelProficiencia)));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoUsuarioNaoTemAbrangencia()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(1, 2024, 2, 5));
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoPerfilNulo()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = 1 } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(1, 2024, 2, 5));
        }

        [Fact]
        public async Task Executar_DeveRetornarVariacaoPositiva_QuandoPsaMaiorQuePsp()
        {
            var dreId = 1;
            var disciplinaId = 2;
            var anoEscolar = 5;
            var anoLetivo = 2024;

            var psa = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { MediaProficiencia = 300, NomeAplicacao = "PSA", Periodo = "Março", RealizaramProva = 100, QuantidadeUes = 10 }
            };

            var psp = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre { MediaProficiencia = 200, NomeAplicacao = "PSP", Periodo = "Março", RealizaramProva = 80, QuantidadeUes = 8 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto> { new DreAbragenciaDetalheDto { Id = dreId } });
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador_DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(psa);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(psp);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Adequado");

            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

            Assert.True(resultado.Variacao > 0);
        }
    }
}