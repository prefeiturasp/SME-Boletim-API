using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioLoteProva : IRepositorioBase<LoteProva>
    {
        Task<IEnumerable<LoteProvaDto>> ObterLotesProva();
    }
}
