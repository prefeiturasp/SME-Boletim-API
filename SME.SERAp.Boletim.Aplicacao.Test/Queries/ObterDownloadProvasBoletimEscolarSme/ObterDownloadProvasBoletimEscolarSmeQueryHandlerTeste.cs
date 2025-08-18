using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterDownloadProvasBoletimEscolarSmeQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorio;
        private readonly ObterDownloadProvasBoletimEscolarSmeQueryHandler queryHandler;
        public ObterDownloadProvasBoletimEscolarSmeQueryHandlerTeste()
        {
            repositorio = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterDownloadProvasBoletimEscolarSmeQueryHandler(repositorio.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Provas_Boletim_Escolar_Sme()
        {
            var loteId = 1L;
            var itens = new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto
                {
                    NomeDreAbreviacao = "DRE Teste",
                    CodigoUE = "123456",
                    NomeUE = "Escola Teste",
                    AnoEscola = 2023,
                    Turma = "Turma A",
                    AlunoRA = 123,
                    NomeAluno = "Aluno Teste",
                    Componente = "Matemática",
                    Proficiencia = 75.5M,
                    Nivel = "Abaixo básico"
                }
            };

            repositorio.Setup(r => r.ObterDownloadProvasBoletimEscolarSme(loteId))
                .ReturnsAsync(itens);

            var result = await queryHandler.Handle(new ObterDownloadProvasBoletimEscolarSmeQuery(loteId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(itens.Count, result.Count());
            repositorio.Verify(r => r.ObterDownloadProvasBoletimEscolarSme(loteId), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Provas_Boletim_Escolar_Sme()
        {
            var loteId = 1L;

            repositorio.Setup(r => r.ObterDownloadProvasBoletimEscolarSme(loteId))
                .ReturnsAsync(new List<DownloadProvasBoletimEscolarPorDreDto>());

            var result = await queryHandler.Handle(new ObterDownloadProvasBoletimEscolarSmeQuery(loteId), CancellationToken.None);

            Assert.Empty(result);
            repositorio.Verify(r => r.ObterDownloadProvasBoletimEscolarSme(loteId), Times.Once);
        }
    }
}
