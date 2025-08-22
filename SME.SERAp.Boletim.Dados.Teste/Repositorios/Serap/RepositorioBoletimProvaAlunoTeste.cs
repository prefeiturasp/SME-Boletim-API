using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
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
                NomeEstudante = "Maria"
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
    }
}