using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarPorUeUseCase : IObterBoletimEscolarPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimEscolarPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<BoletimEscolar>> Executar(long ueId)
        {
            var boletins = await mediator.Send(new ObterBoletimEscolarPorUeQuery(ueId));

            if (boletins != null)
                return boletins;

            throw new NegocioException($"Não foi possível localizar boletins para a UE {ueId}");
        }
    }
}