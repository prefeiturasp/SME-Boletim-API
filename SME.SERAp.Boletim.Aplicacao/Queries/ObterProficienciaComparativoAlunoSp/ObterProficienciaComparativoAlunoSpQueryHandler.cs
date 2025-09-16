using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoAlunoSp
{
    public class ObterProficienciaComparativoAlunoSpHandler : IRequestHandler<ObterProficienciaComparativoAlunoSpQuery, ProficienciaComparativoAlunoSpDto>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        private readonly IMediator mediator;

        public ObterProficienciaComparativoAlunoSpHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar, IMediator mediator)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
            this.mediator = mediator;
        }

        public async Task<ProficienciaComparativoAlunoSpDto> Handle(ObterProficienciaComparativoAlunoSpQuery request, CancellationToken cancellationToken)
        {
            var anoLetivoLote = await mediator.Send(new ObterAnoLoteProvaQuery(request.LoteId));
            var anoLetivoPSP = anoLetivoLote - 1;

            var proficienciasAnoCorrente = (await repositorioBoletimEscolar.ObterProficienciaAlunoProvaSaberesAsync(
              request.UeId, request.DisciplinaId, request.AnoEscolar, request.Turma, request.LoteId))?.ToList() ?? new List<AlunoProficienciaDto>();

            if (!proficienciasAnoCorrente.Any())
            {
                return new ProficienciaComparativoAlunoSpDto
                {
                    Total = 0,
                    Pagina = request.Pagina.GetValueOrDefault(1),
                    ItensPorPagina = request.ItensPorPagina.GetValueOrDefault(0),
                    Aplicacoes = new List<string>(),
                    Itens = new List<ProficienciaAlunoDto>()
                };
            }

            var alunosRa = proficienciasAnoCorrente.Select(x => x.AlunoRa).Distinct().ToList();

            var proficienciasAnoAnterior = (await repositorioBoletimEscolar.ObterProficienciaAlunoProvaSPAsync(
              request.DisciplinaId, anoLetivoPSP, alunosRa))?.ToList() ?? new List<AlunoProficienciaDto>();

            var proficienciasAnteriorDict = proficienciasAnoAnterior.ToDictionary(p => p.AlunoRa);

            var itensCompletos = new List<ProficienciaAlunoDto>();
            var proficienciasCorrenteAgrupadas = proficienciasAnoCorrente.GroupBy(p => p.AlunoRa);
            var niveisProficiencia = await repositorioBoletimEscolar.ObterNiveisProficienciaPorDisciplinaIdAsync(request.DisciplinaId, request.AnoEscolar);

            foreach (var grupoAluno in proficienciasCorrenteAgrupadas)
            {
                var alunoRa = grupoAluno.Key;
                var proficienciasPsa = grupoAluno.ToList();

                var proficiencias = new List<ProficienciaDetalheDto>();
                var proficienciaAnterior = proficienciasAnteriorDict.ContainsKey(alunoRa) ? proficienciasAnteriorDict[alunoRa] : null;

                if (proficienciaAnterior != null)
                {
                    var nivelProficienciaAnterior = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery(proficienciaAnterior.Proficiencia, request.DisciplinaId, niveisProficiencia));
                    proficiencias.Add(new ProficienciaDetalheDto
                    {
                        Descricao = proficienciaAnterior.NomeAplicacao,
                        Mes = "",
                        Valor = proficienciaAnterior.Proficiencia,
                        NivelProficiencia = nivelProficienciaAnterior
                    });
                }

                var proficienciasCorrenteDetalhes = await Task.WhenAll(proficienciasPsa.Select(async p => new ProficienciaDetalheDto
                {
                    Descricao = p.NomeAplicacao,
                    Mes = p.Periodo,
                    Valor = p.Proficiencia,
                    NivelProficiencia = await mediator.Send(new ObterNivelProficienciaDisciplinaQuery(p.Proficiencia, request.DisciplinaId, niveisProficiencia))
                }));

                proficiencias.AddRange(proficienciasCorrenteDetalhes.OrderBy(p => p.Mes));

                var variacao = 0.0;
                if (proficienciaAnterior != null && proficienciasPsa.Any())
                {
                    var ultimaProficiencia = proficienciasPsa.OrderByDescending(p => p.Periodo).First().Proficiencia;
                    variacao = (double)Math.Round(ultimaProficiencia - proficienciaAnterior.Proficiencia, 2);
                }

                itensCompletos.Add(new ProficienciaAlunoDto
                {
                    Nome = proficienciasPsa.First().NomeAluno,
                    Variacao = variacao,
                    Proficiencias = proficiencias
                });
            }

            var itensFiltrados = itensCompletos.AsEnumerable();
            if (request.TipoVariacao.HasValue)
            {
                if (request.TipoVariacao.Value == 1)
                {
                    itensFiltrados = itensFiltrados.Where(x => x.Variacao > 0);
                }
                else if (request.TipoVariacao.Value == 2)
                {
                    itensFiltrados = itensFiltrados.Where(x => x.Variacao < 0);
                }
            }

            if (!string.IsNullOrEmpty(request.NomeAluno))
            {
                itensFiltrados = itensFiltrados.Where(x => x.Nome.ToLower().Contains(request.NomeAluno.ToLower()));
            }

            var itensOrdenados = itensFiltrados.OrderBy(x => x.Nome).ToList();
            var itensPaginados = new List<ProficienciaAlunoDto>();

            if (request.Pagina.HasValue && request.ItensPorPagina.HasValue)
            {
                var skip = (request.Pagina.Value - 1) * request.ItensPorPagina.Value;
                itensPaginados = itensOrdenados.Skip(skip).Take(request.ItensPorPagina.Value).ToList();
            }
            else
            {
                itensPaginados = itensOrdenados;
            }

            var listaAplicacoes = proficienciasAnoCorrente
              .Select(x => x.Periodo)
              .Distinct()
              .OrderBy(x => x)
              .ToList();

            return new ProficienciaComparativoAlunoSpDto
            {
                Total = itensOrdenados.Count,
                Pagina = request.Pagina.GetValueOrDefault(1),
                ItensPorPagina = request.ItensPorPagina.GetValueOrDefault(itensOrdenados.Count),
                Aplicacoes = listaAplicacoes,
                Itens = itensPaginados
            };
        }
    }
}