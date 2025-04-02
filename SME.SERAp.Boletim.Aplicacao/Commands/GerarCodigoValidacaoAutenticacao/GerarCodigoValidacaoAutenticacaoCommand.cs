using MediatR;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao
{
    public class GerarCodigoValidacaoAutenticacaoCommand : IRequest<AutenticacaoValidarDto>
    {
        public GerarCodigoValidacaoAutenticacaoCommand(IEnumerable<AbrangenciaDetalheDto> abrangencias)
        {
            Abrangencias = abrangencias;
        }

        public IEnumerable<AbrangenciaDetalheDto> Abrangencias { get; set; }
    }
}
