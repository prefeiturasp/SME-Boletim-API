using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimDadosUesPorDre;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Teste.UseCase
{
    public class ObterBoletimDadosUesPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterBoletimDadosUesPorDreUseCase useCase;
        public ObterBoletimDadosUesPorDreUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterBoletimDadosUesPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Das_Ues_Por_Dre()
        {
            var itens = new List<UeDadosBoletimDto>
            {
                new UeDadosBoletimDto
                {
                    Id = 1,
                    UeNome = "Escola A",
                    TotalEstudantes = 100,
                    TotalEstudadesRealizaramProva = 80,
                    Disciplinas = new List<UeBoletimDisciplinaProficienciaDto>()
                },
                new UeDadosBoletimDto
                {
                    Id = 2,
                    UeNome = "Escola B",
                    TotalEstudantes = 150,
                    TotalEstudadesRealizaramProva = 120,
                    Disciplinas = new List<UeBoletimDisciplinaProficienciaDto>()
                }
            };

            var loteId = 1L;
            var dreId = 10L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto() { TamanhoPagina = 10 };

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            mediator
                .Setup(m => m.Send(It.IsAny<ObterBoletimDadosUesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoUesBoletimDadosDto(itens, filtros.Pagina, filtros.TamanhoPagina, itens.Count()));
            var resultado = await useCase.Executar(loteId, dreId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(resultado.Pagina, filtros.Pagina);
            Assert.Equal(resultado.TamanhoPagina, filtros.TamanhoPagina);
            Assert.Equal(resultado.Itens.Count(), itens.Count());
            mediator.Verify(m => m.Send(It.IsAny<ObterBoletimDadosUesPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Dados_Das_Ues_Por_Dre()
        {

            var loteId = 1L;
            var dreId = 20L;
            var anoEscolar = 5;
            var filtros = new FiltroUeBoletimDadosDto() { TamanhoPagina = 2 };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterBoletimDadosUesPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoUesBoletimDadosDto(new List<UeDadosBoletimDto>(), filtros.Pagina, filtros.TamanhoPagina, 0));

            var dresAbrangencia = ObterDresAbrangenciaUsuarioLogado();
            mediator.Setup(x => x.Send(It.IsAny<ObterDresAbrangenciaUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresAbrangencia);

            var tipoPerfilUsuarioLogado = TipoPerfil.Administrador;
            mediator.Setup(x => x.Send(It.IsAny<ObterTipoPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoPerfilUsuarioLogado);

            var resultado = await useCase.Executar(loteId, dreId, anoEscolar, filtros);

            Assert.NotNull(resultado);
            Assert.Equal(resultado.Pagina, filtros.Pagina);
            Assert.Equal(resultado.TamanhoPagina, filtros.TamanhoPagina);
            Assert.Empty(resultado.Itens);
            mediator.Verify(m => m.Send(It.IsAny<ObterBoletimDadosUesPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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
