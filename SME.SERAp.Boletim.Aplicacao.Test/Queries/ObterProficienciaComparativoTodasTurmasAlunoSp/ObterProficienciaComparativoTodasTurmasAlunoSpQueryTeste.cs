using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp
{
    public class ObterProficienciaComparativoTodasTurmasAlunoSpQueryTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoTodasTurmasAlunoSpHandler handler;
        private const int ANO_LETIVO = 2024;

        public ObterProficienciaComparativoTodasTurmasAlunoSpQueryTeste()
        {
            repositorioMock = new Mock<IRepositorioBoletimEscolar>();
            mediatorMock = new Mock<IMediator>();
            handler = new ObterProficienciaComparativoTodasTurmasAlunoSpHandler(repositorioMock.Object, mediatorMock.Object);
        }

        private void ConfigurarMocks(
            List<AlunoProficienciaDto> proficienciasAnoCorrente,
            List<AlunoProficienciaDto> proficienciasAnoAnterior,
            List<ObterNivelProficienciaDto> niveisProficiencia = null)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ANO_LETIVO);
            repositorioMock.Setup(r => r.ObterProficienciaAlunoTodasTurmasProvaSaberesAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(proficienciasAnoCorrente);
            repositorioMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(proficienciasAnoAnterior);
            repositorioMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(niveisProficiencia ?? new List<ObterNivelProficienciaDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Nível Teste");
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Vazio_Quando_Nao_Ha_Proficiencias_Ano_Corrente()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            ConfigurarMocks(new List<AlunoProficienciaDto>(), new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Total);
            Assert.Equal(1, resultado.Pagina);
            Assert.Equal(0, resultado.ItensPorPagina);
            Assert.Empty(resultado.Aplicacoes);
            Assert.Empty(resultado.Itens);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Dados_Completos_Com_Proficiencia_Anterior()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 600, Periodo = "1 Bim", NomeAplicacao = "1 Bim", Turma = "5A", DisciplinaNome = "Matemática", NomeLote = "Lote 2024" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim", Turma = "5B", DisciplinaNome = "Matemática", NomeLote = "Lote 2024" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500, NomeAplicacao = "PSP 2023" }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Total);
            Assert.Equal(1, resultado.Pagina);
            Assert.Equal(2, resultado.ItensPorPagina);
            Assert.Equal("Matemática", resultado.NomeDisciplina);
            Assert.Equal("Lote 2024", resultado.NomeLote);
            Assert.Equal("PSP 2023", resultado.NomeAplicacaoPSP);
            Assert.Single(resultado.Aplicacoes);
            Assert.Equal("1 Bim", resultado.Aplicacoes.First());

            var alunoA = resultado.Itens.First(x => x.Nome == "Aluno A");
            Assert.Equal("5A", alunoA.Turma);
            Assert.Equal(2, alunoA.Proficiencias.Count());
            Assert.Equal("PSP 2023", alunoA.Proficiencias.First().Descricao);
            Assert.Equal(20.0, alunoA.Variacao, 1);

            var alunoB = resultado.Itens.First(x => x.Nome == "Aluno B");
            Assert.Equal("5B", alunoB.Turma);
            Assert.Single(alunoB.Proficiencias);
            Assert.Equal(0.0, alunoB.Variacao);
        }

        [Fact]
        public async Task Handle_Deve_Propagar_Raca_E_Sexo_Do_Aluno()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Raca = "Branca", Sexo = "M", Proficiencia = 600, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Raca = null, Sexo = null, Proficiencia = 500, Periodo = "1 Bim", Turma = "5B" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            var alunoA = resultado.Itens.Single(x => x.Nome == "Aluno A");
            Assert.Equal("Branca", alunoA.Raca);
            Assert.Equal("M", alunoA.Sexo);

            var alunoB = resultado.Itens.Single(x => x.Nome == "Aluno B");
            Assert.Null(alunoB.Raca);
            Assert.Null(alunoB.Sexo);
        }

        [Fact]
        public async Task Handle_Deve_Ordenar_Por_Turma_E_Depois_Por_Nome()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Zico",   Proficiencia = 500, Periodo = "1 Bim", Turma = "5B" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Ana",    Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Carlos", Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            var itens = resultado.Itens.ToList();
            Assert.Equal("Ana", itens[0].Nome);
            Assert.Equal("5A", itens[0].Turma);
            Assert.Equal("Carlos", itens[1].Nome);
            Assert.Equal("5A", itens[1].Turma);
            Assert.Equal("Zico", itens[2].Nome);
            Assert.Equal("5B", itens[2].Turma);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Por_Variacao_Positiva()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, new List<int> { 1 }, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Positivo", Proficiencia = 600, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno Negativo", Proficiencia = 400, Periodo = "1 Bim", Turma = "5A" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 500 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, resultado.Total);
            Assert.Single(resultado.Itens);
            Assert.Equal("Aluno Positivo", resultado.Itens.Single().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Por_Variacao_Negativa()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, new List<int> { 2 }, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Positivo", Proficiencia = 600, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno Negativo", Proficiencia = 400, Periodo = "1 Bim", Turma = "5A" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 500 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, resultado.Total);
            Assert.Single(resultado.Itens);
            Assert.Equal("Aluno Negativo", resultado.Itens.Single().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Por_Nome_Aluno()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, "Filtrado");
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno Filtrado", Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Outro Aluno",    Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, resultado.Total);
            Assert.Single(resultado.Itens);
            Assert.Equal("Aluno Filtrado", resultado.Itens.Single().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_NomeDisciplina_E_NomeLote_Vazios_Quando_Sem_Proficiencias()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            ConfigurarMocks(new List<AlunoProficienciaDto>(), new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.True(string.IsNullOrEmpty(resultado.NomeDisciplina));
            Assert.True(string.IsNullOrEmpty(resultado.NomeLote));
            Assert.True(string.IsNullOrEmpty(resultado.NomeAplicacaoPSP));
        }

        [Fact]
        public async Task Handle_Deve_Preencher_Aplicacoes_Com_Periodos_Distintos_Ordenados()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "3 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 520, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 480, Periodo = "1 Bim", Turma = "5B" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, resultado.Aplicacoes.Count());
            Assert.Equal("1 Bim", resultado.Aplicacoes.First());
            Assert.Equal("3 Bim", resultado.Aplicacoes.Last());
        }

        [Fact]
        public async Task Handle_Deve_Usar_Turma_Do_Primeiro_Registro_Do_Aluno()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 520, Periodo = "2 Bim", Turma = "5A" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Single(resultado.Itens);
            Assert.Equal("5A", resultado.Itens.First().Turma);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Variacao_Corretamente()
        {
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(1, 10, 5, 100, null, null);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 600, Periodo = "1 Bim", Turma = "5A" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(20.0, resultado.Itens.Single().Variacao, 1);
        }

        [Fact]
        public async Task Handle_Deve_Chamar_Repositorio_Com_Parametros_Corretos()
        {
            var ueId = 5;
            var disciplinaId = 12;
            var anoEscolar = 9;
            var loteId = 999L;
            var query = new ObterProficienciaComparativoTodasTurmasAlunoSpQuery(ueId, disciplinaId, anoEscolar, loteId, null, null);
            ConfigurarMocks(new List<AlunoProficienciaDto>(), new List<AlunoProficienciaDto>());

            await handler.Handle(query, CancellationToken.None);

            repositorioMock.Verify(r => r.ObterProficienciaAlunoTodasTurmasProvaSaberesAsync(ueId, disciplinaId, anoEscolar, loteId), Times.Once);
        }
    }
}
