using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadeListaPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterResultadoProbabilidadePorUeListaUseCase : IObterResultadoProbabilidadePorUeListaUseCase
    {
        private readonly IMediator mediator;

        public ObterResultadoProbabilidadePorUeListaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ResultadoProbabilidadeListaPaginadoDto> Executar(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            var abrangenciasUsuarioLogado = await mediator.Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var (resultadoProbabilidade, totalRegistros) = await mediator.Send(new ObterResultadoProbabilidadeListaPorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros));

            if (resultadoProbabilidade == null || !resultadoProbabilidade.Any())
            {
                return new ResultadoProbabilidadeListaPaginadoDto
                {
                    PaginaAtual = filtros.Pagina,
                    TamanhoPagina = filtros.TamanhoPagina,
                    TotalRegistros = 0,
                    TotalPaginas = 0,
                    Resultados = new List<ResultadoProbabilidadeDto>()
                };
            }

            return new ResultadoProbabilidadeListaPaginadoDto
            {
                PaginaAtual = filtros.Pagina,
                TamanhoPagina = filtros.TamanhoPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtros.TamanhoPagina),
                Resultados = resultadoProbabilidade
            };
        }
    }
}
