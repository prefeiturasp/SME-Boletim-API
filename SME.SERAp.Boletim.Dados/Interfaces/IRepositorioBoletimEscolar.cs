using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System.Threading.Tasks;

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
        Task<IEnumerable<DreDisciplinaMediaProficienciaDto>> ObterDresMediaProficienciaPorDisciplina(long loteId, long anoEscolar, IEnumerable<long> dresIds);
        Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> ObterDownloadProvasBoletimEscolarSme(long loteId);
        Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadSmeResultadoProbabilidade(long loteId);
        Task<IEnumerable<DreDto>> ObterDreAsync(int anoEscolar, long loteId);
        Task<IEnumerable<DownloadResultadoProbabilidadeDto>> ObterDownloadDreResultadoProbabilidade(long loteId, int dreId);

        Task<IEnumerable<DreResumoDto>> ObterResumoDreAsync(int anoEscolar, long loteId);
        Task<IEnumerable<DreMediaProficienciaDto>> ObterMediaProficienciaDreAsync(int anoEscolar, long loteId);
        Task<IEnumerable<DreNivelProficienciaDto>> ObterNiveisProficienciaAsync(int anoEscolar);
        Task<IEnumerable<UeMediaProficienciaDto>> ObterMediaProficienciaUeAsync(long loteId, int ueId, int disciplinaId, int anoEscolar);
        Task<IEnumerable<UeMediaProficienciaDto>> ObterMediaProficienciaUeAnoAnteriorAsync(long loteId, int ueId, int disciplinaId, int anoEscolar);
        Task<int> ObterTotalAlunosRealizaramProvasUe(long loteId, int anoEscolar, int ueId);
        Task<int> ObterTotalAlunosUeRealizaramProvasSPAnterior(long loteId, int ueId, int disciplinaId, int anoEscolar);
        Task<IEnumerable<ObterNivelProficienciaDto>> ObterNiveisProficienciaPorDisciplinaIdAsync(int disciplinaId, int anoEscolar);
        Task<int> ObterTotalAlunosComProficienciaAsync(int ueId, int disciplinaId, int anoEscolar, string turma, int anoCriacao);
        Task<IEnumerable<AlunoProficienciaDto>> ObterProficienciaAlunoProvaSaberesAsync(int ueId, int disciplinaId, int anoEscolar, string turma, long loteId);
        Task<IEnumerable<AlunoProficienciaDto>> ObterProficienciaAlunoProvaSPAsync(int disciplinaId, int anoLetivo, IEnumerable<long> alunosRa);
        Task<int> ObterAnoPorLoteIdAsync(long loteId);
        Task<IEnumerable<UeProficienciaQueryResultDto>> ObterProficienciaUeProvaSaberesAsync(int dreId, int disciplinaId, int anoLetivo, int anoEscolar, int? ueId = null);
        Task<IEnumerable<UeProficienciaQueryResultDto>> ObterProficienciaUeProvaSPAsync(int dreId, int disciplinaId, int anoLetivo, int anoEscolar, int? ueId = null);

        Task<IEnumerable<ResultadoProeficienciaPorDre>> ObterProficienciaDreProvaSaberesAsync(int dreId, int anoLetivo, int disciplinaId, int anoEscolar);
        Task<IEnumerable<ResultadoProeficienciaPorDre>> ObterProficienciaPorDreProvaSPAsync(int dreId, int anoLetivo  , int disciplinaId , int anoEscolar);
        Task<IEnumerable<ResultadoProeficienciaSme>> ObterProficienciaSmeProvaSaberesAsync(int anoLetivo, int disciplinaId, int anoEscolar);
        Task<IEnumerable<ResultadoProeficienciaSme>> ObterProficienciaSmeProvaSPAsync(int anoLetivo, int disciplinaId, int anoEscolar);

        Task<IEnumerable<ResultadoProeficienciaPorDre>> ObterProficienciasPorSmeProvaSPAsync(int anoLetivo, int disciplinaId, int anoEscolar);
        Task<IEnumerable<ResultadoProeficienciaPorDre>> ObterProficienciaPorSmeProvaSaberesAsync(int anoLetivo, int disciplinaId, int anoEscolar);


        Task<IEnumerable<DreDto>> ObterDresComparativoSmeAsync(int anoAplicacao, int disciplinaId, int anoEscolar);
    }
}