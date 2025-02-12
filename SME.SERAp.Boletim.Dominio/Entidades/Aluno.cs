using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class Aluno : EntidadeBase
    {
        public string Nome { get; set; }
        public long RA { get; set; }
        public int Situacao { get; set; }
        public long TurmaId { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeSocial { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public Aluno()
        {

        }

        public Aluno(string nome, long ra)
        {
            Nome = nome;
            RA = ra;
        }
    }
}