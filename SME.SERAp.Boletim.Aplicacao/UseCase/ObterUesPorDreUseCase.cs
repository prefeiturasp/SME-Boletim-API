using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterUesPorDreUseCase : IObterUesPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterUesPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UePorDreDto>> Executar(long dreId, int anoEscolar, long loteId)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            return await mediator.Send(new ObterUesPorDreQuery(dreId, anoEscolar, loteId));
        }
    }
}