using MediatR;
using SME.SERAp.Boletim.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunoSerapPorRa
{
    public class ObterAlunoSerapPorRaQuery : IRequest<Aluno>
    {
        public ObterAlunoSerapPorRaQuery(long alunoRA)
        {
            AlunoRA = alunoRA;
        }
        public long AlunoRA { get; set; }
    }
}