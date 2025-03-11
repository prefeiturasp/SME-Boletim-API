using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo
{
    public class ObterAbrangenciaPorLoginGrupoQuery : IRequest<IEnumerable<AbrangenciaDetalheDto>>
    {
        public ObterAbrangenciaPorLoginGrupoQuery(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }
}
