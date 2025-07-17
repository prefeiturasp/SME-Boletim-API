using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterUesPorNivelProficienciaDisciplinaPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterUesPorNivelProficienciaDisciplinaPorDreUseCase obterUesPorNivelProficienciaDisciplinaPorDreUseCase;
        public ObterUesPorNivelProficienciaDisciplinaPorDreUseCaseTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.obterUesPorNivelProficienciaDisciplinaPorDreUseCase = new ObterUesPorNivelProficienciaDisciplinaPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Nao_Deve_Permitir_Usuario_Logado_Sem_Abrangencia_Dre()
        {
            var loteId = 1L;
            var dreId = 5L;
            var anoEscolar = 5;

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador_DRE;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var uesPorNiveis = ObterUesNivelProficiencias();
            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesPorNiveis);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", excecao.Message);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Permitir_Usuario_Com_TipoPerfil_Nulo()
        {
            var loteId = 1L;
            var dreId = 4L;
            var anoEscolar = 5;

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<TipoPerfil?>(null!));

            var uesPorNiveis = ObterUesNivelProficiencias();
            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesPorNiveis);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", excecao.Message);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Permitir_Usuario_Logado_Com_TipoPerfil_Invalido()
        {
            var loteId = 1L;
            var dreId = 5L;
            var anoEscolar = 5;

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Diretor;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var uesPorNiveis = ObterUesNivelProficiencias();
            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesPorNiveis);

            var excecao = await Assert.ThrowsAsync<NaoAutorizadoException>(() => obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", excecao.Message);
            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Disciplinas()
        {
            var loteId = 1L;
            var dreId = 3L;
            var anoEscolar = 5;

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador_DRE;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UeNivelProficienciaDto>());

            var resultado = await obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.Disciplinas);
            Assert.Equal(loteId, resultado.LoteId);
            Assert.Equal(dreId, resultado.DreId);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);

            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Disciplinas()
        {
            var loteId = 1L;
            var dreId = 3L;
            var anoEscolar = 5;

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador_DRE;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var uesPorNiveis = ObterUesNivelProficiencias();
            mediator.Setup(x => x.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesPorNiveis);

            var resultado = await obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.Disciplinas);
            Assert.Equal(uesPorNiveis.GroupBy(x => x.DisciplinaId).Count(), resultado.Disciplinas.Count);

            var matematica = resultado.Disciplinas.FirstOrDefault(d => d.DisciplinaId == 4);
            Assert.NotNull(matematica);
            Assert.Equal("Matemática", matematica.DisciplinaNome);
            Assert.Equal(2, matematica.UesPorNiveisProficiencia.Count);

            var matematicaNivelBasico = matematica.UesPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 2);
            Assert.NotNull(matematicaNivelBasico);
            Assert.Equal("Básico", matematicaNivelBasico.Descricao);
            Assert.Equal(1, matematicaNivelBasico.QuantidadeUes);

            var matNivelAvancado = matematica.UesPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 4);
            Assert.NotNull(matNivelAvancado);
            Assert.Equal("Avançado", matNivelAvancado.Descricao);
            Assert.Equal(2, matNivelAvancado.QuantidadeUes);

            var linguaPortuguesa = resultado.Disciplinas.FirstOrDefault(d => d.DisciplinaId == 5);
            Assert.NotNull(linguaPortuguesa);
            Assert.Equal("Língua Portuguesa", linguaPortuguesa.DisciplinaNome);
            Assert.Equal(2, linguaPortuguesa.UesPorNiveisProficiencia.Count);

            var linguaPortuguesaNivelBasico = linguaPortuguesa.UesPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 2);
            Assert.NotNull(linguaPortuguesaNivelBasico);
            Assert.Equal("Básico", linguaPortuguesaNivelBasico.Descricao);
            Assert.Equal(2, linguaPortuguesaNivelBasico.QuantidadeUes);

            var linguaPortuguesaNivelAvancado = linguaPortuguesa.UesPorNiveisProficiencia.FirstOrDefault(n => n.Codigo == 4);
            Assert.NotNull(linguaPortuguesaNivelAvancado);
            Assert.Equal("Avançado", linguaPortuguesaNivelAvancado.Descricao);
            Assert.Equal(1, linguaPortuguesaNivelAvancado.QuantidadeUes);

            Assert.Equal(loteId, resultado.LoteId);
            Assert.Equal(dreId, resultado.DreId);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);

            mediator.Verify(m => m.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterNiveisProficienciaUesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private IEnumerable<DreAbragenciaDetalheDto> ObterDresAbrangenciaUsuarioLogado()
        {
            return new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 1, Abreviacao = "DT1", Codigo = "111", Nome = "Dre teste 1"},
                new DreAbragenciaDetalheDto { Id = 2, Abreviacao = "DT2", Codigo = "112", Nome = "Dre teste 2"},
                new DreAbragenciaDetalheDto { Id = 3, Abreviacao = "DT3", Codigo = "113", Nome = "Dre teste 3"},
                new DreAbragenciaDetalheDto { Id = 4, Abreviacao = "DT4", Codigo = "114", Nome = "Dre teste 4"},
            };
        }

        private IEnumerable<UeNivelProficienciaDto> ObterUesNivelProficiencias()
        {
            return new List<UeNivelProficienciaDto>
            {
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "111", Nome = "UE Teste 1", MediaProficiencia = 150.32M, DisciplinaId = 4, Disciplina = "Matemática", NivelCodigo = 2, NivelDescricao = "Básico" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "112", Nome = "UE Teste 2", MediaProficiencia = 250.21M, DisciplinaId = 4, Disciplina = "Matemática", NivelCodigo = 4, NivelDescricao = "Avançado" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "113", Nome = "UE Teste 3", MediaProficiencia = 267.56M, DisciplinaId = 4, Disciplina = "Matemática", NivelCodigo = 4, NivelDescricao = "Avançado" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "111", Nome = "UE Teste 1", MediaProficiencia = 150.32M, DisciplinaId = 5, Disciplina = "Língua Portuguesa", NivelCodigo = 2, NivelDescricao = "Básico" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "112", Nome = "UE Teste 2", MediaProficiencia = 120.11M, DisciplinaId = 5, Disciplina = "Língua Portuguesa", NivelCodigo = 2, NivelDescricao = "Básico" },
                new UeNivelProficienciaDto { AnoEscolar = 5, Codigo = "113", Nome = "UE Teste 3", MediaProficiencia = 297.26M, DisciplinaId = 5, Disciplina = "Língua Portuguesa", NivelCodigo = 4, NivelDescricao = "Avançado" }
            };
        }
    }
}
