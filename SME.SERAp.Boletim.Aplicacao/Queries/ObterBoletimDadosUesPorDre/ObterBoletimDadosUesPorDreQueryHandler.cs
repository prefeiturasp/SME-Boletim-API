using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimDadosUesPorDre
{
    public class ObterBoletimDadosUesPorDreQueryHandler : IRequestHandler<ObterBoletimDadosUesPorDreQuery, PaginacaoUesBoletimDadosDto>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterBoletimDadosUesPorDreQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public async Task<PaginacaoUesBoletimDadosDto> Handle(ObterBoletimDadosUesPorDreQuery request, CancellationToken cancellationToken)
        {
            var uesDados = await repositorioBoletimEscolar.ObterUesPorDre(request.LoteId, request.DreId, request.AnoEscolar, request.Filtros);
            if(uesDados?.Itens?.Any() ?? false)
            {
                var uesIds = uesDados.Itens.Select(x => x.Id);
                var uesTotalAlunos = await repositorioBoletimEscolar.ObterTotalAlunosPorUes(request.LoteId, request.DreId, request.AnoEscolar, uesIds);
                var uesTotalAlunosRealizaramProva = await repositorioBoletimEscolar.ObterTotalAlunosRealizaramProvaPorUes(request.LoteId, request.DreId, request.AnoEscolar, uesIds);
                var uesDisciplinasProficiencia = await repositorioBoletimEscolar.ObterDiciplinaMediaProficienciaProvaPorUes(request.LoteId, request.DreId, request.AnoEscolar, uesIds);

                foreach(var ue in uesDados.Itens)
                {
                    ue.TotalEstudantes = uesTotalAlunos?.FirstOrDefault(x => x.UeId == ue.Id)?.TotalAlunos ?? 0;
                    ue.TotalEstudadesRealizaramProva = uesTotalAlunosRealizaramProva?.FirstOrDefault(x => x.UeId == ue.Id)?.TotalAlunos ?? 0;
                    ue.Disciplinas = uesDisciplinasProficiencia?.Where(x => x.UeId == ue.Id) ?? new List<UeBoletimDisciplinaProficienciaDto>();
                }
            }

            return uesDados;
        }
    }
}
