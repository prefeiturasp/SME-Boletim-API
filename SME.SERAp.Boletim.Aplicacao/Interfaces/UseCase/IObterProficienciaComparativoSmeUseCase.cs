using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase
{
    public interface IObterProficienciaComparativoSmeUseCase
    {
        Task<TabelaComparativaSmePspPsaDto> Executar(int anoLetivo, int disciplinaId, int anoEscolar);
    }
}
