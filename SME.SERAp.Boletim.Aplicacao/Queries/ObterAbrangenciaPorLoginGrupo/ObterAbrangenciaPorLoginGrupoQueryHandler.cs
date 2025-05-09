using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo
{
    public class ObterAbrangenciaPorLoginGrupoQueryHandler : IRequestHandler<ObterAbrangenciaPorLoginGrupoQuery, IEnumerable<AbrangenciaDetalheDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaPorLoginGrupoQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaDetalheDto>> Handle(ObterAbrangenciaPorLoginGrupoQuery request, CancellationToken cancellationToken)
        {
            if(request.Perfil != Guid.Empty && Perfis.PerfilEhAdministrador(request.Perfil))
                return new List<AbrangenciaDetalheDto> { new AbrangenciaDetalheDto(request.Login, request.Perfil) };

            return await repositorioAbrangencia.ObterAbrangenciaPorLoginGrupo(request.Login, request.Perfil);
        }
    }
}
