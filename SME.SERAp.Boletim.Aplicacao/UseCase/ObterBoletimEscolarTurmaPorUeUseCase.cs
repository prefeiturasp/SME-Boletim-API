using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresTurmasPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasBoletimEscolarPorUe;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarTurmaPorUeUseCase : IObterBoletimEscolarTurmaPorUeUseCase
    {
        private readonly IMediator mediator;
        public ObterBoletimEscolarTurmaPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarPorTurmaDto> Executar(long ueId)
        {
            var provasBoletimEscola = await mediator.Send(new ObterProvasBoletimEscolarPorUeQuery(ueId));
            if(!provasBoletimEscola?.Any() ?? true)
                throw new NegocioException($"Não foi possível localizar boletins escolar para a UE {ueId}");


            foreach(var prova in provasBoletimEscola)
            {
                var turmas = await ObterBoletinsEscolaresTurmas(ueId, prova);
                prova.Turmas = turmas;

                var niveis = await ObterNiveisProficiencia(ueId, prova);
                prova.Niveis = niveis;
            }

            return new BoletimEscolarPorTurmaDto(provasBoletimEscola);
        }

        private async Task<IEnumerable<ProvaNivelProficienciaBoletimEscolarDto>> ObterNiveisProficiencia(long ueId, ProvaBoletimEscolarDto prova)
        {
            var niveisProciencia = await mediator.Send(new ObterNiveisProficienciaBoletimEscolarPorUeIdProvaIdQuery(ueId, prova.Id));

            return default;
        }

        private async Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmas(long ueId, ProvaBoletimEscolarDto prova)
        {
            return await mediator.Send(new ObterBoletinsEscolaresTurmasPorUeIdProvaIdQuery(ueId, prova.Id));
        }
    }
}
