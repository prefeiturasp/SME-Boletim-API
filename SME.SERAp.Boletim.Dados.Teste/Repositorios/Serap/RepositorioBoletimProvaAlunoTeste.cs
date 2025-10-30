using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios.Serap
{
    internal class RepositorioBoletimProvaAlunoFake : RepositorioBoletimProvaAluno
    {
        private readonly IDbConnection _conexaoLeitura;

        public RepositorioBoletimProvaAlunoFake(ConnectionStringOptions options, IDbConnection conexaoLeitura) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
    }

    public class RepositorioBoletimProvaAlunoTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly RepositorioBoletimProvaAlunoFake repositorio;

        public RepositorioBoletimProvaAlunoTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            repositorio = new RepositorioBoletimProvaAlunoFake(new ConnectionStringOptions(), conexaoLeitura.Object);
        }

        [Fact]
        public async Task ObterBoletinsEscolaresTurmasPorUeIdProvaId_DeveRetornarBoletinsComFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var provaId = 200L;
            var filtros = new FiltroBoletimDto
            {
                ComponentesCurriculares = new List<int> { 1, 2 },
                Ano = new List<int> { 3, 4 }
            };

            var boletinsMock = new List<TurmaBoletimEscolarDto>
            {
                new TurmaBoletimEscolarDto { Turma = "Turma A", AbaixoBasico = 5, Total = 20, MediaProficiencia = 500 },
                new TurmaBoletimEscolarDto { Turma = "Turma B", AbaixoBasico = 3, Total = 15, MediaProficiencia = 550 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<TurmaBoletimEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(boletinsMock);

            var resultado = await repositorio.ObterBoletinsEscolaresTurmasPorUeIdProvaId(loteId, ueId, provaId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, b => b.Turma == "Turma A" && b.AbaixoBasico == 5);
            Assert.Contains(resultado, b => b.Turma == "Turma B" && b.AbaixoBasico == 3);
        }

        [Fact]
        public async Task ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId_DeveRetornarNiveisComFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var provaId = 200L;
            var filtros = new FiltroBoletimDto
            {
                ComponentesCurriculares = new List<int> { 1, 2 },
                Ano = new List<int> { 3, 4 }
            };

            var niveisMock = new List<NivelProficienciaBoletimEscolarDto>
            {
                new NivelProficienciaBoletimEscolarDto { Ano = 3, Codigo = 1, Descricao = "Nível Básico", Valor = 100 },
                new NivelProficienciaBoletimEscolarDto { Ano = 4, Codigo = 2, Descricao = "Nível Adequado", Valor = 200 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<NivelProficienciaBoletimEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(niveisMock);

            var resultado = await repositorio.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(loteId, ueId, provaId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, n => n.Codigo == 1 && n.Descricao == "Nível Básico");
            Assert.Contains(resultado, n => n.Codigo == 2 && n.Descricao == "Nível Adequado");
        }

        [Fact]
        public async Task ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId_DeveRetornarOpcoesCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var opcoesMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 1, Texto = "Nível 1" },
                new OpcaoFiltroDto<int> { Valor = 2, Texto = "Nível 2" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(opcoesMock);

            var resultado = await repositorio.ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, o => o.Valor == 1 && o.Texto == "Nível 1");
            Assert.Contains(resultado, o => o.Valor == 2 && o.Texto == "Nível 2");
        }

        [Fact]
        public async Task ObterOpcoesAnoEscolarBoletimEscolarPorUeId_DeveRetornarOpcoesCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var opcoesMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "5" },
                new OpcaoFiltroDto<int> { Valor = 6, Texto = "6" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(opcoesMock);

            var resultado = await repositorio.ObterOpcoesAnoEscolarBoletimEscolarPorUeId(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, o => o.Valor == 5 && o.Texto == "5");
            Assert.Contains(resultado, o => o.Valor == 6 && o.Texto == "6");
        }

        [Fact]
        public async Task ObterOpcoesComponenteCurricularBoletimEscolarPorUeId_DeveRetornarOpcoesCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var opcoesMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 1, Texto = "Matemática" },
                new OpcaoFiltroDto<int> { Valor = 2, Texto = "Linguagens" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(opcoesMock);

            var resultado = await repositorio.ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, o => o.Valor == 1 && o.Texto == "Matemática");
            Assert.Contains(resultado, o => o.Valor == 2 && o.Texto == "Linguagens");
        }

        [Fact]
        public async Task ObterOpcoesTurmaBoletimEscolarPorUeId_DeveRetornarOpcoesCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var opcoesMock = new List<OpcaoFiltroDto<string>>
            {
                new OpcaoFiltroDto<string> { Valor = "A", Texto = "A" },
                new OpcaoFiltroDto<string> { Valor = "B", Texto = "B" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<string>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(opcoesMock);

            var resultado = await repositorio.ObterOpcoesTurmaBoletimEscolarPorUeId(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, o => o.Valor == "A" && o.Texto == "A");
            Assert.Contains(resultado, o => o.Valor == "B" && o.Texto == "B");
        }

        [Fact]
        public async Task ObterValoresNivelProficienciaBoletimEscolarPorUeId_DeveRetornarValoresCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var valoresMock = new BoletimEscolarValoresNivelProficienciaDto
            {
                ValorMinimo = 450,
                ValorMaximo = 750
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryFirstAsync<BoletimEscolarValoresNivelProficienciaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(valoresMock);

            var resultado = await repositorio.ObterValoresNivelProficienciaBoletimEscolarPorUeId(loteId, ueId);

            Assert.NotNull(resultado);
            Assert.Equal(450, resultado.ValorMinimo);
            Assert.Equal(750, resultado.ValorMaximo);
        }

        [Fact]
        public async Task ObterNiveisProficienciaUes_DeveRetornarListaDeUeNiveisCorretamente()
        {
            var dreId = 1L;
            var anoEscolar = 5;
            var loteId = 1L;
            var niveisMock = new List<UeNivelProficienciaDto>
            {
                new UeNivelProficienciaDto { Codigo = "U1", Nome = "UE 1", Disciplina = "Matematica", DisciplinaId = 1, AnoEscolar = 5, MediaProficiencia = 650, NivelCodigo = 1, NivelDescricao = "Basico" },
                new UeNivelProficienciaDto { Codigo = "U2", Nome = "UE 2", Disciplina = "Matematica", DisciplinaId = 1, AnoEscolar = 5, MediaProficiencia = 750, NivelCodigo = 2, NivelDescricao = "Adequado" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<UeNivelProficienciaDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(niveisMock);

            var resultado = await repositorio.ObterNiveisProficienciaUes(dreId, anoEscolar, loteId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("U1", resultado.First().Codigo);
            Assert.Equal("U2", resultado.Last().Codigo);
        }

        [Fact]
        public async Task ObterNiveisProficienciaDisciplinas_DeveRetornarListaDeNiveisProficienciaCorretamente()
        {
            var anoEscolar = 5;
            var loteId = 1L;
            var niveisMock = new List<NivelProficienciaDto>
            {
                new NivelProficienciaDto { DreId = 1, Disciplina = "Matematica", DisciplinaId = 1, AnoEscolar = 5, MediaProficiencia = 650, NivelCodigo = 1, NivelDescricao = "Basico" },
                new NivelProficienciaDto { DreId = 2, Disciplina = "Portugues", DisciplinaId = 2, AnoEscolar = 5, MediaProficiencia = 750, NivelCodigo = 2, NivelDescricao = "Adequado" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<NivelProficienciaDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(niveisMock);

            var resultado = await repositorio.ObterNiveisProficienciaDisciplinas(anoEscolar, loteId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Matematica", resultado.First().Disciplina);
            Assert.Equal("Portugues", resultado.Last().Disciplina);
        }

        [Fact]
        public async Task ObterAbaEstudanteGraficoPorUeId_DeveRetornarResultadosCorretos_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudanteDto();

            var dadosMock = new List<AbaEstudanteGraficoTempDto>
            {
                new AbaEstudanteGraficoTempDto { Turma = "5A", Disciplina = "Matemática", Nome = "João", Proficiencia = 550 },
                new AbaEstudanteGraficoTempDto { Turma = "5A", Disciplina = "Matemática", Nome = "Maria", Proficiencia = 600 },
                new AbaEstudanteGraficoTempDto { Turma = "5B", Disciplina = "Português", Nome = "Pedro", Proficiencia = 650 },
                new AbaEstudanteGraficoTempDto { Turma = "5B", Disciplina = "Português", Nome = "Ana", Proficiencia = 700 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbaEstudanteGraficoTempDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var resultado = await repositorio.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            var turmaA = resultado.FirstOrDefault(r => r.Turma == "5A");
            Assert.NotNull(turmaA);
            Assert.Equal("Matemática", turmaA.Disciplina);
            Assert.Equal(2, turmaA.Alunos.Count());
            Assert.Contains(turmaA.Alunos, a => a.Nome == "João" && a.Proficiencia == 550);
            Assert.Contains(turmaA.Alunos, a => a.Nome == "Maria" && a.Proficiencia == 600);

            var turmaB = resultado.FirstOrDefault(r => r.Turma == "5B");
            Assert.NotNull(turmaB);
            Assert.Equal("Português", turmaB.Disciplina);
            Assert.Equal(2, turmaB.Alunos.Count());
            Assert.Contains(turmaB.Alunos, a => a.Nome == "Pedro" && a.Proficiencia == 650);
            Assert.Contains(turmaB.Alunos, a => a.Nome == "Ana" && a.Proficiencia == 700);
        }

        [Fact]
        public async Task ObterAbaEstudanteGraficoPorUeId_DeveFiltrarResultadosCorretamente_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudanteDto
            {
                ComponentesCurriculares = new List<int> { 1 },
                Ano = new List<int> { 5 },
                NivelMinimo = 500,
                NivelMaximo = 600,
                NomeEstudante = "Maria",
                NivelProficiencia = new List<int> { 1, 2 },
                Turma = new List<string> { "A" }
            };

            var dadosMock = new List<AbaEstudanteGraficoTempDto>
            {
                new AbaEstudanteGraficoTempDto { Turma = "5A", Disciplina = "Matemática", Nome = "Maria", Proficiencia = 600 },
                new AbaEstudanteGraficoTempDto { Turma = "5A", Disciplina = "Matemática", Nome = "Joao", Proficiencia = 550 },
                new AbaEstudanteGraficoTempDto { Turma = "5B", Disciplina = "Português", Nome = "Pedro", Proficiencia = 650 },
                new AbaEstudanteGraficoTempDto { Turma = "5B", Disciplina = "Português", Nome = "Ana", Proficiencia = 700 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbaEstudanteGraficoTempDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var resultado = await repositorio.ObterAbaEstudanteGraficoPorUeId(loteId, ueId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            var turmaMatematica = resultado.FirstOrDefault(r => r.Disciplina == "Matemática");
            Assert.NotNull(turmaMatematica);
            Assert.Equal(2, turmaMatematica.Alunos.Count());
            Assert.Contains(turmaMatematica.Alunos, a => a.Nome == "Maria");
            Assert.Contains(turmaMatematica.Alunos, a => a.Nome == "Joao");

            var turmaPortugues = resultado.FirstOrDefault(r => r.Disciplina == "Português");
            Assert.NotNull(turmaPortugues);
            Assert.Equal(2, turmaPortugues.Alunos.Count());
            Assert.Contains(turmaPortugues.Alunos, a => a.Nome == "Pedro");
            Assert.Contains(turmaPortugues.Alunos, a => a.Nome == "Ana");
        }

        [Fact]
        public async Task ObterAbaEstudanteBoletimEscolarPorUeId_DeveFiltrarResultadosCorretamente_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudantePaginadoDto
            {
                ComponentesCurriculares = new List<int> { 1 },
                Ano = new List<int> { 5 },
                NivelMinimo = 500,
                NivelMaximo = 600,
                NomeEstudante = "Maria",
                EolEstudante = 123,
                NivelProficiencia = new List<int>{ 1,2,3,4},
                PageNumber = 1,
                PageSize = 4,
                Turma = new List<string> { "A", "B"}
            };

            var dadosMock = new List<AbaEstudanteListaDto>
            {
                new AbaEstudanteListaDto { Turma = "5A", Disciplina = "Matemática", AlunoNome = "Maria", Proficiencia = 600 },
                new AbaEstudanteListaDto { Turma = "5A", Disciplina = "Matemática", AlunoNome = "Joao", Proficiencia = 550 },
                new AbaEstudanteListaDto { Turma = "5B", Disciplina = "Português", AlunoNome = "Pedro", Proficiencia = 650 },
                new AbaEstudanteListaDto { Turma = "5B", Disciplina = "Português", AlunoNome = "Ana", Proficiencia = 700 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbaEstudanteListaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterAbaEstudanteBoletimEscolarPorUeId_DeveFiltrarResultadosCorretamente_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var filtros = new FiltroBoletimEstudantePaginadoDto { };

            var dadosMock = new List<AbaEstudanteListaDto>
            {
                new AbaEstudanteListaDto { Turma = "5A", Disciplina = "Matemática", AlunoNome = "Maria", Proficiencia = 600 },
                new AbaEstudanteListaDto { Turma = "5A", Disciplina = "Matemática", AlunoNome = "Joao", Proficiencia = 550 },
                new AbaEstudanteListaDto { Turma = "5B", Disciplina = "Português", AlunoNome = "Pedro", Proficiencia = 650 },
                new AbaEstudanteListaDto { Turma = "5B", Disciplina = "Português", AlunoNome = "Ana", Proficiencia = 700 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AbaEstudanteListaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterAbaEstudanteBoletimEscolarPorUeId(loteId, ueId, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterResultadoProbabilidadePorUeAsync_DeveFiltrarResultadosCorretamente_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            { 
                Habilidade = "Teste",
                Turma = new List<string> { "5A", "5B" },
                Pagina = 1,
                TamanhoPagina = 10
            };

            var dadosMock = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5B" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5B" },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ResultadoProbabilidadeDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterResultadoProbabilidadePorUeAsync_DeveFiltrarResultadosCorretamente_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto { };

            var dadosMock = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5B" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5B" },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ResultadoProbabilidadeDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterResultadoProbabilidadePorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterResultadoProbabilidadeListaPorUeAsync_DeveFiltrarResultadosCorretamente_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto
            {
                Habilidade = "Teste",
                Turma = new List<string> { "5A", "5B" },
                Pagina = 1,
                TamanhoPagina = 10
            };

            var dadosMock = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5B" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5B" },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ResultadoProbabilidadeDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterResultadoProbabilidadeListaPorUeAsync_DeveFiltrarResultadosCorretamente_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 123L;
            var disciplinaId = 4;
            var anoEscolar = 5;
            var filtros = new FiltroBoletimResultadoProbabilidadeDto { };

            var dadosMock = new List<ResultadoProbabilidadeDto>
            {
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5A" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "123", HabilidadeDescricao = "Teste 1", TurmaDescricao = "5B" },
                new ResultadoProbabilidadeDto { AbaixoDoBasico = 10, Adequado = 20, Avancado = 30, Basico = 20, CodigoHabilidade = "124", HabilidadeDescricao = "Teste 2", TurmaDescricao = "5B" },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ResultadoProbabilidadeDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(dadosMock);

            var (resultado, totalRegistros) = await repositorio.ObterResultadoProbabilidadeListaPorUeAsync(loteId, ueId, disciplinaId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(dadosMock.Count, resultado.Count());
        }

        [Fact]
        public async Task ObterTurmasUeAno_DeveRetornarListaDeTurmasCorretamente()
        {
            var loteId = 1L;
            var ueId = 100L;
            var disciplinaId = 1;
            var anoEscolar = 5;

            var turmasMock = new List<TurmaAnoDto>
            {
                new TurmaAnoDto { Turma = "A", Ano = 5, Descricao = "5A"},
                new TurmaAnoDto { Turma = "B", Ano = 5, Descricao = "5B" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<TurmaAnoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(turmasMock);

            var resultado = await repositorio.ObterTurmasUeAno(loteId, ueId, disciplinaId, anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, t => t.Descricao == "5A" && t.Ano == 5 && t.Turma == "A");
            Assert.Contains(resultado, t => t.Descricao == "5B" && t.Ano == 5 && t.Turma == "B");
        }

        [Fact]
        public async Task ObterAnosAplicacaoPorDre_DeveRetornarListaDeAnosCorretamente()
        {
            var dreId = 1;
            var anosMock = new List<int> { 2024,2025 };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(anosMock);

            var resultado = await repositorio.ObterAnosAplicacaoPorDre(dreId);

            Assert.NotNull(resultado);
            Assert.Equal(anosMock.Count, resultado.Count());
            Assert.Contains(2024, resultado);
            Assert.Contains(2025, resultado);
        }

        [Fact]
        public async Task ObterComponentesCurricularesPorDreAno_DeveRetornarListaDeComponentesCorretamente()
        {
            var dreId = 1;
            var anoEscolar = 5;
            var componentesMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 4, Texto = "Matemática" },
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "Português" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(componentesMock);

            var resultado = await repositorio.ObterComponentesCurricularesPorDreAno(dreId, anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(componentesMock.Count, resultado.Count());
            Assert.Contains(resultado, c => c.Valor == 4 && c.Texto == "Matemática");
            Assert.Contains(resultado, c => c.Valor == 5 && c.Texto == "Português");
        }

        [Fact]
        public async Task ObterAnosEscolaresPorDreAnoAplicacao_DeveRetornarListaDeAnosCorretamente()
        {
            var dreId = 1;
            var anoAplicacao = 2025;
            var disciplinaId= 4;
            var anosMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "5" },
                new OpcaoFiltroDto<int> { Valor = 6, Texto = "6" }
            };
            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(anosMock);

            var resultado = await repositorio.ObterAnosEscolaresPorDreAnoAplicacao(dreId, anoAplicacao, disciplinaId);

            Assert.NotNull(resultado);
            Assert.Equal(anosMock.Count, resultado.Count());
            Assert.Contains(resultado, a => a.Valor == 5 && a.Texto == "5");
            Assert.Contains(resultado, a => a.Valor == 6 && a.Texto == "6");
        }

        [Fact]
        public async Task ObterUesComparacaoPorDre_DeveRetornarListaDeUesCorretamente()
        {
            var dreId = 1;
            var anoAplicacao = 1;
            var anoEscolar = 5;
            var disciplinaId = 4;
            var uesMock = new List<UePorDreDto>
            {
                new UePorDreDto { UeId = 1, UeNome = "EMEF JOAO DE BARRO", TipoEscola = TipoEscola.EMEF, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" },
                new UePorDreDto { UeId = 2, UeNome = "CEI LAR DA CRIANCA", TipoEscola = TipoEscola.CEIINDIR, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<UePorDreDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(uesMock);

            var resultado = await repositorio.ObterUesComparacaoPorDre(dreId, anoAplicacao, disciplinaId, anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(uesMock.Count, resultado.Count());

            Assert.Contains(resultado, u => u.UeId == 1 && u.UeNome == "EMEF JOAO DE BARRO");
            Assert.Contains(resultado, u => u.UeId == 2 && u.UeNome == "CEI LAR DA CRIANCA");
        }

        [Fact]
        public async Task ObterComponentesCurricularesSmePorAno_DeveRetornarListaDeComponentesCorretamente()
        {
            var anoEscolar = 5;
            var componentesMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 4, Texto = "Matemática" },
                new OpcaoFiltroDto<int> { Valor = 5, Texto = "Português" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(componentesMock);

            var resultado = await repositorio.ObterComponentesCurricularesSmePorAno(anoEscolar);
            Assert.NotNull(resultado);
            Assert.Equal(componentesMock.Count, resultado.Count());
            Assert.Contains(resultado, c => c.Valor == 4 && c.Texto == "Matemática");
            Assert.Contains(resultado, c => c.Valor == 5 && c.Texto == "Português");
        }

        [Fact]
        public async Task ObterAnosAplicacaoPorSme_DeveRetornarListaCorreta()
        {
            var anosEsperados = new List<int> { 2023, 2024 };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    null, null, null, null))
                .ReturnsAsync(anosEsperados);

            var resultado = await repositorio.ObterAnosAplicacaoPorSme();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(2023, resultado);
            Assert.Contains(2024, resultado);

            conexaoLeitura.Verify(c => c.Close(), Times.Once);
            conexaoLeitura.Verify(c => c.Dispose(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ObterAnosEscolaresPorSmeAnoAplicacao_DeveRetornarListaCorreta()
        {
            var anoAplicacao = 2024;
            var disciplinaId = 5;
            var anosMock = new List<OpcaoFiltroDto<int>>
            {
                new OpcaoFiltroDto<int> { Valor = 1, Texto = "1º Ano" },
                new OpcaoFiltroDto<int> { Valor = 2, Texto = "2º Ano" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<OpcaoFiltroDto<int>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(anosMock);


            var resultado = await repositorio.ObterAnosEscolaresPorSmeAnoAplicacao(anoAplicacao, disciplinaId);

            Assert.NotNull(resultado);
            Assert.Equal(anosMock.Count, resultado.Count());
            Assert.Contains(resultado, a => a.Valor == 1 && a.Texto == "1º Ano");
            Assert.Contains(resultado, a => a.Valor == 2 && a.Texto == "2º Ano");

            conexaoLeitura.Verify(c => c.Close(), Times.Once);
            conexaoLeitura.Verify(c => c.Dispose(), Times.AtLeastOnce);
        }
    }
}