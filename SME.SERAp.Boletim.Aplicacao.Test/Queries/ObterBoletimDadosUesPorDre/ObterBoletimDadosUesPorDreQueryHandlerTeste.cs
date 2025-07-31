using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimDadosUesPorDre;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.Queries
{
    public class ObterBoletimDadosUesPorDreQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolar;
        private readonly ObterBoletimDadosUesPorDreQueryHandler queryHandler;
        public ObterBoletimDadosUesPorDreQueryHandlerTeste()
        {
            repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            queryHandler = new ObterBoletimDadosUesPorDreQueryHandler(repositorioBoletimEscolar.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Das_Ues_Por_Dre()
        {
            var itens = ObterUeDadosBoletins();
            var disciplinasProficiencias = ObterUeBoletinsDisciplinasProficiencias();

            var loteId = 1L;
            var dreId = 2L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto() { TamanhoPagina = 10 };
            var uesIds = itens.Select(x => x.Id).Distinct().ToList();

            repositorioBoletimEscolar
                .Setup(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros))
                .ReturnsAsync(new PaginacaoUesBoletimDadosDto(itens, filtros.Pagina, filtros.TamanhoPagina, itens.Count()));

            repositorioBoletimEscolar
                .Setup(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, uesIds))
                .ReturnsAsync(disciplinasProficiencias);

            var resultado = await queryHandler.Handle(new ObterBoletimDadosUesPorDreQuery(loteId, dreId, anoEscolar, filtros), CancellationToken.None);


            Assert.NotNull(resultado);
            Assert.Equal(resultado.Pagina, filtros.Pagina);
            Assert.Equal(resultado.TamanhoPagina, filtros.TamanhoPagina);
            Assert.Equal(resultado.Itens.Count(), itens.Count());


            var uePadraoResultado = resultado.Itens.FirstOrDefault(x => x.Id == 1);
            var uePadraoDisciplinas = disciplinasProficiencias.Where(x => x.UeId == uePadraoResultado.Id).ToList();
            Assert.Equal(uePadraoResultado.Disciplinas.Count(), uePadraoDisciplinas.Count());

            repositorioBoletimEscolar.Verify(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, uesIds), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dados_Das_Ues_Por_Dre()
        {
            var disciplinasProficiencias = ObterUeBoletinsDisciplinasProficiencias();

            var loteId = 1L;
            var dreId = 2L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto() { TamanhoPagina = 10 };

            repositorioBoletimEscolar
                .Setup(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros))
                .ReturnsAsync(new PaginacaoUesBoletimDadosDto(new List<UeDadosBoletimDto>() , filtros.Pagina, filtros.TamanhoPagina, 0));

            repositorioBoletimEscolar
                .Setup(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, new List<long>()))
                .ReturnsAsync(disciplinasProficiencias);

            var resultado = await queryHandler.Handle(new ObterBoletimDadosUesPorDreQuery(loteId, dreId, anoEscolar, filtros), CancellationToken.None);


            Assert.NotNull(resultado);
            Assert.Equal(resultado.Pagina, filtros.Pagina);
            Assert.Equal(resultado.TamanhoPagina, filtros.TamanhoPagina);
            Assert.Empty(resultado.Itens);

            repositorioBoletimEscolar.Verify(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, new List<long>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Das_Ues_Por_Dre_Sem_Disciplinas()
        {
            var itens = ObterUeDadosBoletins();

            var loteId = 1L;
            var dreId = 2L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto() { TamanhoPagina = 10 };
            var uesIds = itens.Select(x => x.Id).Distinct().ToList();

            repositorioBoletimEscolar
                .Setup(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros))
                .ReturnsAsync(new PaginacaoUesBoletimDadosDto(itens, filtros.Pagina, filtros.TamanhoPagina, itens.Count()));

            repositorioBoletimEscolar
                .Setup(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, uesIds))
                .ReturnsAsync(new List<UeBoletimDisciplinaProficienciaDto>());

            var resultado = await queryHandler.Handle(new ObterBoletimDadosUesPorDreQuery(loteId, dreId, anoEscolar, filtros), CancellationToken.None);


            Assert.NotNull(resultado);
            Assert.Equal(resultado.Pagina, filtros.Pagina);
            Assert.Equal(resultado.TamanhoPagina, filtros.TamanhoPagina);
            Assert.Equal(resultado.Itens.Count(), itens.Count());
            Assert.True(resultado.Itens.All(x=> !x.Disciplinas?.Any() ?? true));

            repositorioBoletimEscolar.Verify(r => r.ObterUesPorDre(loteId, dreId, anoEscolar, filtros), Times.Once);
            repositorioBoletimEscolar.Verify(r => r.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, uesIds), Times.Once);
        }

        private static List<UeDadosBoletimDto> ObterUeDadosBoletins()
        {
            return new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto
                {
                    Id = 1,
                    UeNome = "Escola A",
                    TotalEstudantes = 100,
                    TotalEstudadesRealizaramProva = 80,
                    Disciplinas = new List<UeBoletimDisciplinaProficienciaDto>()
                },
                new UeDadosBoletimDto
                {
                    Id = 2,
                    UeNome = "Escola B",
                    TotalEstudantes = 150,
                    TotalEstudadesRealizaramProva = 120,
                    Disciplinas = new List<UeBoletimDisciplinaProficienciaDto>()
                }
            };
        }

        private static List<UeBoletimDisciplinaProficienciaDto> ObterUeBoletinsDisciplinasProficiencias()
        {
            return new List<UeBoletimDisciplinaProficienciaDto>
            {
                new UeBoletimDisciplinaProficienciaDto
                {
                    UeId = 1,
                    DisciplinaId = 1,
                    MediaProficiencia = 75.5m,
                    NivelCodigo = 1,
                    NivelDescricao = "Abaixo Básico"
                },
                new UeBoletimDisciplinaProficienciaDto
                {
                    UeId = 1,
                    DisciplinaId = 2,
                    MediaProficiencia = 80.0m,
                    NivelCodigo = 1,
                    NivelDescricao = "Abaixo Básico"
                }
            };
        }
    }
}
