using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoUe
{
    public class ObterProficienciaComparativoUeQuery : IRequest<ProficienciaComparativoUeDto>
    {
        public ObterProficienciaComparativoUeQuery(int dreId, int disciplinaId, int anoLetivo, int anoEscolar, int? ueId, List<int>? tiposVariacao, string? nomeUe, int? pagina, int? itensPorPagina)
        {
            DreId = dreId;
            DisciplinaId = disciplinaId;
            AnoLetivo = anoLetivo;
            AnoEscolar = anoEscolar;
            UeId = ueId;
            TiposVariacao = tiposVariacao;
            NomeUe = nomeUe;
            Pagina = pagina;
            ItensPorPagina = itensPorPagina;
        }

        public int DreId { get; set; }
        public int DisciplinaId { get; set; }
        public int AnoLetivo { get; set; }
        public int AnoEscolar { get; set; }
        public int? UeId { get; set; }
        public List<int>? TiposVariacao { get; set; }
        public string? NomeUe { get; set; }
        public int? Pagina { get; set; }
        public int? ItensPorPagina { get; set; }
    }
}