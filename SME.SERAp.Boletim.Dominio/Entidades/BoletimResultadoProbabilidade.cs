using MessagePack;

namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class BoletimResultadoProbabilidade : EntidadeBase
    {
        public long HabilidadeId { get; set; }

        public string CodigoHabilidade { get; set; }

        public string HabilidadeDescricao { get; set; }

        public string TurmaDescricao { get; set; }

        public long TurmaId { get; set; }

        public long ProvaId { get; set; }

        public long UeId { get; set; }

        public long DisciplinaId { get; set; }

        public int AnoEscolar { get; set; }

        public decimal? AbaixoDoBasico { get; set; }

        public decimal? Basico { get; set; }

        public decimal? Adequado { get; set; }

        public decimal? Avancado { get; set; }

        public DateTime? DataConsolidacao { get; set; }
    }
}