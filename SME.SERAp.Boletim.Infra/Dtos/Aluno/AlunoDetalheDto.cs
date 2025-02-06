using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Aluno
{
    public class AlunoDetalheDto : DtoBase
    {
        public long AlunoId { get; set; }
        public string DreAbreviacao { get; set; }
        public string Escola { get; set; }
        public string Turma { get; set; }
        public string Nome { get; set; }
        public string NomeSocial { get; set; }

        public string NomeFinal()
        {
            return string.IsNullOrEmpty(NomeSocial) ? Nome : NomeSocial;
        }
    }
}