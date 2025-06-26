using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadResultadoProbabilidade
{
    public class ObterDownloadResultadoProbabilidadeQueryHandler : IRequestHandler<ObterDownloadResultadoProbabilidadeQuery, IEnumerable<DownloadResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimEscolar _repositorio;

        public ObterDownloadResultadoProbabilidadeQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<DownloadResultadoProbabilidadeDto>> Handle(ObterDownloadResultadoProbabilidadeQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.ObterDownloadResultadoProbabilidade(request.LoteId, request.UeId, request.DisciplinaId, request.AnoEscolar);
        }
    }
}