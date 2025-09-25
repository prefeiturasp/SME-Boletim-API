using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterComponentesCurricularesSmePorAnoUseCase : IObterComponentesCurricularesSmePorAnoUseCase
    {
        private readonly IMediator mediator;
        public ObterComponentesCurricularesSmePorAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<OpcaoFiltroDto<int>>> Executar(int anoAplicacao)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            return await mediator.Send(new ObterComponentesCurricularesSmePorAnoQuery(anoAplicacao));
        }
    }
}
