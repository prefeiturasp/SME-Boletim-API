using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarGraficoPorUeId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAbaEstudanteGraficoPorUeIdUseCase : IObterAbaEstudanteGraficoPorUeIdUseCase
    {
        private readonly IMediator _mediator;

        public ObterAbaEstudanteGraficoPorUeIdUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<AbaEstudanteGraficoDto>> Executar(long ueId)
        {
            var abrangenciasUsuarioLogado = await _mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            return await _mediator.Send(new ObterAbaEstudanteGraficoPorUeIdQuery(ueId));
        }
    }
}