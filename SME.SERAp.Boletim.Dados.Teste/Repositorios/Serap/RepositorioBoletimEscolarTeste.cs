using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Teste.Repositorios.Serap
{
    internal class RepositorioBoletimEscolarFake : RepositorioBoletimEscolar
    {
        private readonly IDbConnection _conexaoLeitura;

        public RepositorioBoletimEscolarFake(ConnectionStringOptions options, IDbConnection conexaoLeitura) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
    }

    public class RepositorioBoletimEscolarTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly RepositorioBoletimEscolarFake repositorio;

        public RepositorioBoletimEscolarTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            repositorio = new RepositorioBoletimEscolarFake(new ConnectionStringOptions(), conexaoLeitura.Object);
        }

        [Fact]
        public async Task ObterBoletinsPorUe_DeveRetornarBoletins_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto() { Ano = new List<int> { 9 }, ComponentesCurriculares = new List<int> { 4 } };
            var boletinsMock = new List<BoletimEscolar>
            {
                new BoletimEscolar { Id = 1, ProvaId = 1, UeId = 100, ComponenteCurricular = "Matemática", MediaProficiencia = 500m },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<BoletimEscolar>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(boletinsMock);

            var resultados = await repositorio.ObterBoletinsPorUe(loteId, ueId, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(boletinsMock.Count, resultados.Count());
            Assert.Equal("Matemática", resultados.First().ComponenteCurricular);
        }

        [Fact]
        public async Task ObterBoletinsPorUe_DeveRetornarBoletins_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto();
            var boletinsMock = new List<BoletimEscolar>
            {
                new BoletimEscolar { Id = 1, ProvaId = 1, UeId = 100, ComponenteCurricular = "Matemática", MediaProficiencia = 500m },
                new BoletimEscolar { Id = 2, ProvaId = 2, UeId = 100, ComponenteCurricular = "Português", MediaProficiencia = 550m }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<BoletimEscolar>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(boletinsMock);

            var resultados = await repositorio.ObterBoletinsPorUe(loteId, ueId, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Matemática", resultados.First().ComponenteCurricular);
            Assert.Equal("Português", resultados.Last().ComponenteCurricular);
        }

        [Fact]
        public async Task ObterDownloadProvasBoletimEscolar_DeveRetornarListaDeProvas_QuandoExistiremResultados()
        {
            var loteId = 1L;
            var ueId = 100L;
            var downloadProvasMock = new List<DownloadProvasBoletimEscolarDto>
            {
                new DownloadProvasBoletimEscolarDto { CodigoUE = "123", AlunoRA = 12345, NomeAluno = "Aluno A" },
                new DownloadProvasBoletimEscolarDto { CodigoUE = "123", AlunoRA = 67890, NomeAluno = "Aluno B" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DownloadProvasBoletimEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(downloadProvasMock);

            var resultados = await repositorio.ObterDownloadProvasBoletimEscolar(loteId, ueId);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Aluno A", resultados.First().NomeAluno);
            Assert.Equal("Aluno B", resultados.Last().NomeAluno);
        }

        [Fact]
        public async Task ObterProvasBoletimEscolarPorUe_DeveRetornarProvas_QuandoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto
            {
                Ano = new List<int> { 9 },
                ComponentesCurriculares = new List<int> { 4 }
            };

            var provasMock = new List<ProvaBoletimEscolarDto>
            {
                new ProvaBoletimEscolarDto { Id = 4, Descricao = "Matemática" },
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ProvaBoletimEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(provasMock);

            var resultados = await repositorio.ObterProvasBoletimEscolarPorUe(loteId, ueId, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(provasMock.Count, resultados.Count());
            Assert.Equal("Matemática", resultados.First().Descricao);
        }

        [Fact]
        public async Task ObterProvasBoletimEscolarPorUe_DeveRetornarProvas_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var ueId = 100L;
            var filtros = new FiltroBoletimDto { };

            var provasMock = new List<ProvaBoletimEscolarDto>
            {
                new ProvaBoletimEscolarDto { Id = 1, Descricao = "Matemática" },
                new ProvaBoletimEscolarDto { Id = 2, Descricao = "Português" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<ProvaBoletimEscolarDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(provasMock);

            var resultados = await repositorio.ObterProvasBoletimEscolarPorUe(loteId, ueId, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(provasMock.Count, resultados.Count());
            Assert.Equal("Matemática", resultados.First().Descricao);
            Assert.Equal("Português", resultados.Last().Descricao);
        }

        [Fact]
        public async Task ObterDownloadResultadoProbabilidade_DeveRetornarResultados_QuandoExistiremResultados()
        {
            var loteId = 1L;
            var ueId = 100L;
            var disciplinaId = 12L;
            var anoEscolar = 5;
            var resultadosMock = new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto
                {
                    CodigoHabilidade = "SP.MAT.123",
                    HabilidadeDescricao = "Habilidade 1",
                    CodigoDre = "DRE_A",
                    NomeDreAbreviacao = "DA",
                    CodigoUe = "UE_A",
                    NomeUe = "Escola A",
                    AnoEscolar = 5,
                    Componente = "Matemática",
                    TurmaDescricao = "5o Ano A",
                    AbaixoDoBasico = 10.5m,
                    Basico = 20.2m,
                    Adequado = 35.8m,
                    Avancado = 33.5m
                },
                new DownloadResultadoProbabilidadeDto
                {
                    CodigoHabilidade = "SP.MAT.456",
                    HabilidadeDescricao = "Habilidade 2",
                    CodigoDre = "DRE_A",
                    NomeDreAbreviacao = "DA",
                    CodigoUe = "UE_A",
                    NomeUe = "Escola A",
                    AnoEscolar = 5,
                    Componente = "Matemática",
                    TurmaDescricao = "5o Ano B",
                    AbaixoDoBasico = 5.1m,
                    Basico = 15.7m,
                    Adequado = 40.4m,
                    Avancado = 38.8m
                }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DownloadResultadoProbabilidadeDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(resultadosMock);

            var resultadosObtidos = await repositorio.ObterDownloadResultadoProbabilidade(loteId, ueId, disciplinaId, anoEscolar);

            Assert.NotNull(resultadosObtidos);
            Assert.Equal(2, resultadosObtidos.Count());
            Assert.Equal("SP.MAT.123", resultadosObtidos.First().CodigoHabilidade);
            Assert.Equal(10.5m, resultadosObtidos.First().AbaixoDoBasico);
        }

        [Fact]
        public async Task ObterTotalUesPorDreAsync_DeveRetornarValorCorreto_QuandoHaUes()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 3;
            var totalUesMock = 5;

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(new[] { totalUesMock });

            var resultado = await repositorio.ObterTotalUesPorDreAsync(loteId, dreId, anoEscolar);

            Assert.Equal(totalUesMock, resultado);
        }

        [Fact]
        public async Task ObterTotalAlunosPorDreAsync_DeveRetornarValorCorreto_QuandoHaAlunos()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 3;
            var totalAlunosMock = 150;

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(new[] { totalAlunosMock });

            var resultado = await repositorio.ObterTotalAlunosPorDreAsync(loteId, dreId, anoEscolar);

            Assert.Equal(totalAlunosMock, resultado);
        }

        [Fact]
        public async Task ObterMediaProficienciaPorDreAsync_DeveRetornarMedias_QuandoExistemResultados()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 5;
            var mediasMock = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto { DisciplinaId = 1, DisciplinaNome = "Matemática", MediaProficiencia = 500.50m },
                new MediaProficienciaDisciplinaDto { DisciplinaId = 2, DisciplinaNome = "Português", MediaProficiencia = 550.25m }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<MediaProficienciaDisciplinaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(mediasMock);

            var resultados = await repositorio.ObterMediaProficienciaPorDreAsync(loteId, dreId, anoEscolar);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Matemática", resultados.First().DisciplinaNome);
            Assert.Equal(550.25m, resultados.Last().MediaProficiencia);
        }

        [Fact]
        public async Task ObterUesPorDreAsync_DeveRetornarUes_QuandoExistemResultados()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 5;
            var uesMock = new List<UePorDreDto>
            {
                new UePorDreDto { UeId = 1, UeNome = "EMEF JOAO DE BARRO", TipoEscola = TipoEscola.EMEF, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" },
                new UePorDreDto { UeId = 2, UeNome = "CEI LAR DA CRIANCA", TipoEscola = TipoEscola.CEIINDIR, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<UePorDreDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(uesMock);

            var resultados = await repositorio.ObterUesPorDreAsync(dreId, anoEscolar, loteId);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("EMEF JOAO DE BARRO", resultados.First().UeNome);
            Assert.Equal(TipoEscola.CEIINDIR, resultados.Last().TipoEscola);
        }

        [Fact]
        public async Task ObterDownloadProvasBoletimEscolarPorDre_DeveRetornarProvas_QuandoExistemResultados()
        {
            var loteId = 1L;
            var dreId = 50L;
            var provasDownloadMock = new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto { CodigoDre = 50, NomeDreAbreviacao = "DRE-1", AlunoRA = 12345, NomeAluno = "Aluno Teste A", Proficiencia = 650.75m, Componente = "História", Nivel = "1 - Nível 1" },
                new DownloadProvasBoletimEscolarPorDreDto { CodigoDre = 50, NomeDreAbreviacao = "DRE-1", AlunoRA = 67890, NomeAluno = "Aluno Teste B", Proficiencia = 700.50m, Componente = "Geografia", Nivel = "2 - Nível 2" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DownloadProvasBoletimEscolarPorDreDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(provasDownloadMock);

            var resultados = await repositorio.ObterDownloadProvasBoletimEscolarPorDre(dreId, loteId);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Aluno Teste A", resultados.First().NomeAluno);
            Assert.Equal(700.50m, resultados.Last().Proficiencia);
        }

        [Fact]
        public async Task ObterDiciplinaMediaProficienciaProvaPorUes_DeveRetornarMediasCorretas()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 5;
            var uesIds = new List<long> { 1, 2 };

            var mediasMock = new List<UeBoletimDisciplinaProficienciaDto>
            {
                new UeBoletimDisciplinaProficienciaDto { UeId = 1, DisciplinaId = 1, Disciplina = "Matemática", MediaProficiencia = 500.5m, NivelCodigo = 1, NivelDescricao = "Abaixo do Básico" },
                new UeBoletimDisciplinaProficienciaDto { UeId = 2, DisciplinaId = 2, Disciplina = "Português", MediaProficiencia = 550.2m, NivelCodigo = 2, NivelDescricao = "Básico" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<UeBoletimDisciplinaProficienciaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(mediasMock);

            var resultados = await repositorio.ObterDiciplinaMediaProficienciaProvaPorUes(loteId, dreId, anoEscolar, uesIds);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Matemática", resultados.First().Disciplina);
            Assert.Equal(550.2m, resultados.Last().MediaProficiencia);
        }

        [Fact]
        public async Task ObterTotalUes_DeveRetornarTotalCorreto_QuandoExistemUes()
        {
            var loteId = 1L;
            var anoEscolar = 5;
            var totalUesMock = 10;

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(new[] { totalUesMock });

            var resultado = await repositorio.ObterTotalUes(loteId, anoEscolar);

            Assert.Equal(totalUesMock, resultado);
        }

        [Fact]
        public async Task ObterTotalDres_DeveRetornarTotalCorreto_QuandoExistemDres()
        {
            var loteId = 1L;
            var anoEscolar = 5;
            var totalDresMock = 2;

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(new[] { totalDresMock });

            var resultado = await repositorio.ObterTotalDres(loteId, anoEscolar);

            Assert.Equal(totalDresMock, resultado);
        }

        [Fact]
        public async Task ObterTotalAlunos_DeveRetornarTotalCorreto_QuandoExistemDres()
        {
            var loteId = 1L;
            var anoEscolar = 5;
            var totalAlunosMock = 2;

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(new[] { totalAlunosMock });

            var resultado = await repositorio.ObterTotalAlunos(loteId, anoEscolar);

            Assert.Equal(totalAlunosMock, resultado);
        }

        [Fact]
        public async Task ObterMediaProficienciaGeral_DeveRetornarMedias_QuandoExistemResultados()
        {
            var loteId = 1L;
            var anoEscolar = 5;
            var mediasMock = new List<MediaProficienciaDisciplinaDto>
            {
                new MediaProficienciaDisciplinaDto { DisciplinaId = 1, DisciplinaNome = "Matemática", MediaProficiencia = 550.25m },
                new MediaProficienciaDisciplinaDto { DisciplinaId = 2, DisciplinaNome = "Português", MediaProficiencia = 610.80m }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<MediaProficienciaDisciplinaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(mediasMock);

            var resultados = await repositorio.ObterMediaProficienciaGeral(loteId, anoEscolar);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Matemática", resultados.First().DisciplinaNome);
            Assert.Equal(610.80m, resultados.Last().MediaProficiencia);
        }

        [Fact]
        public async Task ObterDresMediaProficienciaPorDisciplina_DeveRetornarMedias_QuandoExistemResultados()
        {
            var loteId = 1L;
            var anoEscolar = 5L;
            var dresIds = new List<long> { 1, 2 };
            var mediasMock = new List<DreDisciplinaMediaProficienciaDto>
            {
                new DreDisciplinaMediaProficienciaDto { DreId = 1, DreNome = "DRE-A", DisciplinaId = 1, Disciplina = "Matemática", ProvaId = 1, MediaProficiencia = 500.5m },
                new DreDisciplinaMediaProficienciaDto { DreId = 2, DreNome = "DRE-B", DisciplinaId = 2, Disciplina = "Português", ProvaId = 2, MediaProficiencia = 550.2m }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DreDisciplinaMediaProficienciaDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(mediasMock);

            var resultados = await repositorio.ObterDresMediaProficienciaPorDisciplina(loteId, anoEscolar, dresIds);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("DRE-A", resultados.First().DreNome);
            Assert.Equal(550.2m, resultados.Last().MediaProficiencia);
        }

        [Fact]
        public async Task ObterDownloadProvasBoletimEscolarSme_DeveRetornarProvas_QuandoExistemResultados()
        {
            var loteId = 1L;
            var provasMock = new List<DownloadProvasBoletimEscolarPorDreDto>
            {
                new DownloadProvasBoletimEscolarPorDreDto { CodigoDre = 1, NomeDreAbreviacao = "DRE-A", CodigoUE = "UE-A", NomeUE = "Escola A", AnoEscola = 5, Turma = "5o Ano A", AlunoRA = 12345, NomeAluno = "Aluno 1", Componente = "Matemática", Proficiencia = 550.2m, Nivel = "1 - Nível 1" },
                new DownloadProvasBoletimEscolarPorDreDto { CodigoDre = 1, NomeDreAbreviacao = "DRE-A", CodigoUE = "UE-B", NomeUE = "Escola B", AnoEscola = 5, Turma = "5o Ano B", AlunoRA = 67890, NomeAluno = "Aluno 2", Componente = "Português", Proficiencia = 610.8m, Nivel = "2 - Nível 2" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<DownloadProvasBoletimEscolarPorDreDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(provasMock);

            var resultados = await repositorio.ObterDownloadProvasBoletimEscolarSme(loteId);

            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count());
            Assert.Equal("Aluno 1", resultados.First().NomeAluno);
            Assert.Equal(610.8m, resultados.Last().Proficiencia);
        }

        [Fact]
        public async Task ObterDownloadSmeResultadoProbabilidade_DeveRetornarResultados()
        {
            var esperado = new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto { CodigoDre = "01", NomeUe = "UE Teste" }
            };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DownloadResultadoProbabilidadeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterDownloadSmeResultadoProbabilidade(1);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("01", resultado.First().CodigoDre);
        }

        [Fact]
        public async Task ObterDreAsync_DeveRetornarDres()
        {
            var esperado = new List<DreDto> { new DreDto { DreId = 10, DreNome = "DRE Teste" } };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DreDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterDreAsync(5, 99);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(10, resultado.First().DreId);
        }

        [Fact]
        public async Task ObterDownloadDreResultadoProbabilidade_DeveRetornarResultados()
        {
            var esperado = new List<DownloadResultadoProbabilidadeDto>
            {
                new DownloadResultadoProbabilidadeDto { CodigoDre = "02", NomeUe = "UE Dre" }
            };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DownloadResultadoProbabilidadeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterDownloadDreResultadoProbabilidade(1, 2);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("02", resultado.First().CodigoDre);
        }

        [Fact]
        public async Task ObterResumoDreAsync_DeveRetornarResumo()
        {
            var esperado = new List<DreResumoDto>
            {
                new DreResumoDto { DreId = 5, DreNome = "DRE Teste", TotalAlunos = 100 }
            };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DreResumoDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterResumoDreAsync(5, 1);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(5, resultado.First().DreId);
        }

        [Fact]
        public async Task ObterMediaProficienciaDreAsync_DeveRetornarMedias()
        {
            var esperado = new List<DreMediaProficienciaDto>
            {
                new DreMediaProficienciaDto { DreId = 1, Disciplina = "Matemática", MediaProficiencia = 250 }
            };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DreMediaProficienciaDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterMediaProficienciaDreAsync(5, 1);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Matemática", resultado.First().Disciplina);
        }

        [Fact]
        public async Task ObterNiveisProficienciaAsync_DeveRetornarNiveis()
        {
            var esperado = new List<DreNivelProficienciaDto>
            {
                new DreNivelProficienciaDto { DisciplinaId = 1, Ano = 5, Descricao = "Básico", ValorReferencia = 250 }
            };

            conexaoLeitura.SetupDapperAsync(c =>
                c.QueryAsync<DreNivelProficienciaDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterNiveisProficienciaAsync(5);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(5, resultado.First().Ano);
        }

        [Fact]
        public async Task ObterUesPorDre_DeveRetornarUes_QuandoHaFiltros()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto
            {
                Pagina = 1,
                TamanhoPagina = 2,
                UesIds = new List<long> { 1, 2 }
            };

            var uesDadosMock = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto { AnoEscolar = 5, UeNome = "EMEF JOAO DE BARRO", TipoEscola = TipoEscola.EMEF, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" , PercentualEstudadesRealizaramProva = 50, TotalEstudadesRealizaramProva = 10, TotalEstudantes = 20 },
                new UeDadosBoletimDto { AnoEscolar = 5, UeNome = "CEI LAR DA CRIANCA", TipoEscola = TipoEscola.CEIINDIR, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1", PercentualEstudadesRealizaramProva = 50, TotalEstudadesRealizaramProva = 10, TotalEstudantes = 20 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<UeDadosBoletimDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ReturnsAsync(uesDadosMock);

            var resultados = await repositorio.ObterUesPorDre(loteId, dreId, anoEscolar, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(filtros.TamanhoPagina, resultados.TamanhoPagina);
            Assert.Equal(filtros.Pagina, resultados.Pagina);
            Assert.Equal(uesDadosMock.Count, resultados.Itens.Count());
        }

        [Fact]
        public async Task ObterUesPorDre_DeveRetornarUes_QuandoNaoHaFiltros()
        {
            var loteId = 1L;
            var dreId = 50L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto{};

            var uesDadosMock = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto { AnoEscolar = 5, UeNome = "EMEF JOAO DE BARRO", TipoEscola = TipoEscola.EMEF, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1" , PercentualEstudadesRealizaramProva = 50, TotalEstudadesRealizaramProva = 10, TotalEstudantes = 20 },
                new UeDadosBoletimDto { AnoEscolar = 5, UeNome = "CEI LAR DA CRIANCA", TipoEscola = TipoEscola.CEIINDIR, DreId = 50, DreNomeAbreviado = "DRE-1", DreNome = "DIRETORIA REGIONAL DE ENSINO 1", PercentualEstudadesRealizaramProva = 50, TotalEstudadesRealizaramProva = 10, TotalEstudantes = 20 }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<UeDadosBoletimDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ReturnsAsync(uesDadosMock);

            var resultados = await repositorio.ObterUesPorDre(loteId, dreId, anoEscolar, filtros);

            Assert.NotNull(resultados);
            Assert.Equal(uesDadosMock.Count, resultados.Itens.Count());
        }
    }
}