using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioAbrangencia : IRepositorioBase<Abrangencia>
    {
        Task<IEnumerable<AbrangenciaDetalheDto>> ObterAbrangenciaPorLogin(string login);

        Task<IEnumerable<AbrangenciaDetalheDto>> ObterAbrangenciaPorLoginGrupo(string login, Guid perfil);

        Task<IEnumerable<AbrangenciaUeDto>> ObterUesPorAbrangenciaDre(long dreId, long? ueId);

        Task<IEnumerable<AbrangenciaUeDto>> ObterUesAdministrador();
    }
}
