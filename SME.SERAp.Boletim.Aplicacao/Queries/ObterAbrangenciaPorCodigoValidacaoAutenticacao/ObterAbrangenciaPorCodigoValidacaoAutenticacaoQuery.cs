using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbrangenciaPorCodigoValidacaoAutenticacao
{
    public class ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery : IRequest<IEnumerable<AbrangenciaDetalheDto>>
    {
        public ObterAbrangenciaPorCodigoValidacaoAutenticacaoQuery(string codigo)
        {
            Codigo = codigo;
        }

        public string Codigo { get; set; }
    }
}
