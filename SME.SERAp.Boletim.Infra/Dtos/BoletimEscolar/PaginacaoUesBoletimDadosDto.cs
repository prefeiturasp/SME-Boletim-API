
namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class PaginacaoUesBoletimDadosDto : PaginacaoDto<UeDadosBoletimDto>
    {
        public PaginacaoUesBoletimDadosDto(IEnumerable<UeDadosBoletimDto> itens, int pagina, int tamanhoPagina, int totalRegistros) : base(itens, pagina, tamanhoPagina, totalRegistros)
        {
        }
    }
}
