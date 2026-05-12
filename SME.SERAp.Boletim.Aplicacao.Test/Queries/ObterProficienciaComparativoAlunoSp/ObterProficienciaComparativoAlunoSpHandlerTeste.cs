using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Test.Queries.ObterProficienciaComparativoAlunoSp
{
    public class ObterProficienciaComparativoAlunoSpHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimEscolar> repositorioBoletimEscolarMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProficienciaComparativoAlunoSpHandler handler;
        private const int ANO_LETIVO = 2024;

        public ObterProficienciaComparativoAlunoSpHandlerTeste()
        {
            repositorioBoletimEscolarMock = new Mock<IRepositorioBoletimEscolar>();
            mediatorMock = new Mock<IMediator>();
            handler = new ObterProficienciaComparativoAlunoSpHandler(repositorioBoletimEscolarMock.Object, mediatorMock.Object);
        }

        private void ConfigurarMocks(
            List<AlunoProficienciaDto> proficienciasAnoCorrente,
            List<AlunoProficienciaDto> proficienciasAnoAnterior,
            List<ObterNivelProficienciaDto> niveisProficiencia = null)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoLoteProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(ANO_LETIVO);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSaberesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(proficienciasAnoCorrente);
            repositorioBoletimEscolarMock.Setup(r => r.ObterProficienciaAlunoProvaSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<long>>())).ReturnsAsync(proficienciasAnoAnterior);
            repositorioBoletimEscolarMock.Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(niveisProficiencia ?? new List<ObterNivelProficienciaDto>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel Teste");
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Dados_Completos_Quando_Tudo_Existir()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
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
            ConfigurarMocks(proficienciasAnoCorrente, proficienciasAnoAnterior);

            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 450), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 1");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 500), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 2");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 550), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 3");
            mediatorMock.Setup(m => m.Send(It.Is<ObterNivelProficienciaDisciplinaQuery>(q => q.Media == 600), It.IsAny<CancellationToken>())).ReturnsAsync("Nivel 4");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Total);
            Assert.Equal(1, resultado.Pagina);
            Assert.Equal(10, resultado.ItensPorPagina);
            Assert.Equal(2, resultado.Aplicacoes.Count());
            Assert.Contains("1 Bim", resultado.Aplicacoes);
            Assert.Contains("2 Bim", resultado.Aplicacoes);
            Assert.Equal(2, resultado.Itens.Count());

            var alunoB = resultado.Itens.FirstOrDefault(x => x.Nome == "Aluno B");
            Assert.NotNull(alunoB);
            Assert.Equal(22.22, alunoB.Variacao, 2);
            Assert.Equal(3, alunoB.Proficiencias.Count());
            Assert.Equal("PSP", alunoB.Proficiencias.ElementAt(0).Descricao);
            Assert.Equal(450, alunoB.Proficiencias.ElementAt(0).Valor);
            Assert.Equal("1 Bim", alunoB.Proficiencias.ElementAt(1).Descricao);
            Assert.Equal(500, alunoB.Proficiencias.ElementAt(1).Valor);
            Assert.Equal("2 Bim", alunoB.Proficiencias.ElementAt(2).Descricao);
            Assert.Equal(550, alunoB.Proficiencias.ElementAt(2).Valor);

            var alunoC = resultado.Itens.FirstOrDefault(x => x.Nome == "Aluno C");
            Assert.NotNull(alunoC);
            Assert.Equal(0.0, alunoC.Variacao);
            Assert.Single(alunoC.Proficiencias);
            Assert.Equal(600, alunoC.Proficiencias.First().Valor);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Vazio_Quando_Nao_Houver_Proficiencia_No_Ano_Corrente()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, null, null);
            ConfigurarMocks(new List<AlunoProficienciaDto>(), new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Total);
            Assert.Equal(1, resultado.Pagina);
            Assert.Equal(0, resultado.ItensPorPagina);
            Assert.Empty(resultado.Aplicacoes);
            Assert.Empty(resultado.Itens);
        }

        [Theory]
        [InlineData(1, "Aluno Positivo", 1)]
        [InlineData(2, "Aluno Negativo", 1)]
        public async Task Handle_Deve_Filtrar_Corretamente(int? tipoVariacao, string nomeAlunoEsperado, int totalEsperado)
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, tipoVariacao.HasValue ? new List<int> { tipoVariacao.Value } : null, null, 1, 10);
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
            ConfigurarMocks(proficienciasAnoCorrente, proficienciasAnoAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(totalEsperado, resultado.Total);
            if (totalEsperado > 0)
            {
                Assert.Single(resultado.Itens);
                Assert.Contains(nomeAlunoEsperado, resultado.Itens.First().Nome);
            }
            else
            {
                Assert.Empty(resultado.Itens);
            }
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Todos_Alunos_Quando_TipoVariacao_For_Nulo()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
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
            ConfigurarMocks(proficienciasAnoCorrente, proficienciasAnoAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Total);
            Assert.Equal(3, resultado.Itens.Count());
        }

        [Fact]
        public async Task Handle_Deve_Filtrar_Aluno_Pelo_Nome()
        {
            var nomeAlunoFiltro = "Aluno Filtrado";
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, nomeAlunoFiltro, 1, 10);
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
            ConfigurarMocks(proficienciasAnoCorrente, proficienciasAnoAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Total);
            Assert.Single(resultado.Itens);
            Assert.Equal(nomeAlunoFiltro, resultado.Itens.First().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Paginacao_Corretamente()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 2, 2);
            var proficienciasAnoCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno C", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 4, NomeAluno = "Aluno D", Proficiencia = 500, Periodo = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 5, NomeAluno = "Aluno E", Proficiencia = 500, Periodo = "1 Bim" },
            };
            ConfigurarMocks(proficienciasAnoCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.Total);
            Assert.Equal(2, resultado.Pagina);
            Assert.Equal(2, resultado.ItensPorPagina);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Equal("Aluno C", resultado.Itens.First().Nome);
            Assert.Equal("Aluno D", resultado.Itens.Last().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Aluno_Sem_Proficiencia_Anterior()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 3, NomeAluno = "Aluno Sem PSP", Proficiencia = 700, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Itens);
            Assert.Single(resultado.Itens.First().Proficiencias);
            Assert.Equal("1 Bim", resultado.Itens.First().Proficiencias.First().Descricao);
        }

        [Fact]
        public async Task Handle_Deve_Agrupar_Proficiencias_Mesmo_Periodo()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 520, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Itens);
            Assert.Equal(3, resultado.Itens.First().Proficiencias.Count());
        }

        [Fact]
        public async Task Handle_Deve_Lidar_Com_NivelProficiencia_Nulo()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 }
            };
            ConfigurarMocks(profCorrente, profAnterior);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNivelProficienciaDisciplinaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((string)null);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.True(resultado.Itens.First().Proficiencias.All(p => p.NivelProficiencia == null));
        }

        [Fact]
        public async Task Handle_Deve_Ignorar_Alunos_Somente_Ano_Anterior()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "Turma A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno B", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 450 },
                new AlunoProficienciaDto { AlunoRa = 2, Proficiencia = 400 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Itens);
            Assert.Equal("Aluno B", resultado.Itens.First().Nome);
        }

        [Fact]
        public async Task Handle_Deve_Lidar_Com_Parametros_Nulos()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(0, 0, 0, null, 0, null, null, null, null);
            ConfigurarMocks(new List<AlunoProficienciaDto>(), new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Total);
            Assert.Empty(resultado.Itens);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Todos_Itens_Quando_Paginacao_For_Nula()
        {
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 600, Periodo = "1 Bim", NomeAplicacao = "1 Bim" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 400, NomeAplicacao = "PSP" }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var query = new ObterProficienciaComparativoAlunoSpQuery(
                ueId: 1, disciplinaId: 10, anoEscolar: 5, turma: "Turma A",
                loteId: 100, tiposVariacao: null, nomeAluno: null, pagina: null, itensPorPagina: null);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Total);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Contains(resultado.Itens, x => x.Nome == "Aluno A");
            Assert.Contains(resultado.Itens, x => x.Nome == "Aluno B");
        }

        [Fact]
        public async Task Handle_Deve_Preencher_Turma_No_Item_Resultado()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "5A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim", NomeAplicacao = "1 Bim", Turma = "5A" },
                new AlunoProficienciaDto { AlunoRa = 2, NomeAluno = "Aluno B", Proficiencia = 600, Periodo = "1 Bim", NomeAplicacao = "1 Bim", Turma = "5B" }
            };
            ConfigurarMocks(profCorrente, new List<AlunoProficienciaDto>());

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Itens.Count());
            Assert.Equal("5A", resultado.Itens.First(x => x.Nome == "Aluno A").Turma);
            Assert.Equal("5B", resultado.Itens.First(x => x.Nome == "Aluno B").Turma);
        }

        [Fact]
        public async Task Handle_Deve_Calcular_Variacao_Zero_Quando_Proficiencia_Nao_Muda()
        {
            var query = new ObterProficienciaComparativoAlunoSpQuery(1, 10, 5, "5A", 100, null, null, 1, 10);
            var profCorrente = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, NomeAluno = "Aluno A", Proficiencia = 500, Periodo = "1 Bim", Turma = "5A" }
            };
            var profAnterior = new List<AlunoProficienciaDto>
            {
                new AlunoProficienciaDto { AlunoRa = 1, Proficiencia = 500 }
            };
            ConfigurarMocks(profCorrente, profAnterior);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Itens);
            Assert.Equal(0.0, resultado.Itens.First().Variacao);
        }
    }
}
