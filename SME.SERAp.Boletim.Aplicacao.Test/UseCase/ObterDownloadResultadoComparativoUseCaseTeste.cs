using ClosedXML.Excel;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Test.UseCase
{
    public class ObterDownloadResultadoComparativoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDownloadResultadoComparativoUseCase useCase;

        public ObterDownloadResultadoComparativoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDownloadResultadoComparativoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Resultado_Comparativo_Todas_Turmas()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            string? turma = null;
            var loteId = 1L;
            var tiposVariacao = new List<int> { 1, 2 };
            var nomeAluno = "João Silva";
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoAlunoSpDto();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, tiposVariacao, nomeAluno);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Sem_Itens_Resultado_Comparativo()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoAlunoSpDtoVazio();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Nulos()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProficienciaComparativoAlunoSpDto)null);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Sem_ProvaSP()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            string? turma = null;
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = new ProficienciaUeComparacaoProvaSPDto { ProvaSP = null };
            var dados = ObterProficienciaComparativoAlunoSpDto();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Turma_Especifica()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoMultiplasTurmas();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Multiplas_Turmas()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            string? turma = null;
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoMultiplasTurmas();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Alunos_Sem_Proficiencias()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoSemProficiencias();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Todos_Niveis_Proficiencia()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoTodosNiveis();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Variacoes_Positivas_Negativas_E_Neutras()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoComVariacoes();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_ProvaSP_Periodo_Igual_NomeAplicacao()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = new ProficienciaUeComparacaoProvaSPDto
            {
                ProvaSP = new ProficienciaProvaSpDto
                {
                    NomeAplicacao = "Prova SP 2024",
                    Periodo = "Prova SP 2024",
                    MediaProficiencia = 550m
                }
            };
            var dados = ObterProficienciaComparativoAlunoSpDto();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Nivel_Proficiencia_Nulo()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoNivelNulo();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task Deve_Escrever_Cabecalhos_Raca_Cor_E_Genero_Apos_Nome_Do_Estudante()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoComRacaSexo();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            using var workbook = new XLWorkbook(result);
            var ws = workbook.Worksheet("Comparativo");
            var linhaCabecalho = EncontrarLinhaPorValorNaColuna1(ws, "Nome do estudante");

            Assert.Equal("Raça/Cor", ws.Cell(linhaCabecalho, 2).GetString());
            Assert.Equal("Gênero", ws.Cell(linhaCabecalho, 3).GetString());
        }

        [Fact]
        public async Task Deve_Escrever_Raca_E_Genero_De_Cada_Estudante_Com_Fallback_Nao_Informado()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "5A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();
            var dadosProvaSP = ObterDadosProvaSP();
            var dados = ObterProficienciaComparativoComRacaSexo();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosProvaSP);
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            using var workbook = new XLWorkbook(result);
            var ws = workbook.Worksheet("Comparativo");

            var linhaJoao = EncontrarLinhaPorValorNaColuna1(ws, "João Silva");
            Assert.Equal("Parda", ws.Cell(linhaJoao, 2).GetString());
            Assert.Equal("Masculino", ws.Cell(linhaJoao, 3).GetString());

            var linhaMaria = EncontrarLinhaPorValorNaColuna1(ws, "Maria Santos");
            Assert.Equal("Não informado", ws.Cell(linhaMaria, 2).GetString());
            Assert.Equal("Feminino", ws.Cell(linhaMaria, 3).GetString());

            var linhaPedro = EncontrarLinhaPorValorNaColuna1(ws, "Pedro Costa");
            Assert.Equal("Preta", ws.Cell(linhaPedro, 2).GetString());
            Assert.Equal("Não informado", ws.Cell(linhaPedro, 3).GetString());
        }

        [Fact]
        public async Task Deve_Retornar_Excecao_Usuario_Sem_Abrangencia_Para_Ue()
        {
            var ueId = 999;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
            var loteId = 1L;
            var abrangencias = ObterAbrangencias();

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(abrangencias);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null));
            Assert.Equal("Usuário não possui abrangências para essa UE.", excecao.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Excecao_Abrangencias_Nulas()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
            var loteId = 1L;

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AbrangenciaUeDto>)null);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null));
            Assert.Equal("Usuário não possui abrangências para essa UE.", excecao.Message);
        }

        [Fact]
        public async Task Deve_Retornar_Excecao_Abrangencias_Vazia()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
            var loteId = 1L;

            mediator.Setup(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeDto>());

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null));
            Assert.Equal("Usuário não possui abrangências para essa UE.", excecao.Message);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static IEnumerable<AbrangenciaUeDto> ObterAbrangencias()
        {
            return new List<AbrangenciaUeDto>
            {
                new AbrangenciaUeDto { UeId = 1, UeNome = "Escola Teste 1", DreId = 1, DreAbreviacao = "DRE 1" },
                new AbrangenciaUeDto { UeId = 2, UeNome = "Escola Teste 2", DreId = 1, DreAbreviacao = "DRE 1" }
            };
        }

        private static ProficienciaUeComparacaoProvaSPDto ObterDadosProvaSP()
        {
            return new ProficienciaUeComparacaoProvaSPDto
            {
                ProvaSP = new ProficienciaProvaSpDto
                {
                    NomeAplicacao = "Prova SP",
                    Periodo = "2024",
                    MediaProficiencia = 550m
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoAlunoSpDto()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril", "Junho" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "João Silva",
                        Turma = "5A",
                        Variacao = 10.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 550m, NivelProficiencia = "Adequado" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 520m, NivelProficiencia = "Básico" },
                            new ProficienciaDetalheDto { Mes = "Junho", Valor = 580m, NivelProficiencia = "Avançado" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Maria Santos",
                        Turma = "5A",
                        Variacao = -5.2,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 480m, NivelProficiencia = "Abaixo do Básico" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 490m, NivelProficiencia = "Básico" }
                        }
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoAlunoSpDtoVazio()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                Aplicacoes = new List<string>(),
                Itens = new List<ProficienciaAlunoDto>()
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoMultiplasTurmas()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril", "Junho" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "João Silva",
                        Turma = "5A",
                        Variacao = 10.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 550m, NivelProficiencia = "Adequado" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 520m, NivelProficiencia = "Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Maria Santos",
                        Turma = "5B",
                        Variacao = -5.2,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 480m, NivelProficiencia = "Básico" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 490m, NivelProficiencia = "Adequado" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Pedro Costa",
                        Turma = "5A",
                        Variacao = 0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 500m, NivelProficiencia = "Básico" }
                        }
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoSemProficiencias()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "João Silva",
                        Turma = "5A",
                        Variacao = 0,
                        Proficiencias = null
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Maria Santos",
                        Turma = "5A",
                        Variacao = 0,
                        Proficiencias = new List<ProficienciaDetalheDto>()
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoTodosNiveis()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Abaixo",
                        Turma = "5A",
                        Variacao = -10.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 400m, NivelProficiencia = "Abaixo do Básico" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 420m, NivelProficiencia = "Abaixo do Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Básico",
                        Turma = "5A",
                        Variacao = 5.0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 500m, NivelProficiencia = "Básico" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 510m, NivelProficiencia = "Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Adequado",
                        Turma = "5A",
                        Variacao = 8.0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 600m, NivelProficiencia = "Adequado" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 620m, NivelProficiencia = "Adequado" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Avançado",
                        Turma = "5A",
                        Variacao = 12.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 700m, NivelProficiencia = "Avançado" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 720m, NivelProficiencia = "Avançado" }
                        }
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoComVariacoes()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Positivo",
                        Turma = "5A",
                        Variacao = 15.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 550m, NivelProficiencia = "Adequado" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Negativo",
                        Turma = "5A",
                        Variacao = -8.3,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 480m, NivelProficiencia = "Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Aluno Neutro",
                        Turma = "5A",
                        Variacao = 0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 500m, NivelProficiencia = "Básico" }
                        }
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoNivelNulo()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "João Silva",
                        Turma = "5A",
                        Variacao = 5.0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 550m, NivelProficiencia = null },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 520m, NivelProficiencia = "Desconhecido" }
                        }
                    }
                }
            };
        }

        private static ProficienciaComparativoAlunoSpDto ObterProficienciaComparativoComRacaSexo()
        {
            return new ProficienciaComparativoAlunoSpDto
            {
                NomeDisciplina = "Matemática",
                NomeLote = "Lote 2024",
                UeDescricao = "EMEF Teste",
                Aplicacoes = new List<string> { "Abril" },
                Itens = new List<ProficienciaAlunoDto>
                {
                    new ProficienciaAlunoDto
                    {
                        Nome = "João Silva",
                        Raca = "Parda",
                        Sexo = "M",
                        Turma = "5A",
                        Variacao = 10.5,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 550m, NivelProficiencia = "Adequado" },
                            new ProficienciaDetalheDto { Mes = "Abril", Valor = 520m, NivelProficiencia = "Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Maria Santos",
                        Raca = null,
                        Sexo = "F",
                        Turma = "5A",
                        Variacao = -5.2,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 480m, NivelProficiencia = "Básico" }
                        }
                    },
                    new ProficienciaAlunoDto
                    {
                        Nome = "Pedro Costa",
                        Raca = "Preta",
                        Sexo = null,
                        Turma = "5A",
                        Variacao = 0,
                        Proficiencias = new List<ProficienciaDetalheDto>
                        {
                            new ProficienciaDetalheDto { Mes = string.Empty, Valor = 500m, NivelProficiencia = "Básico" }
                        }
                    }
                }
            };
        }

        private static int EncontrarLinhaPorValorNaColuna1(IXLWorksheet ws, string valor)
        {
            return ws.Column(1).CellsUsed()
                .Single(c => c.GetString() == valor)
                .Address.RowNumber;
        }
    }
}

