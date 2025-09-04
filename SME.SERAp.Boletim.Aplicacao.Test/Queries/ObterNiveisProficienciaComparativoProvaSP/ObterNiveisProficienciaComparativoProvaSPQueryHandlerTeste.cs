using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterNiveisProficienciaComparativoProvaSP
{
    public class ObterNiveisProficienciaComparativoProvaSPQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolarMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterNiveisProficienciaComparativoProvaSPQueryHandler handler;

        public ObterNiveisProficienciaComparativoProvaSPQueryHandlerTeste()
        {
            repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            mediatorMock = new Mock<IMediator>();
            handler = new ObterNiveisProficienciaComparativoProvaSPQueryHandler(repositorioBoletimEscolarMock.Object, mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarComparativoCompleto_QuandoTodosOsDadosExistem()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var query = new ObterNiveisProficienciaComparativoProvaSPQuery(loteId, ueId, disciplinaId, anoEscolar);

            var niveisProficiencia = new List<ObterNivelProficienciaDto>
            {
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = anoEscolar, Descricao = "Nível Básico", ValorReferencia = 500 },
                new ObterNivelProficienciaDto { DisciplinaId = disciplinaId, Ano = anoEscolar, Descricao = "Nível Intermediário", ValorReferencia = 600 },
            };

            var proficienciasAnoCorrente = new List<UeMediaProficienciaDto>
            {
                new UeMediaProficienciaDto { LoteId = 1, NomeAplicacao = "Teste", Periodo = "1 Semestre", MediaProficiencia = 550.0M }
            };

            var proficienciaAnoAnterior = new List<UeMediaProficienciaDto>
            {
                new UeMediaProficienciaDto { LoteId = 0, NomeAplicacao = "Prova SP", Periodo = "2023", MediaProficiencia = 520.0M }
            };

            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(niveisProficiencia);

            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasAnoCorrente);

            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAnoAnteriorAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciaAnoAnterior);

            repositorioBoletimEscolarMock.Setup(r => r.ObterTotalAlunosUeRealizaramProvasSPAnterior(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(100);

            repositorioBoletimEscolarMock.Setup(r => r.ObterTotalAlunosRealizaramProvasUe(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(90);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Nível Intermediário");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.TotalLotes);
            Assert.NotNull(resultado.ProvaSP);
            Assert.Equal("Nível Intermediário", resultado.ProvaSP.NivelProficiencia);
            Assert.Equal(100, resultado.ProvaSP.TotalRealizaramProva);
            Assert.Single(resultado.Lotes);
            Assert.Equal("Nível Intermediário", resultado.Lotes.First().NivelProficiencia);
            Assert.Equal(90, resultado.Lotes.First().TotalRealizaramProva);
        }

        [Fact]
        public async Task Handle_DeveRetornarProvaSPNaoAvaliada_QuandoNaoHaDadosDoAnoAnterior()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var query = new ObterNiveisProficienciaComparativoProvaSPQuery(loteId, ueId, disciplinaId, anoEscolar);

            var niveisProficiencia = new List<ObterNivelProficienciaDto>();
            var proficienciasAnoCorrente = new List<UeMediaProficienciaDto>
            {
                new UeMediaProficienciaDto { LoteId = 1, NomeAplicacao = "Teste", Periodo = "1 Semestre", MediaProficiencia = 550.0M }
            };
            var proficienciaAnoAnterior = new List<UeMediaProficienciaDto>();

            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(niveisProficiencia);
            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAnoAnteriorAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(proficienciaAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterTotalAlunosRealizaramProvasUe(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(90);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Nível Não Definido");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal("Não Avaliado", resultado.ProvaSP.NomeAplicacao);
            Assert.Equal("Não Avaliado", resultado.ProvaSP.NivelProficiencia);
            Assert.Equal(0, resultado.ProvaSP.MediaProficiencia);
            Assert.Single(resultado.Lotes);
        }

        [Fact]
        public async Task Handle_DeveRetornarListasVazias_QuandoNaoHaDados()
        {
            var loteId = 1L;
            var ueId = 100;
            var disciplinaId = 1;
            var anoEscolar = 2024;
            var query = new ObterNiveisProficienciaComparativoProvaSPQuery(loteId, ueId, disciplinaId, anoEscolar);

            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ObterNivelProficienciaDto>());
            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<UeMediaProficienciaDto>());
            repositorioBoletimEscolarMock.Setup(r => r.ObterMediaProficienciaUeAnoAnteriorAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<UeMediaProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.TotalLotes);
            Assert.Equal("Não Avaliado", resultado.ProvaSP.NomeAplicacao);
            Assert.Empty(resultado.Lotes);
        }
    }
}