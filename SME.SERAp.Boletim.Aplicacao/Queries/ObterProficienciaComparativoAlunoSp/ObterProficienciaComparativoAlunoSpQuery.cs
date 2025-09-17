using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp
{
    public class ObterProficienciaComparativoAlunoSpQuery : IRequest<ProficienciaComparativoAlunoSpDto>
    {
        public ObterProficienciaComparativoAlunoSpQuery(int ueId, int disciplinaId, int anoEscolar, string turma, long loteId, int? tipoVariacao, string? nomeAluno, int? pagina, int? itensPorPagina)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            Turma = turma;
            LoteId = loteId;
            TipoVariacao = tipoVariacao;
            NomeAluno = nomeAluno;
            Pagina = pagina;
            ItensPorPagina = itensPorPagina;
        }

        public int UeId { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
        public string Turma { get; set; }
        public long LoteId { get; set; }
        public int? TipoVariacao { get; set; }
        public string? NomeAluno { get; set; }
        public int? Pagina { get; set; }
        public int? ItensPorPagina { get; set; }
    }
}