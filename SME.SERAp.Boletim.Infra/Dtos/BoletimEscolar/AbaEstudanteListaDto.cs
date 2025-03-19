using SME.SERAp.Boletim.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        private static string ObterDescricaoDoNivel(long codigo)
        {
            var tipo = typeof(TipoNivelProficiencia);
            var membro = tipo.GetMember(((TipoNivelProficiencia)codigo).ToString()).FirstOrDefault();

            return membro?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "Nível desconhecido";
        }
    }
}