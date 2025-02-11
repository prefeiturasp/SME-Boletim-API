using MediatR;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorLoginGrupo
{
    public class ObterAbrangenciaPorLoginGrupoQuery : IRequest<IEnumerable<AbrangenciaDetalheDto>>
    {
        public ObterAbrangenciaPorLoginGrupoQuery(string login, long grupoId)
        {
            Login = login;
            GrupoId = grupoId;
        }

        public string Login { get; set; }
        public long GrupoId { get; set; }
    }
}
