using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalAlunosPorDre;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTotalUesPorDre;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarResumoDreUseCase : IObterBoletimEscolarResumoDreUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimEscolarResumoDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarResumoDreDto> Executar(long loteId, long dreId, int anoEscolar)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            var totalUes = await mediator.Send(new ObterTotalUesPorDreQuery(loteId, dreId, anoEscolar));
            var totalAlunos = await mediator.Send(new ObterTotalAlunosPorDreQuery(loteId, dreId, anoEscolar));
            var proficiencias = await mediator.Send(new ObterMediaProficienciaPorDreQuery(loteId, dreId, anoEscolar));

            return new BoletimEscolarResumoDreDto
            {
                TotalUes = totalUes,
                TotalAlunos = totalAlunos,
                ProficienciaDisciplina = proficiencias
            };
        }
    }
}