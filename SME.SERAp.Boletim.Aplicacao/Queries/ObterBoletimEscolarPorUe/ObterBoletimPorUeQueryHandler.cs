using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe
{
    public class ObterBoletimPorUeQueryHandler : IRequestHandler<ObterBoletimEscolarPorUeQuery, IEnumerable<BoletimEscolar>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        private readonly IRepositorioCache repositorioCache;

        public ObterBoletimPorUeQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar, IRepositorioCache repositorioCache)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar ?? throw new ArgumentNullException(nameof(repositorioBoletimEscolar));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<BoletimEscolar>> Handle(ObterBoletimEscolarPorUeQuery request, CancellationToken cancellationToken)
        {
            var chaveBoletimUe = string.Format(CacheChave.BoletimUe, request.UeId);

            return await repositorioCache.ObterRedisAsync(
                chaveBoletimUe,
                async () => await repositorioBoletimEscolar.ObterBoletinsPorUe(request.UeId)
            );

            //return await repositorioBoletimEscolar.ObterBoletinsPorUe(request.UeId);
        }
    }
}