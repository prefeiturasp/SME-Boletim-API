using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt
{
    public class ObterTokenJwtQuery : IRequest<AutenticacaoRetornoDto>
    {
        public ObterTokenJwtQuery(IEnumerable<AbrangenciaDetalheDto> abrangencias)
        {
            Abrangencias = abrangencias;
        }

        public IEnumerable<AbrangenciaDetalheDto> Abrangencias { get; set; }
    }
}
