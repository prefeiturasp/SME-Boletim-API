using FluentAssertions;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaComparativoAlunoSp
{
    public class ObterProficienciaComparativoAlunoSpHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolarMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoAlunoSpHandler handler;

        public ObterProficienciaComparativoAlunoSpHandlerTeste()
        {
            repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            mediatorMock = new Mock<IMediator>();
            handler = new ObterProficienciaComparativoAlunoSpHandler(repositorioBoletimEscolarMock.Object, mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Dados_Completos_Quando_Tudo_Existir()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 550, Periodo = "2 Bim", NomeAplicacao = "2 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno C", Proficiencia = 600, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
            };
            var proficienciasAnoAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450, NomeAplicacao = "PSP" },
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 450), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 1");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 500), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 2");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 550), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 3");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 600), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 4");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(2);
            resultado.Pagina.Should().Be(1);
            resultado.ItensPorPagina.Should().Be(10);
            resultado.Aplicacoes.Should().HaveCount(2).And.Contain("1 Bim", "2 Bim");
            resultado.Itens.Should().HaveCount(2);

            var alunoB = resultado.Itens.FirstOrDefault(x => x.Nome == "Aluno B");
            alunoB.Should().NotBeNull();
            alunoB.Variacao.Should().Be(100.0);
            alunoB.Proficiencias.Should().HaveCount(3);
            alunoB.Proficiencias.ElementAt(0).Descricao.Should().Be("PSP");
            alunoB.Proficiencias.ElementAt(0).Valor.Should().Be(450);
            alunoB.Proficiencias.ElementAt(1).Descricao.Should().Be("1 Bim");
            alunoB.Proficiencias.ElementAt(1).Valor.Should().Be(500);
            alunoB.Proficiencias.ElementAt(2).Descricao.Should().Be("2 Bim");
            alunoB.Proficiencias.ElementAt(2).Valor.Should().Be(550);

            var alunoC = resultado.Itens.FirstOrDefault(x => x.Nome == "Aluno C");
            alunoC.Should().NotBeNull();
            alunoC.Variacao.Should().Be(0.0);
            alunoC.Proficiencias.Should().HaveCount(1);
            alunoC.Proficiencias.First().Valor.Should().Be(600);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Vazio_Quando_Nao_Houver_Proficiencia_No_Ano_Corrente()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, null, null);
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>();

            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(0);
            resultado.Pagina.Should().Be(1);
            resultado.ItensPorPagina.Should().Be(0);
            resultado.Aplicacoes.Should().BeEmpty();
            resultado.Itens.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Dados_Por_Variacao_Positiva()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, 1, null, null, null);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Positivo", Proficiencia = 600, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno Negativo", Proficiencia = 400, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno Sem Variacao", Proficiencia = 500, Periodo = "1 Bim" },
            };
            var proficienciasAnoAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 3, Proficiencia = 500 },
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Nome.Should().Be("Aluno Positivo");
            resultado.Itens.First().Variacao.Should().Be(100.0);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Dados_Por_Variacao_Negativa()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, 2, null, null, null);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Positivo", Proficiencia = 600, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno Negativo", Proficiencia = 400, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno Sem Variacao", Proficiencia = 500, Periodo = "1 Bim" },
            };
            var proficienciasAnoAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 3, Proficiencia = 500 },
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Nome.Should().Be("Aluno Negativo");
            resultado.Itens.First().Variacao.Should().Be(-100.0);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Dados_Sem_Variacao()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, 3, null, null, null);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Positivo", Proficiencia = 600, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno Negativo", Proficiencia = 400, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno Sem Variacao", Proficiencia = 500, Periodo = "1 Bim" },
            };
            var proficienciasAnoAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 3, Proficiencia = 500 },
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Nome.Should().Be("Aluno Sem Variacao");
            resultado.Itens.First().Variacao.Should().Be(0.0);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Aluno_Pelo_Nome()
        {
            var nomeAlunoFiltro = "Aluno Filtrado";
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, nomeAlunoFiltro, 1, 10);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Filtrado", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Outro Aluno", Proficiencia = 600, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
                    var proficienciasAnoAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450, NomeAplicacao = "PSP" },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 550, NomeAplicacao = "PSP" }
            };
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(1);
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Nome.Should().Be(nomeAlunoFiltro);
        }

        [Fact]
        public async Task Handle_Deve_Paginacao_Corretamente()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 2, 2);
            var anoLetivo = 2024;
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno C", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 4, NomeAluno = "Aluno D", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 5, NomeAluno = "Aluno E", Proficiencia = 500, Periodo = "1 Bim" },
            };
            var proficienciasAnoAnterior = new List<AlunoProficienciaDto>();
            var niveisProficiencia = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(5);
            resultado.Pagina.Should().Be(2);
            resultado.ItensPorPagina.Should().Be(2);
            resultado.Itens.Should().HaveCount(2);
            resultado.Itens.First().Nome.Should().Be("Aluno C");
            resultado.Itens.Last().Nome.Should().Be("Aluno D");
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Aluno_Sem_Proficiencia_Anterior()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var anoLetivo = 2024;
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno Sem PSP", Proficiencia = 700, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>();
            var niveis = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(profCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(profAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveis);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Proficiencias.Should().HaveCount(1);
            resultado.Itens.First().Proficiencias.First().Descricao.Should().Be("1 Bim");
        }

        [Fact]
        public async Task Handle_Deve_Agrupar_Proficiencias_Mesmo_Periodo()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var anoLetivo = 2024;
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 520, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 }
            };
            var niveis = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(profCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(profAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveis);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Proficiencias.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handle_Deve_Lidar_Com_NivelProficiencia_Nulo()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var anoLetivo = 2024;
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 }
            };
            var niveis = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(profCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(profAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveis);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((string)null);

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Itens.First().Proficiencias.All(p => p.NivelProficiencia == null).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Deve_Ignorar_Alunos_Somente_Ano_Anterior()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var anoLetivo = 2024;
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 400 }
            };
            var niveis = new List<ObterNivelProficienciaDto>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoLetivo);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(profCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(profAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveis);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel X");

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Itens.Should().HaveCount(1);
            resultado.Itens.First().Nome.Should().Be("Aluno B");
        }

        [Fact]
        public async Task Handle_Deve_Lidar_Com_Parametros_Nulos()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(0, 0, 0, null, 0, null, null, null, null);
            var profCorrente = new List<AlunoProficienciaDto>();
            var profAnterior = new List<AlunoProficienciaDto>();
            var niveis = new List<ObterNivelProficienciaDto>();

            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(profCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(profAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveis);

            var resultado = await handler.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Total.Should().Be(0);
            resultado.Itens.Should().BeEmpty();
        }
    }
}