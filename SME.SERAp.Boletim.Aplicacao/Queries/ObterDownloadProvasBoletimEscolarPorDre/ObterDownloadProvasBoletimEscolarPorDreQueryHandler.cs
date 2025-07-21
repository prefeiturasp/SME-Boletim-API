using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolarPorDre
{
    public class ObterDownloadProvasBoletimEscolarPorDreQueryHandler : IRequestHandler<ObterDownloadProvasBoletimEscolarPorDreQuery, IEnumerable<DownloadProvasBoletimEscolarPorDreDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterDownloadProvasBoletimEscolarPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<DownloadProvasBoletimEscolarPorDreDto>> Handle(ObterDownloadProvasBoletimEscolarPorDreQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterDownloadProvasBoletimEscolarPorDre(request.DreId, request.LoteId);
    }
}