using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;


namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId
{
    public class ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Niveis_Quando_Existir()
        {
            var loteId = 1L;
            var ueId = 123L;
            var provaId = 10L;
            var filtros = new FiltroBoletimDto();
            var niveisEsperados = new List<NivelProficienciaBoletimEscolarDto>
            {
                new NivelProficienciaBoletimEscolarDto
                {
                    Ano = 2024,
                    Codigo = 1,
                    Descricao = "Adequado",
                    Valor = 100
                },
                new NivelProficienciaBoletimEscolarDto
                {
                    Ano = 2024,
                    Codigo = 2,
                    Descricao = "Avançado",
                    Valor = 50
                }
            };

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(loteId, ueId, provaId, filtros))
                .ReturnsAsync(niveisEsperados);

            var handler = new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQueryHandler(repositorioBoletimProvaAlunoMock.Object);
            var query = new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(loteId, ueId, provaId, filtros);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<NivelProficienciaBoletimEscolarDto>)resultado).Count);
            Assert.Equal("Adequado", ((List<NivelProficienciaBoletimEscolarDto>)resultado)[0].Descricao);
            Assert.Equal("Avançado", ((List<NivelProficienciaBoletimEscolarDto>)resultado)[1].Descricao);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Nao_Existir_Niveis()
        {
            var loteId = 1L;
            var ueId = 999L;
            var provaId = 20L;
            var filtros = new FiltroBoletimDto();

            var repositorioBoletimProvaAlunoMock = new Mock<IRepositorioBoletimProvaAluno>();
            repositorioBoletimProvaAlunoMock
                .Setup(r => r.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(loteId, ueId, provaId, filtros))
                .ReturnsAsync(new List<NivelProficienciaBoletimEscolarDto>());

            var handler = new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQueryHandler(repositorioBoletimProvaAlunoMock.Object);
            var query = new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(loteId, ueId, provaId, filtros);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}