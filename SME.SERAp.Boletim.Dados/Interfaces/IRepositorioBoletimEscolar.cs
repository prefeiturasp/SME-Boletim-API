using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimEscolar : IRepositorioBase<BoletimEscolar>
    {
        Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long loteId, long ueId, FiltroBoletimDto filtros);
        Task<IEnumerable<DownloadProvasBoletimEscolarDto>> ObterDownloadProvasBoletimEscolar(long ueId);
        Task<IEnumerable<ProvaBoletimEscolarDto>> ObterProvasBoletimEscolarPorUe(long loteId, long ueId, FiltroBoletimDto filtros);
        Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadResultadoProbabilidade(long ueId, long disciplinaId, int anoEscolar);
    }
}