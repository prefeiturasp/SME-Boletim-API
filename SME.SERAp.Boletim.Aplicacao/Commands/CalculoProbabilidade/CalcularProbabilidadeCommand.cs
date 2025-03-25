using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Aplicacao.Commands.CalculoProbabilidade
{
    public class CalcularProbabilidadeCommand : IRequest<double>
    {
        public CalcularProbabilidadeCommand(double acertoCasual, double dificuldade, double discriminacao, double proficiencia)
        {
            AcertoCasual = acertoCasual;
            Dificuldade = dificuldade;
            Discriminacao = discriminacao;
            Proficiencia = proficiencia;
        }
        public double AcertoCasual { get; set; }
        public double Dificuldade { get; set; }
        public double Discriminacao { get; set; }
        public double Proficiencia { get; set; }

    }
}

