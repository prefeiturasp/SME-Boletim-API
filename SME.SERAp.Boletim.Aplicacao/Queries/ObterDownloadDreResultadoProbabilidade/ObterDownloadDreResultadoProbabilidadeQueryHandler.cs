using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadDreResultadoProbabilidade
{
    public class ObterDownloadDreResultadoProbabilidadeQueryHandler : IRequestHandler<ObterDownloadDreResultadoProbabilidadeQuery, IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;
        public ObterDownloadDreResultadoProbabilidadeQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }
        public Task<IEnumerable<DownloadResultadoProbabilidadeDto>> Handle(ObterDownloadDreResultadoProbabilidadeQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterDownloadDreResultadoProbabilidade(request.LoteId, request.DreId);
        }
    }
}