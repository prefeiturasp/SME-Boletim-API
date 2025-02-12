using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Commands.RemoverCodigoValidacaoAutenticacao
{
    public class RemoverCodigoValidacaoAutenticacaoCommand : IRequest<bool>
    {
        public RemoverCodigoValidacaoAutenticacaoCommand(string codigo)
        {
            Codigo = codigo;
        }

        public string Codigo { get; set; }
    }
}
