using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterResultadoProbabilidadePorUeId
{
    public class ObterResultadoProbabilidadePorUeIdQuery : IRequest<(IEnumerable<ResultadoProbabilidadeDto>, int)>
    {
        public long UeId { get; }
        public long ProvaId { get; }
        public long DisciplinaId { get; }
        public int AnoEscolar { get; }
        public int Pagina { get; }
        public int TamanhoPagina { get; }
        public int QuantidadeRegistros { get; }

        public ObterResultadoProbabilidadePorUeIdQuery(long ueId, long disciplinaId, int anoEscolar, int pagina, int tamanhoPagina)
        {
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            Pagina = pagina;
            TamanhoPagina = tamanhoPagina;
        }
    }
}