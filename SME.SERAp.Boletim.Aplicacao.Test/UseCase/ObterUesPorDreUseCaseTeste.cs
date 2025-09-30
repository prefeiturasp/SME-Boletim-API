using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterUesPorDreUseCaseTeste
    {
        [Fact]
        public async Task ExecutarAsync_DeveRetornarListaDeUePorDreDto_QuandoMediatorRetornaDados()
        {
            // Arrange
            var dreId = 10L;
            var anoEscolar = 5;
            var loteId = 16L;

            var retornoEsperado = new List<UePorDreDto>
            {
                new UePorDreDto
                {
                    UeId = 1001,
                    UeNome = "EMEF Teste 1",
                    TipoEscola = Dominio.Enumerados.TipoEscola.EMEF,
                    DreNomeAbreviado = "DRE LESTE",
                    DreId = dreId,
                    DreNome = "DIRETORIA REGIONAL DE EDUCACAO LESTE"
                },
                new UePorDreDto
                {
                    UeId = 1002,
                    UeNome = "EMEF Teste 2",
                    TipoEscola = Dominio.Enumerados.TipoEscola.EMEF,
                    DreNomeAbreviado = "DRE LESTE",
                    DreId = dreId,
                    DreNome = "DIRETORIA REGIONAL DE EDUCACAO LESTE"
                }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUesPorDreQuery>(
                        q => q.DreId == dreId && q.AnoEscolar == anoEscolar && q.LoteId == loteId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dreId, anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<UePorDreDto>)resultado).Count);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1001, item.UeId);
                    Assert.Equal("EMEF Teste 1", item.UeNome);
                },
                item =>
                {
                    Assert.Equal(1002, item.UeId);
                    Assert.Equal("EMEF Teste 2", item.UeNome);
                });
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarListaVazia_QuandoMediatorRetornaNada()
        {
            // Arrange
            var dreId = 20L;
            var anoEscolar = 3;
            var loteId = 30L;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UePorDreDto>());

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dreId, anoEscolar, loteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção quando usuário não possui abrangência")]
        public async Task DeveLancarExcecaoSeUsuarioNaoPossuiAbrangencia()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DreAbragenciaDetalheDto>()); // sem abrangência
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Administrador);

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(10, 5, 16));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", ex.Message);
        }

        [Fact(DisplayName = "Deve lançar exceção quando tipoPerfilUsuarioLogado for nulo")]
        public async Task DeveLancarExcecaoSeTipoPerfilNulo()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterDresAbrangenciaUsuarioLogado()); // possui abrangência
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TipoPerfil?)null);

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(10, 5, 16));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", ex.Message);
        }

        [Fact(DisplayName = "Deve lançar exceção quando tipoPerfilUsuarioLogado não possui permissão")]
        public async Task DeveLancarExcecaoSeUsuarioSemPermissao()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterDresAbrangenciaUsuarioLogado());
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoPerfil.Professor); // perfil sem permissão

            var useCase = new ObterUesPorDreUseCase(mediatorMock.Object);

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => useCase.Executar(10, 5, 16));
            Assert.Equal("Usuário não possui abrangências para essa DRE.", ex.Message);
        }


        private IEnumerable<DreAbragenciaDetalheDto> ObterDresAbrangenciaUsuarioLogado()
        {
            return new List<DreAbragenciaDetalheDto>
            {
                new DreAbragenciaDetalheDto { Id = 10, Abreviacao = "DT1", Codigo = "111", Nome = "Dre teste 1"},
                new DreAbragenciaDetalheDto { Id = 20, Abreviacao = "DT2", Codigo = "112", Nome = "Dre teste 2"}
            };
        }
    }
}