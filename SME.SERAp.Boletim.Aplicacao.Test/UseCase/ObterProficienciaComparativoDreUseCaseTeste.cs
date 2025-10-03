using Moq;
using MediatR;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSPAPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterProficienciaComparativoDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolarMock;
        private readonly ObterProficienciaComparativoDreUseCase useCase;

        public ObterProficienciaComparativoDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            useCase = new ObterProficienciaComparativoDreUseCase(mediatorMock.Object, repositorioBoletimEscolarMock.Object);
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

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsa);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsp);

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Adequado");

             
            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

             
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Aplicacao);
            Assert.True(resultado.Aplicacao is IEnumerable<ProficienciaTabelaComparativaDre>);
            Assert.True(resultado.Aplicacao.GetEnumerator().MoveNext());
            Assert.True(resultado.Variacao != 0);
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaComparativaVazia_QuandoNaoHouverDados()
        {
             
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

             
            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

             
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Aplicacao);
            Assert.Equal(0,resultado.Aplicacao.First().QtdeEstudante);
            Assert.Equal(0, resultado.Aplicacao.First().ValorProficiencia);
            Assert.Equal(0, resultado.Aplicacao.First().QtdeEstudante);
            Assert.Equal(string.Empty, resultado.Aplicacao.First().NivelProficiencia);
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaComVariacaoZero_QuandoNaoHouverProficienciaPsp()
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

            var proficienciasPsp = new List<ResultadoProeficienciaPorDre>(); // Nenhum PSP

            var niveisProficiencia = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = anoEscolar, ValorReferencia = 200 }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsa);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsp);

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Adequado");

             
            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

             
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Aplicacao);
            Assert.True(resultado.Aplicacao.Any());
            Assert.Equal(0, resultado.Variacao);
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaComNivelProficienciaVazio_QuandoNiveisProficienciaNaoExistem()
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

            var niveisProficiencia = new List<ObterNivelProficienciaDto>(); // Nenhum nível

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsa);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciasPsp);

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(niveisProficiencia);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

             
            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

             
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Aplicacao);
            Assert.True(resultado.Aplicacao.Any());
            Assert.All(resultado.Aplicacao, x => Assert.True(string.IsNullOrEmpty(x.NivelProficiencia)));
        }

        [Fact]
        public async Task Executar_DeveRetornarTabelaComValoresDefault_QuandoTudoVazio()
        {
             
            int dreId = 1, anoLetivo = 2024, disciplinaId = 2, anoEscolar = 5;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSaberesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProficienciaProvaSPAPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            repositorioBoletimEscolarMock
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(disciplinaId, anoEscolar))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

             
            var resultado = await useCase.Executar(dreId, anoLetivo, disciplinaId, anoEscolar);

             
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Aplicacao);
            Assert.Equal(0, resultado.Variacao);
        }
    }


}