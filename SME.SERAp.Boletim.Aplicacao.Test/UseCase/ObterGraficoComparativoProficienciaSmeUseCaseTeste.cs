using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaPorSmeProvaSaberes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciasPorSmeProvaSP;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterGraficoComparativoProficienciaSmeUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterGraficoComparativoProficienciaSmeUseCase useCase;

        public ObterGraficoComparativoProficienciaSmeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterGraficoComparativoProficienciaSmeUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_TipoPerfil_Nulo()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(2024, 1, 5));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_TipoPerfil_Diferente_De_Administrador()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor);

            await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(2024, 1, 5));
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Quando_ProficienciasProvaSaberes_Vazia()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaPorSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciasPorSmeProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var resultado = await useCase.Executar(2024, 1, 5);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Quando_Dados_Validos()
        {
            var proficienciaSaberes = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    DreAbreviacao = "DRE1",
                    DreNome = "Dre Centro",
                    Periodo = "1º Bimestre",
                    NomeAplicacao = "Prova Saberes",
                    MediaProficiencia = 200,
                    LoteId = 10
                },
                new ResultadoProeficienciaPorDre
                {
                    DreAbreviacao = "DRE1",
                    DreNome = "Dre Centro",
                    Periodo = "2º Bimestre",
                    NomeAplicacao = "Prova Saberes",
                    MediaProficiencia = 250,
                    LoteId = 20
                }
            };

            var proficienciaSP = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    DreAbreviacao = "DRE1",
                    DreNome = "Dre Centro",
                    Periodo = "Anual",
                    NomeAplicacao = "Prova SP",
                    MediaProficiencia = 300
                }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaPorSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciaSaberes);

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciasPorSmeProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciaSP);

            var resultado = await useCase.Executar(2024, 1, 5);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.TodasAplicacoesDisponiveis);
            Assert.Contains("PSA", resultado.TodasAplicacoesDisponiveis.First());
            Assert.Contains("PSP", resultado.TodasAplicacoesDisponiveis.Last());
            Assert.NotEmpty(resultado.Dados);
            Assert.Equal("DRE1", resultado.Dados.First().DreAbreviacao);
            Assert.Equal("Dre Centro", resultado.Dados.First().DreNome);
            Assert.NotEmpty(resultado.Dados.First().ListaProficienciaGraficoComparativoDto);
        }

        [Fact]
        public async Task Executar_Deve_Mapear_Corretamente_Aplicacoes_Quando_ProficienciasSP_Vazia()
        {
            var proficienciaSaberes = new List<ResultadoProeficienciaPorDre>
            {
                new ResultadoProeficienciaPorDre
                {
                    DreAbreviacao = "DRE1",
                    DreNome = "Dre Norte",
                    Periodo = "1º Bimestre",
                    NomeAplicacao = "Prova Saberes",
                    MediaProficiencia = 200,
                    LoteId = 10
                }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaPorSmeProvaSaberesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proficienciaSaberes);

            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciasPorSmeProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ResultadoProeficienciaPorDre>());

            var resultado = await useCase.Executar(2024, 1, 5);

            Assert.NotNull(resultado);
            Assert.Single(resultado.TodasAplicacoesDisponiveis);
            Assert.Contains("PSA", resultado.TodasAplicacoesDisponiveis.First());
            Assert.NotEmpty(resultado.Dados);
        }
    }
}
