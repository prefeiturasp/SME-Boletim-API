using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaComparativoProvaSP;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaComparativoProvaSPUseCase : IObterProficienciaComparativoProvaSPUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaComparativoProvaSPUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ProficienciaUeComparacaoProvaSPDto> Executar(long loteId, int ueId, int disciplinaId, int anoEscolar)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            return await mediator.Send(new ObterNiveisProficienciaComparativoProvaSPQuery(loteId, ueId, disciplinaId, anoEscolar));
        }
    }
}