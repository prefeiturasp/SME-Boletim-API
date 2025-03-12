using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class BoletimProvaAluno : EntidadeBase
    {
        public long DreId { get; set; }

        public string CodigoUe { get; set; }

        public string NomeUe { get; set; }

        public long ProvaId { get; set; }

        public string ProvaDescricao { get; set; }

        public int AnoEscolar { get; set; }

        public string Turma { get; set; }

        public long AlunoRa { get; set; }

        public string AlunoNome { get; set; }

        public string Disciplina { get; set; }

        public long DisciplinaId { get; set; }

        public ProvaStatus ProvaStatus { get; set; }

        public decimal Proficiencia { get; set; }

        public decimal ErroMedida { get; set; }

        public long NivelCodigo { get; set; }
    }
}
