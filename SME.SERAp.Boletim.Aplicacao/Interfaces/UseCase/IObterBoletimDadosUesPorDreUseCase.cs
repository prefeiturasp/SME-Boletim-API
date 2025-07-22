using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterBoletimDadosUesPorDreUseCase
    {
        Task<PaginacaoUesBoletimDadosDto> Executar(long loteId, long dreId, int anoEscolar, FiltroUeBoletimDadosDto filtros);
    }
}
