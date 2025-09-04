using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProvasBoletimEscolarPorUe
{
    public class ObterProvasBoletimEscolarPorUeQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Provas_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimDto();
            var provasEsperadas = new List<ProvaBoletimEscolarDto>
            {
                new ProvaBoletimEscolarDto
                {
                    Id = 10,
                    Descricao = "Prova Matemática",
                    Niveis = new List<ProvaNivelProficienciaBoletimEscolarDto>
                    {
                        new ProvaNivelProficienciaBoletimEscolarDto { AnoEscolar = 1, Basico = "B", Adequado = "A", Avancado = "AV" }
                    },
                    Turmas = new List<ProvaTurmaBoletimEscolarDto>
                    {
                        new ProvaTurmaBoletimEscolarDto { Turma = "A", Basico = "B", Adequado = "A", Avancado = "AV", Total = 30 }
                    }
                }
            };

            var repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            repositorioBoletimEscolarMock
                .Setup(r => r.ObterProvasBoletimEscolarPorUe(loteId, ueId, filtros))
                .ReturnsAsync(provasEsperadas);

            var handler = new ObterProvasBoletimEscolarPorUeQueryHandler(repositorioBoletimEscolarMock.Object);
            var query = new ObterProvasBoletimEscolarPorUeQuery(loteId, ueId, filtros);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            var prova = ((List<ProvaBoletimEscolarDto>)resultado)[0];
            Assert.Equal(10, prova.Id);
            Assert.Equal("Prova Matemática", prova.Descricao);
            Assert.Single(prova.Niveis);
            Assert.Single(prova.Turmas);
            Assert.Equal("A", prova.Turmas is not null ? ((List<ProvaTurmaBoletimEscolarDto>)prova.Turmas)[0].Turma : null);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_Provas()
        {
            var loteId = 1L;
            var ueId = 999L;
            var filtros = new FiltroBoletimDto();

            var repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            repositorioBoletimEscolarMock
                .Setup(r => r.ObterProvasBoletimEscolarPorUe(loteId, ueId, filtros))
                .ReturnsAsync(new List<ProvaBoletimEscolarDto>());

            var handler = new ObterProvasBoletimEscolarPorUeQueryHandler(repositorioBoletimEscolarMock.Object);
            var query = new ObterProvasBoletimEscolarPorUeQuery(loteId, ueId, filtros);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}