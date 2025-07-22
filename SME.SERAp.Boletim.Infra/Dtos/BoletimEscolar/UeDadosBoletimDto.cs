using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeDadosBoletimDto
    {
        public long Id { get;set; }

        public string Nome { get;set; }

        public TipoEscola TipoEscola { get;set; }

        public int AnoEscolar { get;set; }
        
        public IEnumerable<UeBoletimDisciplinaProficienciaDto> Disciplinas { get; set; }

        public int TotalEstudantes { get; set; }

        public int TotalEstudadesRealizaramProva { get; set; }

        public decimal PercentualEstudadesRealizaramProva { get; set; }
    }
}
