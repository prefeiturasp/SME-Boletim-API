using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimProvaAluno : IRepositorioBase<BoletimProvaAluno>
    {
        Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long loteId, long ueId, long provaId, FiltroBoletimDto filtros);
        Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long loteId, long ueId, long provaId, FiltroBoletimDto filtros);
        Task<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)> ObterAbaEstudanteBoletimEscolarPorUeId(long loteId, long ueId, FiltroBoletimEstudantePaginadoDto filtros);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(long loteId, long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesAnoEscolarBoletimEscolarPorUeId(long loteId, long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(long loteId, long ueId);
        Task<IEnumerable<OpcaoFiltroDto<string>>> ObterOpcoesTurmaBoletimEscolarPorUeId(long loteId, long ueId);
        Task<BoletimEscolarValoresNivelProficienciaDto> ObterValoresNivelProficienciaBoletimEscolarPorUeId(long loteId, long ueId);
        Task<IEnumerable<AbaEstudanteGraficoDto>> ObterAbaEstudanteGraficoPorUeId(long loteId, long ueId, FiltroBoletimEstudanteDto filtros);
        Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadePorUeAsync(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
        Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadeListaPorUeAsync(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
        Task<IEnumerable<UeNivelProficienciaDto>> ObterNiveisProficienciaUes(long dreId, int anoEscolar, long loteId);
        Task<IEnumerable<NivelProficienciaDto>> ObterNiveisProficienciaDisciplinas(int anoEscolar, long loteId);
        Task<IEnumerable<TurmaAnoDto>> ObterTurmasUeAno(long loteId, long ueId, int disciplinaId, int anoEscolar);
        Task<IEnumerable<int>> ObterAnosAplicacaoPorDre(long dreId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterComponentesCurricularesPorDreAno(long dreId, int anoAplicacao);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterAnosEscolaresPorDreAnoAplicacao(long dreId, int anoAplicacao, int disciplinaId);
        Task<IEnumerable<UePorDreDto>> ObterUesComparacaoPorDre(long dreId, int anoAplicacao, int disciplinaId, int anoEscolar);
        Task<IEnumerable<int>> ObterAnosAplicacaoPorSme();
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterComponentesCurricularesSmePorAno(int anoAplicacao);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterAnosEscolaresPorSmeAnoAplicacao(int anoAplicacao, int disciplinaId);
    }
}