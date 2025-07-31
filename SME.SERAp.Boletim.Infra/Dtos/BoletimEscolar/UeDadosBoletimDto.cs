using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class UeDadosBoletimDto
    {
        public long Id { get;set; }

        public string UeNome { get;set; }

        public string Nome => ConstruirNome();

        public TipoEscola TipoEscola { get;set; }

        public int AnoEscolar { get;set; }
        
        public IEnumerable<UeBoletimDisciplinaProficienciaDto> Disciplinas { get; set; }

        public int TotalEstudantes { get; set; }

        public int TotalEstudadesRealizaramProva { get; set; }

        public decimal PercentualEstudadesRealizaramProva { get; set; }

        public long DreId { get; set; }

        public string DreNomeAbreviado { get; set; }

        public string DreNome { get; set; }

        private string ConstruirNome()
        {
            return UeNome?.ObterUeDescricao(TipoEscola, DreNome, DreNomeAbreviado) ?? string.Empty;
        }
    }
}
