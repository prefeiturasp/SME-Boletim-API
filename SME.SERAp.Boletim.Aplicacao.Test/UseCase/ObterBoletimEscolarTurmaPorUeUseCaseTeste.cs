using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Reflection;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimEscolarTurmaPorUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_Deve_Retornar_BoletimEscolarPorTurmaDto()
        {

            var loteId = 1L;
            var ueId = 100L;
            var provaId = 999L;

            var filtros = new FiltroBoletimDto { Ano = new List<int> { 5 } };

            var abrangencias = new List<AbrangenciaUeDto>
            {
                new() { UeId = ueId }
            };

            var provas = new List<ProvaBoletimEscolarDto>
            {
                new() { Id = provaId, Descricao = "Prova ABC" }
            };

            var turmas = new List<TurmaBoletimEscolarDto>
            {
                new()
                {
                    Turma = "Turma 1",
                    Total = 30,
                    AbaixoBasico = 5,
                    AbaixoBasicoPorcentagem = 16.67m,
                    Basico = 10,
                    BasicoPorcentagem = 33.33m,
                    Adequado = 10,
                    AdequadoPorcentagem = 33.33m,
                    Avancado = 5,
                    AvancadoPorcentagem = 16.67m,
                    MediaProficiencia = 250
                }
            };

            var niveis = new List<NivelProficienciaBoletimEscolarDto>
            {
                new() { Ano = 5, Codigo = (int)TipoNivelProficiencia.AbaixoBasico, Valor = 200 },
                new() { Ano = 5, Codigo = (int)TipoNivelProficiencia.Basico, Valor = 225 },
                new() { Ano = 5, Codigo = (int)TipoNivelProficiencia.Adequado, Valor = 250 },
                new() { Ano = 5, Codigo = (int)TipoNivelProficiencia.Avancado, Valor = 275 }
            };

            var mediator = new Mock<IMediator>();

            mediator.Setup(x => x.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            mediator.Setup(x => x.Send(It.IsAny<ObterProvasBoletimEscolarPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provas);

            mediator.Setup(x => x.Send(It.IsAny<ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveis);

            var useCase = new ObterBoletimEscolarTurmaPorUeUseCase(mediator.Object);


            var resultado = await useCase.Executar(loteId, ueId, filtros);

            var prova = resultado.Provas.Single();

            Assert.Equal("Prova ABC", prova.Descricao);
            Assert.Single(prova.Turmas);
            Assert.Single(prova.Niveis);

            var nivel = prova.Niveis.First();
            Assert.Equal(5, nivel.AnoEscolar);
            Assert.Equal("<200", nivel.AbaixoBasico);
            Assert.Equal(">=200 e <225", nivel.Basico);
            Assert.Equal(">=225 e <250", nivel.Adequado);
            Assert.Equal(">=250", nivel.Avancado);

            var turma = prova.Turmas.First();
            Assert.Equal("Turma 1", turma.Turma);
            Assert.Equal("5 (16.67%)", turma.AbaixoBasico.Replace(',', '.'));
            Assert.Equal("10 (33.33%)", turma.Basico.Replace(',', '.'));
            Assert.Equal("10 (33.33%)", turma.Adequado.Replace(',', '.'));
            Assert.Equal("5 (16.67%)", turma.Avancado.Replace(',', '.'));
            Assert.Equal(250, turma.MediaProficiencia);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NaoAutorizadoException_Se_Nao_Possuir_Abrangencia()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>());

            var useCase = new ObterBoletimEscolarTurmaPorUeUseCase(mediator.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(1, 100, new()));
            Assert.Equal("Usuário não possui abrangências para essa UE.", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Dto_Vazio_Quando_Nao_Houver_Provas()
        {
            var ueId = 100L;
            var mediator = new Mock<IMediator>();

            mediator.Setup(x => x.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto> { new() { UeId = ueId } });

            mediator.Setup(x => x.Send(It.IsAny<ObterProvasBoletimEscolarPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProvaBoletimEscolarDto>());

            var useCase = new ObterBoletimEscolarTurmaPorUeUseCase(mediator.Object);

            var resultado = await useCase.Executar(1, ueId, new());

            Assert.Empty(resultado.Provas); // não entrou no foreach
        }

        [Fact]
        public void ObterDescricaoNivelProficiencia_Deve_Retornar_Vazio_Quando_TipoNaoMapeado()
        {
            var grupoFake = new List<NivelProficienciaBoletimEscolarDto>
            {
                new() { Ano = 5, Codigo = 999, Valor = 300 } // tipo inexistente no dicionário
            }.GroupBy(x => x.Ano).First();

            var descricao = InvokeDescricaoNivelProficiencia((TipoNivelProficiencia)999, grupoFake);

            Assert.Equal(string.Empty, descricao);
        }

        private static string InvokeDescricaoNivelProficiencia(TipoNivelProficiencia tipo, IGrouping<int, NivelProficienciaBoletimEscolarDto> grupo)
        {
            var method = typeof(ObterBoletimEscolarTurmaPorUeUseCase)
                .GetMethod("ObterDescricaoNivelProficiencia", BindingFlags.NonPublic | BindingFlags.Static);

            return (string)method.Invoke(null, new object[] { tipo, grupo });
        }
    }
}
