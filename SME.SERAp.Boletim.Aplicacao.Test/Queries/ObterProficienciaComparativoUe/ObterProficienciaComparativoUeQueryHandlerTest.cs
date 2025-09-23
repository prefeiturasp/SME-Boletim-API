using FluentAssertions;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoUe;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaComparativoUe
{
    public class ObterProficienciaComparativoUeQueryHandlerTest
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolarMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoUeQueryHandler handler;

        private const int ANO_LETIVO = 2024;
        private const int DRE_ID = 1;
        private const int DISCIPLINA_ID = 10;
        private const int ANO_ESCOLAR = 5;

        public ObterProficienciaComparativoUeQueryHandlerTest()
        {
            repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            mediatorMock = new Mock<IMediator>();
            handler = new ObterProficienciaComparativoUeQueryHandler(repositorioBoletimEscolarMock.Object, mediatorMock.Object);
        }

        private void SetupMocks(
            List<UeProficienciaQueryResultDto> proficienciasPsa = null,
            List<UeProficienciaQueryResultDto> proficienciasPsp = null,
            List<ObterNivelProficienciaDto> niveisProficiencia = null)
        {
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaUeProvaSaberesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>()))
                .ReturnsAsync(proficienciasPsa ?? new List<UeProficienciaQueryResultDto>());

            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaUeProvaSPAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>()))
                .ReturnsAsync(proficienciasPsp ?? new List<UeProficienciaQueryResultDto>());

            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(
                It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(niveisProficiencia ?? new List<ObterNivelProficienciaDto>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Nível Teste");

            BoletimExtensions.ObterUeDescricao(null, TipoEscola.Nenhum, null, null);
            typeof(BoletimExtensions).GetMethod("ObterUeDescricao", BindingFlags.Static | BindingFlags.Public).Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_DeveRetornarVazio_QuandoNaoHouverProficiencias()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, null, 1, 10);
            SetupMocks();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(0);
            resultado.Pagina.Should().Be(1);
            resultado.ItensPorPagina.Should().Be(10);
            resultado.Ues.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_DeveRetornarDadosCompletos_QuandoTudoExistir()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, null, null, null);
            var proficienciasPsa = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE A", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 600, NomeAplicacao = "1° Bimestre", Periodo = "2024.1", RealizaramProva = 50 },
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE A", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 650, NomeAplicacao = "2° Bimestre", Periodo = "2024.2", RealizaramProva = 55 },
                new UeProficienciaQueryResultDto { UeId = 2, UeNome = "UE B", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 700, NomeAplicacao = "1° Bimestre", Periodo = "2024.1", RealizaramProva = 60 },
            };
            var proficienciasPsp = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE A", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 500, NomeAplicacao = "PSP", RealizaramProva = 45 },
                new UeProficienciaQueryResultDto { UeId = 2, UeNome = "UE B", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 750, NomeAplicacao = "PSP", RealizaramProva = 65 },
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();
            SetupMocks(proficienciasPsa, proficienciasPsp, niveisProficiencia);

            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(2);
            resultado.DreId.Should().Be(DRE_ID);
            resultado.DreAbreviacao.Should().Be("DRE A");
            resultado.Ues.Should().HaveCount(2);

            var ueA = resultado.Ues.FirstOrDefault(x => x.UeId == 1);
            ueA.Should().NotBeNull();
            ueA.Variacao.Should().BeApproximately(30.00, 0.01);
            ueA.AplicacaoPsp.Should().NotBeNull();
            ueA.AplicacaoPsp.MediaProficiencia.Should().Be(500);
            ueA.AplicacoesPsa.Should().HaveCount(2);
            ueA.AplicacoesPsa.First().MediaProficiencia.Should().Be(600);
            ueA.AplicacoesPsa.Last().MediaProficiencia.Should().Be(650);

            var ueB = resultado.Ues.FirstOrDefault(x => x.UeId == 2);
            ueB.Should().NotBeNull();
            ueB.Variacao.Should().BeApproximately(-6.67, 0.01);
            ueB.AplicacaoPsp.Should().NotBeNull();
            ueB.AplicacaoPsp.MediaProficiencia.Should().Be(750);
            ueB.AplicacoesPsa.Should().HaveCount(1);
            ueB.AplicacoesPsa.First().MediaProficiencia.Should().Be(700);
        }

        [Fact]
        public async Task Handle_DeveRetornarUeComApenasProficienciasPsa()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, null, null, null);
            var proficienciasPsa = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 3, UeNome = "UE C", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 500, NomeAplicacao = "1° Bimestre", Periodo = "2024.1", RealizaramProva = 40 }
            };
            SetupMocks(proficienciasPsa, new List<UeProficienciaQueryResultDto>());
            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            var ueC = resultado.Ues.First();
            ueC.UeId.Should().Be(3);
            ueC.AplicacaoPsp.Should().BeNull();
            ueC.AplicacoesPsa.Should().HaveCount(1);
            ueC.AplicacoesPsa.First().MediaProficiencia.Should().Be(500);
            ueC.Variacao.Should().Be(0.0);
        }

        [Fact]
        public async Task Handle_DeveRetornarUeComApenasProficienciasPsp()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, null, null, null);
            var proficienciasPsp = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 4, UeNome = "UE D", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, DisciplinaId = 10, MediaProficiencia = 600, NomeAplicacao = "PSP", RealizaramProva = 45 }
            };
            SetupMocks(new List<UeProficienciaQueryResultDto>(), proficienciasPsp);
            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            var ueD = resultado.Ues.First();
            ueD.UeId.Should().Be(4);
            ueD.AplicacaoPsp.Should().NotBeNull();
            ueD.AplicacaoPsp.MediaProficiencia.Should().Be(600);
            ueD.AplicacoesPsa.Should().BeEmpty();
            ueD.Variacao.Should().Be(0.0);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(null, 3)]
        public async Task Handle_DeveFiltrarPorTipoVariacaoCorretamente(int? tipoVariacao, int totalEsperado)
        {
            // Arrange
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, tipoVariacao.HasValue ? new List<int> { tipoVariacao.Value } : null, null, null, null);
            var proficienciasPsa = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE Positiva", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, MediaProficiencia = 600, Periodo = "2024.1" },
                new UeProficienciaQueryResultDto { UeId = 2, UeNome = "UE Negativa", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, MediaProficiencia = 400, Periodo = "2024.1" },
                new UeProficienciaQueryResultDto { UeId = 3, UeNome = "UE Nula", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF, MediaProficiencia = 500, Periodo = "2024.1" }
            };
            var proficienciasPsp = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, MediaProficiencia = 500 },
                new UeProficienciaQueryResultDto { UeId = 2, MediaProficiencia = 500 },
                new UeProficienciaQueryResultDto { UeId = 3, MediaProficiencia = 500 }
            };
            SetupMocks(proficienciasPsa, proficienciasPsp);
            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(totalEsperado);
            if (totalEsperado > 0)
            {
                var ueEsperada = resultado.Ues.First();
                if (tipoVariacao == 1) ueEsperada.Variacao.Should().BeGreaterThan(0);
                if (tipoVariacao == 2) ueEsperada.Variacao.Should().BeLessThan(0);
                if (tipoVariacao == 3) ueEsperada.Variacao.Should().Be(0);
            }
        }

        [Fact]
        public async Task Handle_DeveFiltrarPorNomeUeCorretamente()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, "UE B", null, null);
            var proficienciasPsa = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE A", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 2, UeNome = "UE B", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 3, UeNome = "UE C", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF }
            };
            SetupMocks(proficienciasPsa);
            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            resultado.Ues.Should().HaveCount(1);
            resultado.Ues.First().UeNome.Should().Contain("UE B");
        }

        [Fact]
        public async Task Handle_DeveAplicarPaginacaoCorretamente()
        {
            var query = new ObterProficienciaComparativoUeQuery(DRE_ID, DISCIPLINA_ID, ANO_LETIVO, ANO_ESCOLAR, null, null, null, 2, 2);
            var proficienciasPsa = new List<UeProficienciaQueryResultDto>
            {
                new UeProficienciaQueryResultDto { UeId = 1, UeNome = "UE A", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 2, UeNome = "UE B", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 3, UeNome = "UE C", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 4, UeNome = "UE D", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF },
                new UeProficienciaQueryResultDto { UeId = 5, UeNome = "UE E", DreAbreviacao = "DRE A", TipoEscola = TipoEscola.EMEF }
            };
            SetupMocks(proficienciasPsa);
            MockStaticExtensionMethod();

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(5);
            resultado.Pagina.Should().Be(2);
            resultado.ItensPorPagina.Should().Be(2);
            resultado.Ues.Should().HaveCount(2);
            resultado.Ues.First().UeNome.Should().Contain("UE C");
            resultado.Ues.Last().UeNome.Should().Contain("UE D");
        }

        private void MockStaticExtensionMethod()
        {
            var methodInfo = typeof(BoletimExtensions).GetMethod("ObterUeDescricao", BindingFlags.Static | BindingFlags.Public);
            methodInfo.Should().NotBeNull();
        }
    }
}