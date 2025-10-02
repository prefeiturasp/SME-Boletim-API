using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoUe
{
    public class ObterProficienciaComparativoUeQueryHandler : IRequestHandler<ObterProficienciaComparativoUeQuery, ProficienciaComparativoUeDto>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        private readonly IMediator mediator;

        public ObterProficienciaComparativoUeQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar, IMediator mediator)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
            this.mediator = mediator;
        }

        public async Task<ProficienciaComparativoUeDto> Handle(ObterProficienciaComparativoUeQuery request, CancellationToken cancellationToken)
        {
            var anoLetivoPSP = request.AnoLetivo - 1;

            var proficienciasPsa = (await repositorioBoletimEscolar.ObterProficienciaUeProvaSaberesAsync(
                request.DreId, request.DisciplinaId, request.AnoLetivo, request.AnoEscolar, request.UeId))?.ToList() ?? new List<UeProficienciaQueryResultDto>();

            var proficienciasPsp = (await repositorioBoletimEscolar.ObterProficienciaUeProvaSPAsync(
                request.DreId, request.DisciplinaId, anoLetivoPSP, request.AnoEscolar - 1, request.UeId))?.ToList() ?? new List<UeProficienciaQueryResultDto>();

            if (!proficienciasPsa.Any() && !proficienciasPsp.Any())
            {
                return new ProficienciaComparativoUeDto
                {
                    Total = 0,
                    Pagina = request.Pagina.GetValueOrDefault(1),
                    ItensPorPagina = request.ItensPorPagina.GetValueOrDefault(0),
                    DreId = request.DreId,
                    DreAbreviacao = "",
                    Ues = new List<UeProficienciaDto>()
                };
            }

            var proficienciasPsaAgrupadas = proficienciasPsa.GroupBy(p => p.UeId).ToDictionary(g => g.Key, g => g.ToList());
            var proficienciasPspAgrupadas = proficienciasPsp.GroupBy(p => p.UeId).ToDictionary(g => g.Key, g => g.FirstOrDefault());

            var uesCompletas = new List<UeProficienciaDto>();
            var niveisProficiencia = (await repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(request.DisciplinaId, request.AnoEscolar))?.ToList();

            var todasUesIds = proficienciasPsaAgrupadas.Keys.Union(proficienciasPspAgrupadas.Keys).Distinct();

            foreach (var ueId in todasUesIds)
            {
                ProficienciaDetalheUeDto aplicacaoPsp = null;
                var aplicacoesPsa = new List<ProficienciaDetalheUeDto>();

                if (proficienciasPspAgrupadas.TryGetValue(ueId, out var pspData))
                {
                    var nivelPsp = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)pspData.MediaProficiencia, request.DisciplinaId, niveisProficiencia));
                    aplicacaoPsp = new ProficienciaDetalheUeDto
                    {
                        LoteId = 0,
                        NomeAplicacao = pspData.NomeAplicacao,
                        Periodo = pspData.Periodo,
                        MediaProficiencia = pspData.MediaProficiencia,
                        RealizaramProva = pspData.RealizaramProva,
                        NivelProficiencia = nivelPsp
                    };
                }

                if (proficienciasPsaAgrupadas.TryGetValue(ueId, out var psaDataList))
                {
                    var psaDetalhes = await Task.WhenAll(psaDataList.Select(async psa =>
                    {
                        var nivelPsa = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery((decimal)psa.MediaProficiencia, request.DisciplinaId, niveisProficiencia));
                        return new ProficienciaDetalheUeDto
                        {
                            LoteId = psa.LoteId,
                            NomeAplicacao = psa.NomeAplicacao,
                            Periodo = psa.Periodo,
                            MediaProficiencia = psa.MediaProficiencia,
                            RealizaramProva = psa.RealizaramProva,
                            NivelProficiencia = nivelPsa
                        };
                    }));
                    aplicacoesPsa.AddRange(psaDetalhes.OrderBy(p => p.Periodo));
                }

                var variacao = 0.0;
                var proficienciaFinal = aplicacoesPsa.Any() ? aplicacoesPsa.Last().MediaProficiencia : 0;
                var proficienciaInicial = aplicacaoPsp?.MediaProficiencia;

                if (proficienciaInicial.HasValue && proficienciaFinal > 0)
                {
                    variacao = (double)BoletimExtensions.CalcularPercentual((decimal)proficienciaFinal, (decimal)proficienciaInicial.Value);
                }

                var dadosUe = proficienciasPsaAgrupadas.ContainsKey(ueId) ? proficienciasPsaAgrupadas[ueId].First() : proficienciasPspAgrupadas[ueId];

                uesCompletas.Add(new UeProficienciaDto
                {
                    UeId = dadosUe.UeId,
                    UeNome = BoletimExtensions.ObterUeDescricao(dadosUe.UeNome, dadosUe.TipoEscola, dadosUe.DreNome, dadosUe.DreAbreviacao),
                    Disciplinaid = dadosUe.DisciplinaId,
                    Variacao = variacao,
                    AplicacaoPsp = aplicacaoPsp,
                    AplicacoesPsa = aplicacoesPsa
                });
            }

            var itensFiltrados = uesCompletas.AsEnumerable();

            if (request.TiposVariacao != null && request.TiposVariacao.Any())
            {
                itensFiltrados = itensFiltrados.Where(x =>
                    request.TiposVariacao.Contains(1) && x.Variacao > 0 ||
                    request.TiposVariacao.Contains(2) && x.Variacao < 0 ||
                    request.TiposVariacao.Contains(3) && x.Variacao == 0
                );
            }

            if (!string.IsNullOrEmpty(request.NomeUe))
            {
                itensFiltrados = itensFiltrados.Where(x => x.UeNome.ToLower().Contains(request.NomeUe.ToLower()));
            }

            var itensOrdenados = itensFiltrados.OrderBy(x => x.UeNome).ToList();

            var itensPaginados = new List<UeProficienciaDto>();
            if (request.Pagina.HasValue && request.ItensPorPagina.HasValue)
            {
                var skip = (request.Pagina.Value - 1) * request.ItensPorPagina.Value;
                itensPaginados = itensOrdenados.Skip(skip).Take(request.ItensPorPagina.Value).ToList();
            }
            else
            {
                itensPaginados = itensOrdenados;
            }

            var dreAbreviacao = proficienciasPsa.FirstOrDefault()?.DreAbreviacao ?? proficienciasPsp.FirstOrDefault()?.DreAbreviacao ?? "";

            return new ProficienciaComparativoUeDto
            {
                Total = itensOrdenados.Count,
                Pagina = request.Pagina.GetValueOrDefault(1),
                ItensPorPagina = request.ItensPorPagina.GetValueOrDefault(itensOrdenados.Count),
                DreId = request.DreId,
                DreAbreviacao = dreAbreviacao,
                Ues = itensPaginados
            };
        }
    }
}