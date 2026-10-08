using MediatR;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAnoLoteProva;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNivelProficienciaDisciplina;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProficienciaComparativoTodasTurmasAlunoSp
{
    public class ObterProficienciaComparativoTodasTurmasAlunoSpHandler : IRequestHandler<ObterProficienciaComparativoTodasTurmasAlunoSpQuery, ProficienciaComparativoAlunoSpDto>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        private readonly IMediator mediator;

        public ObterProficienciaComparativoTodasTurmasAlunoSpHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar, IMediator mediator)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
            this.mediator = mediator;
        }

        public async Task<ProficienciaComparativoAlunoSpDto> Handle(ObterProficienciaComparativoTodasTurmasAlunoSpQuery request, CancellationToken cancellationToken)
        {
            var anoLetivoLote = await mediator.Send(new ObterAnoLoteProvaQuery(request.LoteId));
            var anoLetivoPSP = anoLetivoLote - 1;

            var proficienciasAnoCorrente = (await repositorioBoletimEscolar.ObterProficienciaAlunoTodasTurmasProvaSaberesAsync(
                request.UeId, request.DisciplinaId, request.AnoEscolar, request.LoteId))?.ToList() ?? new List<AlunoProficienciaDto>();

            if (!proficienciasAnoCorrente.Any())
            {
                return new ProficienciaComparativoAlunoSpDto
                {
                    Total = 0,
                    Pagina = 1,
                    ItensPorPagina = 0,
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
                    variacao = ultimaProficiencia.CalcularPercentual(proficienciaAnterior.Proficiencia);
                }

                itensCompletos.Add(new ProficienciaAlunoDto
                {
                    Nome = proficienciasPsa.First().NomeAluno,
                    Raca = proficienciasPsa.First().Raca,
                    Sexo = proficienciasPsa.First().Sexo,
                    Turma = proficienciasPsa.First().Turma,
                    Variacao = variacao,
                    Proficiencias = proficiencias
                });
            }

            var itensFiltrados = itensCompletos.AsEnumerable();

            if (request.TiposVariacao != null && request.TiposVariacao.Any())
            {
                itensFiltrados = itensFiltrados.Where(x =>
                    request.TiposVariacao.Contains(1) && x.Variacao > 0 ||
                    request.TiposVariacao.Contains(2) && x.Variacao < 0 ||
                    request.TiposVariacao.Contains(3) && x.Variacao == 0
                );
            }

            if (!string.IsNullOrEmpty(request.NomeAluno))
            {
                itensFiltrados = itensFiltrados.Where(x => x.Nome.ToLower().Contains(request.NomeAluno.ToLower()));
            }

            var itensOrdenados = itensFiltrados.OrderBy(x => x.Turma).ThenBy(x => x.Nome).ToList();

            var listaAplicacoes = proficienciasAnoCorrente
                .Select(x => x.Periodo)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return new ProficienciaComparativoAlunoSpDto
            {
                Total = itensOrdenados.Count,
                Pagina = 1,
                ItensPorPagina = itensOrdenados.Count,
                Aplicacoes = listaAplicacoes,
                Itens = itensOrdenados,
                NomeDisciplina = proficienciasAnoCorrente.FirstOrDefault()?.DisciplinaNome ?? string.Empty,
                NomeLote = proficienciasAnoCorrente.FirstOrDefault()?.NomeLote ?? string.Empty,
                NomeAplicacaoPSP = proficienciasAnoAnterior.FirstOrDefault()?.NomeAplicacao ?? string.Empty
            };
        }
    }
}
