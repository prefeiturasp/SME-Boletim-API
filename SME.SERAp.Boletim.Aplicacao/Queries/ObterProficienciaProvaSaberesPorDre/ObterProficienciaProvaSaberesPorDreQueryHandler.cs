using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;


namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaProvaSaberesPorDre
{
    public class ObterProficienciaProvaSaberesPorDreQueryHandler : IRequestHandler<ObterProficienciaProvaSaberesPorDreQuery, IEnumerable<ResultadoProeficienciaPorDre>>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterProficienciaProvaSaberesPorDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IEnumerable<ResultadoProeficienciaPorDre>> Handle(ObterProficienciaProvaSaberesPorDreQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterProficienciaDreProvaSaberesAsync(request.DreId, request.AnoLetivo, request.DisciplinaId, request.AnoEscolar);
    }
}

