using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterMediaProficienciaPorDre;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarResumoSmeUseCase : IObterBoletimEscolarResumoSmeUseCase
    {
        private readonly IMediator mediator;
        public ObterBoletimEscolarResumoSmeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<BoletimEscolarResumoSmeDto> Executar(long loteId, int anoEscolar)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            var totalDres = await mediator.Send(new ObterTotalDresQuery(loteId, anoEscolar));
            var totalUes = await mediator.Send(new ObterTotalUesQuery(loteId, anoEscolar));
            var totalAlunos = await mediator.Send(new ObterTotalAlunosQuery(loteId, anoEscolar));
            var proficiencias = await mediator.Send(new ObterMediaProficienciaGeralQuery(loteId, anoEscolar));

            return new BoletimEscolarResumoSmeDto
            {
                TotalDres = totalDres,
                TotalUes = totalUes,
                TotalAlunos = totalAlunos,
                ProficienciaDisciplina = proficiencias
            };
        }
    }
}