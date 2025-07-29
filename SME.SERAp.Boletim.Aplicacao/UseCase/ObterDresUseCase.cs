using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesPorDre;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDresUseCase : IObterDresUseCase
    {
        private readonly IMediator mediator;

        public ObterDresUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<DreDto>> Executar(int anoEscolar, long loteId)
        {
            return await mediator.Send(new ObterDreQuery(anoEscolar, loteId));
        }
    }
}