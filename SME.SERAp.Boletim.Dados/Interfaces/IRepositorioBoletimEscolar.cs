using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimEscolar : IRepositorioBase<BoletimEscolar>
    {
        Task<IEnumerable<BoletimEscolar>> ObterBoletinsPorUe(long loteId, long ueId, FiltroBoletimDto filtros);
        Task<IEnumerable<DownloadProvasBoletimEscolarDto>> ObterDownloadProvasBoletimEscolar(long loteId, long ueId);
        Task<IEnumerable<ProvaBoletimEscolarDto>> ObterProvasBoletimEscolarPorUe(long loteId, long ueId, FiltroBoletimDto filtros);
        Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadResultadoProbabilidade(long loteId, long ueId, long disciplinaId, int anoEscolar);

        Task<int> ObterTotalUesPorDreAsync(long loteId, long dreId, int anoEscolar);
        Task<int> ObterTotalAlunosPorDreAsync(long loteId, long dreId, int anoEscolar);
        Task<IEnumerable<MediaProficienciaDisciplinaDto>> ObterMediaProficienciaPorDreAsync(long loteId, long dreId, int anoEscolar);
        Task<IEnumerable<UePorDreDto>> ObterUesPorDreAsync(long dreId, int anoEscolar, long loteId);
        Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> ObterDownloadProvasBoletimEscolarPorDre(long dreId, long loteId);
        Task<PaginacaoUesBoletimDadosDto> ObterUesPorDre(long loteId, long dreId, int anoEscolar, FiltroUeBoletimDadosDto filtros);
        Task<IEnumerable<UeBoletimDisciplinaProficienciaDto>> ObterDiciplinaMediaProficienciaProvaPorUes(long loteId, long dreId, int anoEscolar, IEnumerable<long> uesIds);

        Task<int> ObterTotalUes(long loteId, int anoEscolar);
        Task<int> ObterTotalDres(long loteId, int anoEscolar);
        Task<int> ObterTotalAlunos(long loteId, int anoEscolar);
        Task<IEnumerable<MediaProficienciaDisciplinaDto>> ObterMediaProficienciaGeral(long loteId, int anoEscolar);
    }
}