using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterResultadoProbabilidadePorUeUseCase : IObterResultadoProbabilidadePorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterResultadoProbabilidadePorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ResultadoProbabilidadePaginadoDto> Executar(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros)
        {
            var abrangenciasUsuarioLogado = await mediator.Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var (resultadoProbabilidade, totalRegistros) = await mediator.Send(new ObterResultadoProbabilidadePorUeIdQuery(loteId, ueId, disciplinaId, anoEscolar, filtros));

            if (resultadoProbabilidade == null || !resultadoProbabilidade.Any())
            {
                return new ResultadoProbabilidadePaginadoDto
                {
                    PaginaAtual = filtros.Pagina,
                    TamanhoPagina = filtros.TamanhoPagina,
                    TotalRegistros = 0,
                    TotalPaginas = 0,
                    Resultados = new List<ResultadoProbabilidadeAgrupadoDto>()
                };
            }

            var resultadoAgrupado = resultadoProbabilidade
                .GroupBy(r => new { r.CodigoHabilidade, r.HabilidadeDescricao })
                .Select(grupo => new ResultadoProbabilidadeAgrupadoDto
                {
                    CodigoHabilidade = grupo.Key.CodigoHabilidade,
                    HabilidadeDescricao = grupo.Key.HabilidadeDescricao,
                    Turmas = grupo.Select(t => new TurmaProbabilidadeDto
                    {
                        TurmaDescricao = t.TurmaDescricao,
                        AbaixoDoBasico = t.AbaixoDoBasico,
                        Basico = t.Basico,
                        Adequado = t.Adequado,
                        Avancado = t.Avancado
                    }).OrderBy(t => t.TurmaDescricao).ToList()
                })
                .ToList();

            return new ResultadoProbabilidadePaginadoDto
            {
                PaginaAtual = filtros.Pagina,
                TamanhoPagina = filtros.TamanhoPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtros.TamanhoPagina),
                Resultados = resultadoAgrupado
            };
        } 
    }
}