using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAbaEstudanteBoletimEscolarPorUeId
{
    class ObterAbaEstudanteBoletimEscolarPorUeIdQuery
    : IRequest<(IEnumerable<AbaEstudanteListaDto> estudantes, int totalRegistros)>
    {
        public ObterAbaEstudanteBoletimEscolarPorUeIdQuery(string ueId, int pagina, int tamanhoPagina)
        {
            UeId = ueId;
            Pagina = pagina;
            TamanhoPagina = tamanhoPagina;
        }

        public string UeId { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}