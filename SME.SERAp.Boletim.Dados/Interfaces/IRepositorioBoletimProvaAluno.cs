using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimProvaAluno : IRepositorioBase<BoletimProvaAluno>
    {
        Task<IEnumerable<TurmaBoletimEscolarDto>> ObterBoletinsEscolaresTurmasPorUeIdProvaId(long ueId, long provaId);
        Task<IEnumerable<NivelProficienciaBoletimEscolarDto>> ObterNiveisProficienciaBoletimEscolarPorUeIdProvaId(long ueId, long provaId);
        Task<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)> ObterAbaEstudanteBoletimEscolarPorUeId(long ueId, int pagina, int tamanhoPagina);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesNiveisProficienciaBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesAnoEscolarBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<int>>> ObterOpcoesComponenteCurricularBoletimEscolarPorUeId(long ueId);
        Task<IEnumerable<OpcaoFiltroDto<string>>> ObterOpcoesTurmaBoletimEscolarPorUeId(long ueId);
        Task<BoletimEscolarValoresNivelProficienciaDto> ObterValoresNivelProficienciaBoletimEscolarPorUeId(long ueId);
    }
}