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
            var resumoDres = await repositorio.ObterResumoDreAsync(request.AnoEscolar, request.LoteId);

            if (resumoDres == null || !resumoDres.Any())
            {
                return new ProficienciaDreCompletoDto
                {
                    TotalTipoDisciplina = 0,
                    Itens = Enumerable.Empty<DreProficienciaDto>()
                };
            }

            var mediaProficiencias = await repositorio.ObterMediaProficienciaDreAsync(request.AnoEscolar, request.LoteId);
            var niveisProficiencia = await repositorio.ObterNiveisProficienciaAsync(request.AnoEscolar);

            if (request.DreIds != null && request.DreIds.Any())
            {
                resumoDres = resumoDres.Where(d => request.DreIds.Contains(d.DreId)).ToList();
                mediaProficiencias = mediaProficiencias.Where(d => request.DreIds.Contains(d.DreId)).ToList();
            }

            if (!resumoDres.Any())
            {
                return new ProficienciaDreCompletoDto
                {
                    TotalTipoDisciplina = 0,
                    Itens = Enumerable.Empty<DreProficienciaDto>()
                };
            }

            var resultadoCompleto = resumoDres.GroupJoin(
                mediaProficiencias,
                resumo => resumo.DreId,
                media => media.DreId,
                (resumo, medias) => new
                {
                    Resumo = resumo,
                    Medias = medias
                }
            ).ToList();

            var resultadoAgrupado = resultadoCompleto.Select(dreData =>
            {
                var disciplinasDetalhe = dreData.Medias.Select(media =>
                {
                    var nivelProficiencia = ObterNivelProficiencia(media.MediaProficiencia, media.DisciplinaId, niveisProficiencia);

                    return new DisciplinaProficienciaDetalheDto
                    {
                        Disciplina = media.Disciplina,
                        MediaProficiencia = media.MediaProficiencia,
                        NivelProficiencia = nivelProficiencia
                    };
                }).ToList();

                var percentualParticipacao = dreData.Resumo.TotalAlunos > 0
                    ? (decimal)dreData.Resumo.TotalRealizaramProva / dreData.Resumo.TotalAlunos * 100
                    : 0;

                return new DreProficienciaDto
                {
                    DreId = dreData.Resumo.DreId,
                    DreNome = dreData.Resumo.DreNome.ObterNomeDreAbreviado(),
                    AnoEscolar = dreData.Resumo.AnoEscolar,
                    TotalUes = dreData.Resumo.TotalUes,
                    TotalAlunos = dreData.Resumo.TotalAlunos,
                    TotalRealizaramProva = dreData.Resumo.TotalRealizaramProva,
                    PercentualParticipacao = decimal.Round(percentualParticipacao, 2),
                    Disciplinas = disciplinasDetalhe
                };
            }).ToList();

            var totalDisciplinas = resultadoAgrupado.Any() && resultadoAgrupado.First().Disciplinas.Any()
                ? resultadoAgrupado.First().Disciplinas.Count()
                : 0;

            return new ProficienciaDreCompletoDto
            {
                TotalTipoDisciplina = totalDisciplinas,
                Itens = resultadoAgrupado.OrderBy(x => x.DreNome).ToList()
            };
        }

        private string ObterNivelProficiencia(decimal media, long disciplinaId, IEnumerable<DreNivelProficienciaDto> niveis)
        {
            var niveisDaDisciplina = niveis.Where(n => n.DisciplinaId == disciplinaId).OrderBy(n => n.ValorReferencia).ToList();

            foreach (var nivel in niveisDaDisciplina)
            {
                if (nivel.ValorReferencia.HasValue && media <= nivel.ValorReferencia.Value)
                {
                    return nivel.Descricao;
                }
            }

            return niveisDaDisciplina.FirstOrDefault(n => !n.ValorReferencia.HasValue)?.Descricao ?? "Nível não definido";
        }
    }
}