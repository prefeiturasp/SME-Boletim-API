using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterAnosEscolaresPorDreAnoAplicacaoUseCase : IObterAnosEscolaresPorDreAnoAplicacaoUseCase
    {
        private readonly IMediator mediator;
        public ObterAnosEscolaresPorDreAnoAplicacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Executar(long dreId, int anoAplicacao, int disciplinaId)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            return await mediator.Send(new ObterAnosEscolaresPorDreAnoAplicacaoQuery(dreId, anoAplicacao, disciplinaId));
        }
    }
}
