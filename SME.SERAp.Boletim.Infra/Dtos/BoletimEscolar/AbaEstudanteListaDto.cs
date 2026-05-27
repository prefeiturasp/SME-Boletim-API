using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar
{
    public class BoletimEscolarComDisciplinasDto
    {
        public IEnumerable<string> Disciplinas { get; set; }
        public PaginacaoDto<AbaEstudanteListaDto> Estudantes { get; set; }
    }

    public class AbaEstudanteListaDto
    {
        public string Disciplina { get; set; }
        public int AnoEscolar { get; set; }
        public string Turma { get; set; }
        public long AlunoRa { get; set; }
        public string AlunoNome { get; set; }
        public decimal Proficiencia { get; set; }
        public long NivelCodigo { get; set; }
        public string NivelDescricao => ObterDescricaoDoNivel(NivelCodigo);

        private string _sexo;
        public string Sexo
        {
            get => _sexo;
            set => _sexo = value;
        }

        public string SexoDescricao => _sexo switch
        {
            "M" => "Masculino",
            "F" => "Feminino",
            _ => "Não informado"
        };

        public bool Pap { get; set; }
        public bool Aee { get; set; }
        public string? Raca { get; set; }
        public bool PossuiDeficiencia { get; set; }

        private static string ObterDescricaoDoNivel(long codigo)
        {
            var tipo = typeof(TipoNivelProficiencia);
            var membro = tipo.GetMember(((TipoNivelProficiencia)codigo).ToString()).FirstOrDefault();
            return membro?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "Nível desconhecido";
        }
    }
}