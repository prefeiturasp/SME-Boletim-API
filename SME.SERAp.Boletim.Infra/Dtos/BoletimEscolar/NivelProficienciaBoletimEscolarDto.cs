using SME.SERAp.Boletim.Dominio.Entidades;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class NivelProficienciaBoletimEscolarDto : EntidadeBase
    {
        public int Ano { get; set; }

        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public long Valor { get; set; }
    }
}
