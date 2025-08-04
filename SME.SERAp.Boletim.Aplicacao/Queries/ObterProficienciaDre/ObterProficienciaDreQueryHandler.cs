using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaDre
{
    public class ObterProficienciaDreQueryHandler : IRequestHandler<ObterProficienciaDreQuery, ProficienciaDreCompletoDto>
    {
        private readonly IRepositorioBoletimEscolar repositorio;

        public ObterProficienciaDreQueryHandler(IRepositorioBoletimEscolar repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<ProficienciaDreCompletoDto> Handle(ObterProficienciaDreQuery request, CancellationToken cancellationToken)
        {
            var resultadoPlano = await repositorio.ObterProficienciaDreAsync(request.AnoEscolar, request.LoteId);

            if (resultadoPlano == null || !resultadoPlano.Any())
            {
                return new ProficienciaDreCompletoDto
                {
                    TotalTipoDisciplina = 0,
                    Itens = Enumerable.Empty<DreProficienciaDto>()
                };
            }

            if (request.DreIds != null && request.DreIds.Any())
            {
                resultadoPlano = resultadoPlano.Where(d => request.DreIds.Contains(d.DreId)).ToList();
            }

            if (!resultadoPlano.Any())
            {
                return new ProficienciaDreCompletoDto
                {
                    TotalTipoDisciplina = 0,
                    Itens = Enumerable.Empty<DreProficienciaDto>()
                };
            }

            var resultadoAgrupado = resultadoPlano.GroupBy(d => d.DreId)
                                                  .Select(grupo => new DreProficienciaDto
                                                  {
                                                      DreId = grupo.Key,
                                                      DreNome = grupo.First().DreNome.ObterNomeDreAbreviado(),
                                                      AnoEscolar = grupo.First().AnoEscolar,
                                                      TotalUes = grupo.First().TotalUes,
                                                      TotalAlunos = grupo.First().TotalAlunos,
                                                      TotalRealizaramProva = grupo.First().TotalRealizaramProva,
                                                      PercentualParticipacao = grupo.First().PercentualParticipacao,
                                                      Disciplinas = grupo.Select(item => new DisciplinaProficienciaDetalheDto
                                                      {
                                                          Disciplina = item.Disciplina,
                                                          MediaProficiencia = item.MediaProficiencia,
                                                          NivelProficiencia = item.NivelProficiencia
                                                      }).ToList()
                                                  }).ToList();

            var totalDisciplinas = resultadoAgrupado.Any() ? resultadoAgrupado.First().Disciplinas.Count() : 0;

            return new ProficienciaDreCompletoDto
            {
                TotalTipoDisciplina = totalDisciplinas,
                Itens = resultadoAgrupado
            };
        }
    }
}