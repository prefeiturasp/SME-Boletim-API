using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterProficienciaDreUseCase : IObterProficienciaDreUseCase
    {
        private readonly IMediator mediator;

        public ObterProficienciaDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ProficienciaDreCompletoDto> Executar(int anoEscolar, long loteId, IEnumerable<long> dreIds = null)
        {
            return await mediator.Send(new ObterProficienciaDreQuery(anoEscolar, loteId, dreIds));
        }
    }
}