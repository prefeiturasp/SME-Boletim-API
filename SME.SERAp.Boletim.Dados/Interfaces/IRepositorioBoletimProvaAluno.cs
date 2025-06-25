using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimProvaAluno : IRepositorioBase<BoletimProvaAluno>
    {
        Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long ueId, long provaId, FiltroBoletimDto filtros);
        Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long ueId, long provaId, FiltroBoletimDto filtros);
        Task<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)> ObterAbaEstudanteBoletimEscolarPorUeId(long loteId, long ueId, FiltroBoletimEstudantePaginadoDto filtros);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesAnoEscolarBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<string>>> ObterOpcoesTurmaBoletimEscolarPorUeId(long ueId);
        Task<BoletimEscolarValoresNivelProficienciaDto> ObterValoresNivelProficienciaBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<AbaEstudanteGraficoDto>> ObterAbaEstudanteGraficoPorUeId(long ueId, FiltroBoletimEstudanteDto filtros);
        Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadePorUeAsync(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
        Task<(IEnumerable<ResultadoProbabilidadeDto>, int)> ObterResultadoProbabilidadeListaPorUeAsync(long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
    }
}