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
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Resultado_Comparativo_Por_Turma()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            var turma = "A";
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
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, tiposVariacao, nomeAluno);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaComparativoProvaSPQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoTodasTurmasAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_MemoryStream_Com_Dados_Resultado_Comparativo_Todas_Turmas()
        {
            var ueId = 1;
            var disciplinaId = 1;
            var anoEscolar = 5;
            string? turma = null;
            var loteId = 1L;
            var tiposVariacao = new List<int> { 1 };
            var nomeAluno = "Maria";
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
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Never);
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
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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
            mediator.Setup(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProficienciaComparativoAlunoSpDto)null);

            var result = await useCase.Executar(ueId, disciplinaId, anoEscolar, loteId, turma, null, null);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            mediator.Verify(m => m.Send(It.IsAny<ObterUesAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProficienciaComparativoAlunoSpQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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
    }
}

