using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp
{
    public class ObterProficienciaComparativoTodasTurmasAlunoSpQuery : IRequest<ProficienciaComparativoAlunoSpDto>
    {
        public ObterProficienciaComparativoTodasTurmasAlunoSpQuery(int ueId, int disciplinaId, int anoEscolar, long loteId, List<int>? tiposVariacao, string? nomeAluno)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            LoteId = loteId;
            TiposVariacao = tiposVariacao;
            NomeAluno = nomeAluno;
        }

        public int UeId { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoEscolar { get; set; }
        public long LoteId { get; set; }
        public List<int>? TiposVariacao { get; set; }
        public string? NomeAluno { get; set; }
    }
}
