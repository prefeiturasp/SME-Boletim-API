using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.GerarCodigoValidacaoAutenticacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Cache;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Commands.CalculoProbabilidade
{
   public class CalcularProbabilidadeCommandHandler : IRequestHandler<CalcularProbabilidadeCommand, double>
    {
    
        public CalcularProbabilidadeCommandHandler(IRepositorioCache repositorioCache)
        {
            
        }

        public async Task<double> Handle(CalcularProbabilidadeCommand request, CancellationToken cancellationToken)
        {
           var probabilidadeAluno = request.AcertoCasual + ((1 - request.AcertoCasual) * (1 / (1 + Math.Exp(-1.7 * request.Discriminacao * (request.Proficiencia - request.Dificuldade)))));

            return probabilidadeAluno;
        }
    }
}
