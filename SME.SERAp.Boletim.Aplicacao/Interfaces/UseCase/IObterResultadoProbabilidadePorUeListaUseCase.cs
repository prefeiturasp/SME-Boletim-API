using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterResultadoProbabilidadePorUeListaUseCase
    {
        Task<ResultadoProbabilidadeListaPaginadoDto> Executar(long loteId, long ueId, long disciplinaId, int anoEscolar, FiltroBoletimResultadoProbabilidadeDto filtros);
    }
}
