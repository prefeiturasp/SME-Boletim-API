using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

namespace SME.SERAp.Boletim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoletimEscolarController : ControllerBase
    {
        [HttpGet("{loteId}/{ueId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarDto), 200)]
        public async Task<IActionResult> ObterBoletimPorUe(long ueId, long loteId,
            [FromServices] IObterBoletimEscolarPorUeUseCase obterBoletimEscolarPorUeUseCase, [FromQuery] FiltroBoletimDto filtros)
        {
            return Ok(await obterBoletimEscolarPorUeUseCase.Executar(loteId, ueId, filtros));
        }

        [HttpGet("{loteId}/{ueId}/turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarPorTurmaDto), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(long ueId, long loteId, [FromQuery] FiltroBoletimDto filtros,
            [FromServices] IObterBoletimEscolarTurmaPorUeUseCase obterBoletimEscolarTurmaPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarTurmaPorUeUseCase.Executar(loteId, ueId, filtros));
        }

        [HttpGet("download/{loteId}/{ueId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(long ueId, long loteId,
          [FromServices] IObterDownloadBoletimProvaEscolarUseCase obterDownloadBoletimProvaEscolarUseCase)
        {
            var file = await obterDownloadBoletimProvaEscolarUseCase.Executar(loteId, ueId);

            return File(file, "application/vnd.ms-excel", "relatorio.xls", enableRangeProcessing: true);
        }

        [HttpGet("{loteId}/{ueId}/estudantes")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarComDisciplinasDto), 200)]
        public async Task<IActionResult> ObterAbaEstudanteBoletimEscolarPorUeId(long loteId, long ueId, [FromQuery] FiltroBoletimEstudantePaginadoDto filtros,
            [FromServices] IObterAbaEstudanteBoletimEscolarPorUeIdUseCase obterAbaEstudanteBoletimEscolarPorUeIdUseCase)
        {
            var resultado = await obterAbaEstudanteBoletimEscolarPorUeIdUseCase.Executar(loteId, ueId, filtros);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/{ueId}/filtros")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarOpcoesFiltrosDto), 200)]
        public async Task<IActionResult> ObterOpcoesFiltrosBoletimEscolarPorUe(long ueId, long loteId,
            [FromServices] IObterBoletimEscolarOpcoesFiltrosPorUeUseCase obterBoletimEscolarOpcoesFiltrosPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarOpcoesFiltrosPorUeUseCase.Executar(loteId, ueId));
        }

        [HttpGet("{loteId}/{ueId}/estudantes-grafico")]
        [ProducesResponseType(typeof(IEnumerable<AbaEstudanteGraficoDto>), 200)]
        public async Task<IActionResult> ObterAbaEstudanteGraficoPorUeId(long loteId, long ueId, [FromQuery] FiltroBoletimEstudanteDto filtros,
            [FromServices] IObterAbaEstudanteGraficoPorUeIdUseCase obterAbaEstudanteGraficoPorUeIdUseCase)
        {
            var resultado = await obterAbaEstudanteGraficoPorUeIdUseCase.Executar(loteId, ueId, filtros);
            return Ok(resultado);
        }

        [HttpGet("aplicacoes-prova")]
        [ProducesResponseType(typeof(IEnumerable<LoteProvaDto>), 200)]
        public async Task<IActionResult> ObterBoletimAplicacoesProva([FromServices] IObterBoletimAplicacoesProvaUseCase obterAplicacoesProva)
        {
            var resultado = await obterAplicacoesProva.Executar();
            return Ok(resultado);
        }

        [HttpGet("{loteId}/{ueId}/{disciplinaId}/{anoEscolar}/resultado-probabilidade")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ResultadoProbabilidadeAgrupadoDto>), 200)]
        public async Task<IActionResult> ObterResultadoProbabilidadePorUeAsync(long loteId, long ueId, long disciplinaId, int anoEscolar,
        [FromQuery] FiltroBoletimResultadoProbabilidadeDto filtros,
        [FromServices] IObterResultadoProbabilidadePorUeUseCase obterResultadoProbabilidadePorUeUseCase)
        {
            if (filtros.Pagina < 1 || filtros.TamanhoPagina < 1)
                return BadRequest("Página e tamanho da página devem ser maiores que zero.");

            var resultado = await obterResultadoProbabilidadePorUeUseCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);
            return Ok(resultado);
        }

        [HttpGet("download-probabilidade/{loteId}/{ueId}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterDownloadResultadoProbabilidade(long loteId, long ueId, long disciplinaId, int anoEscolar,
        [FromServices] IObterDownloadResultadoProbabilidadeUseCase obterDownloadResultadoProbabilidadeUseCase)
        {
            var file = await obterDownloadResultadoProbabilidadeUseCase.Executar(loteId, ueId, disciplinaId, anoEscolar);
            return File(file, "application/vnd.ms-excel", $"resultado_probabilidade_{DateTime.Now:dd-MM-yyyy}.xls", enableRangeProcessing: true);
        }

        [HttpGet("{loteId}/{ueId}/{disciplinaId}/{anoEscolar}/resultado-probabilidade/lista")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ResultadoProbabilidadeListaPaginadoDto>), 200)]
        public async Task<IActionResult> ObterResultadoProbabilidadePorUeListaAsync(long loteId, long ueId, long disciplinaId, int anoEscolar,
            [FromQuery] FiltroBoletimResultadoProbabilidadeDto filtros,
            [FromServices] IObterResultadoProbabilidadePorUeListaUseCase obterResultadoProbabilidadePorUeListaUseCase)
        {
            if (filtros.Pagina < 1 || filtros.TamanhoPagina < 1)
                return BadRequest("Página e tamanho da página devem ser maiores que zero.");

            var resultado = await obterResultadoProbabilidadePorUeListaUseCase.Executar(loteId, ueId, disciplinaId, anoEscolar, filtros);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/anos-escolares")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<AnoEscolarDto>), 200)]
        public async Task<IActionResult> ObterAnosEscolares(long loteId,
            [FromServices] IObterAnosEscolaresPorLoteIdUseCase obterAnosEscolaresPorLoteIdUseCase)
        {
            var resultado = await obterAnosEscolaresPorLoteIdUseCase.Executar(loteId);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/{dreId}/{anoEscolar}/resumo-dre")]
        [ProducesResponseType(typeof(BoletimEscolarResumoDreDto), 200)]
        public async Task<IActionResult> ObterResumoDreBoletimEscolar(long loteId, long dreId, int anoEscolar,
        [FromServices] IObterBoletimEscolarResumoDreUseCase useCase)
        {
            var resultado = await useCase.Executar(loteId, dreId, anoEscolar);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/dre/{dreId}/ano-escolar/{anoEscolar}/grafico/niveis-proficiencia-disciplina")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(DreResumoUesNivelProficienciaDto), 200)]
        public async Task<IActionResult> ObterDreNiveisProficienciaDisciplinasUes(long loteId, long dreId, int anoEscolar,
            [FromServices] IObterUesPorNivelProficienciaDisciplinaPorDreUseCase obterUesPorNivelProficienciaDisciplinaPorDreUseCase)
        {
            var resultado = await obterUesPorNivelProficienciaDisciplinaPorDreUseCase.Executar(loteId, dreId, anoEscolar);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/{dreId}/{anoEscolar}/ues-por-dre")]
        [ProducesResponseType(typeof(IEnumerable<UePorDreDto>), 200)]
        public async Task<IActionResult> ObterUesPorDre(long loteId, long dreId, int anoEscolar,
        [FromServices] IObterUesPorDreUseCase useCase)
        {
            var resultado = await useCase.Executar(dreId, anoEscolar, loteId);
            return Ok(resultado);
        }

        [HttpGet("download-dre/{loteId}/{dreId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorDre(long dreId, long loteId,
        [FromServices] IObterDownloadBoletimProvaEscolarPorDreUseCase useCase)
        {
            var file = await useCase.Executar(loteId, dreId);
            return File(file, "application/vnd.ms-excel", "relatorio-dre.xls", enableRangeProcessing: true);
        }

        [HttpGet("{loteId}/{dreId}/{anoEscolar}/ue-por-dre-dados")]
        [ProducesResponseType(typeof(IEnumerable<PaginacaoUesBoletimDadosDto>), 200)]
        public async Task<IActionResult> ObterBoletimDadosUesPorDre(long loteId, long dreId, int anoEscolar, [FromQuery] FiltroUeBoletimDadosDto filtros,
        [FromServices] IObterBoletimDadosUesPorDreUseCase useCase)
        {
            var resultado = await useCase.Executar(loteId, dreId, anoEscolar, filtros);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/{anoEscolar}/resumo-sme")]
        [ProducesResponseType(typeof(BoletimEscolarResumoSmeDto), 200)]
        public async Task<IActionResult> ObterResumoSmeBoletimEscolar(long loteId, int anoEscolar,
        [FromServices] IObterBoletimEscolarResumoSmeUseCase useCase)
        {
            var resultado = await useCase.Executar(loteId, anoEscolar);
            return Ok(resultado);
        }

        [HttpGet("{anoEscolar}/{loteId}/grafico/niveis-proficiencia-disciplina")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ResumoDresNivelProficienciaDto), 200)]
        public async Task<IActionResult> ObterNiveisProficienciaDisciplinasUes(int anoEscolar,  long loteId,
            [FromServices] IObterDresPorNivelProficienciaDisciplinaUseCase obterUesPorNivelProficienciaDisciplinaUseCase)
        {
            var resultado = await obterUesPorNivelProficienciaDisciplinaUseCase.Executar(anoEscolar, loteId);
            return Ok(resultado);
        }

        [HttpGet("{loteId}/ano-escolar/{anoEscolar}/grafico/media-proficiencia")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(DreResumoUesNivelProficienciaDto), 200)]
        public async Task<IActionResult> ObterBoletimEscolarDresMediaProficiencia(long loteId, int anoEscolar, [FromQuery]IEnumerable<long> dresIds,
            [FromServices] IObterBoletimEscolarDresMediaProficienciaUseCase obterBoletimEscolarDresMediaProficienciaUseCase)
        {
            var resultado = await obterBoletimEscolarDresMediaProficienciaUseCase.Executar(loteId, anoEscolar, dresIds);
            return Ok(resultado);
        }

        [HttpGet("download-sme/{loteId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(File), 200)]
        public async Task<IActionResult> ObterBoletimEscolarSmeDownload(long loteId,
            [FromServices] IObterDownloadBoletimProvaEscolarSmeUseCase useCase)
        {
            var file = await useCase.Executar(loteId);
            return File(file, "application/vnd.ms-excel", "relatorio-sme.xls", enableRangeProcessing: true);
        }

        [HttpGet("download-sme-probabilidade/{loteId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(File), 200)]
        public async Task<IActionResult> ObterDownloadSmeResultadoProbabilidade(long loteId,
        [FromServices] IObterDownloadSmeResultadoProbabilidadeUseCase useCase)
        {
            var file = await useCase.Executar(loteId);
            return File(file, "application/vnd.ms-excel", $"resultado-sme-probabilidade-{DateTime.Now:dd-MM-yyyy}.xls", enableRangeProcessing: true);
        }

        [HttpGet("{anoEscolar}/{loteId}/dres")]
        [ProducesResponseType(typeof(IEnumerable<DreDto>), 200)]
        public async Task<IActionResult> ObterDres(int anoEscolar, long loteId,
        [FromServices] IObterDresUseCase useCase)
        {
            var resultado = await useCase.Executar(anoEscolar, loteId);
            return Ok(resultado);
        }

        [HttpGet("{anoEscolar}/{loteId}/dres/proficiencia")]
        [ProducesResponseType(typeof(ProficienciaDreCompletoDto), 200)]
        public async Task<IActionResult> ObterProficienciaDre(
            int anoEscolar,
            long loteId,
            [FromServices] IObterProficienciaDreUseCase useCase,
            [FromQuery] IEnumerable<long>? dreIds = null)
        {
            var resultado = await useCase.Executar(anoEscolar, loteId, dreIds);

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpGet("download-dre-probabilidade/{loteId}/{dreId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(File), 200)]
        public async Task<IActionResult> ObterDownloadDreResultadoProbabilidade(long loteId, int dreId,
        [FromServices] IObterDownloadDreResultadoProbabilidadeUseCase useCase)
        {
            var file = await useCase.Executar(loteId, dreId);
            return File(file, "application/vnd.ms-excel", $"resultado-dre-probabilidade-{DateTime.Now:dd-MM-yyyy}.xls", enableRangeProcessing: true);
        }

        [HttpGet("proficienciaComparativoProvaSp/{loteId}/{ueId}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(ProficienciaUeComparacaoProvaSPDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObterProficienciaComparativoProvaSP(
            long loteId,
            int ueId,
            int disciplinaId,
            int anoEscolar,
            [FromServices] IObterProficienciaComparativoProvaSPUseCase useCase)
        {
            var resultado = await useCase.Executar(loteId, ueId, disciplinaId, anoEscolar);

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpGet("{loteId}/turmas-ue-ano/{ueId}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<TurmaAnoDto>), 200)]
        public async Task<IActionResult> ObterTurmasUeAno(long loteId, long ueId, int disciplinaId, int anoEscolar,
            [FromServices] IObterTurmasUeAnoUseCase obterTurmasUeAnoUseCase)
        {
            return Ok(await obterTurmasUeAnoUseCase.Executar(loteId, ueId, disciplinaId, anoEscolar));
        }

        [HttpGet("comparativo-aluno-ue/{ueId}/{disciplinaId}/{anoEscolar}/{turma}/{loteId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ProficienciaComparativoAlunoSpDto), 200)]
        public async Task<IActionResult> ObterProficienciaComparativoAlunoSp(int ueId, int disciplinaId, int anoEscolar, string turma, long loteId,
        [FromQuery] List<int>? tiposVariacao,
        [FromQuery] string? nomeAluno,
        [FromQuery] int? pagina, [FromQuery] int? itensPorPagina,
        [FromServices] IObterProficienciaComparativoAlunoSpUseCase obterProficienciaComparativoAlunoSpUseCase)
        {
            var result = await obterProficienciaComparativoAlunoSpUseCase.Executar(ueId, disciplinaId, anoEscolar, turma, loteId, tiposVariacao, nomeAluno, pagina, itensPorPagina);
            return Ok(result);
        }

        [HttpGet("anos-aplicacao-dre/{dreId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<int>), 200)]
        public async Task<IActionResult> ObterAnosAplicacaoPorDre(long dreId, [FromServices] IObterAnosAplicacaoPorDreUseCase obterAnosAplicacaoPorDreUseCase)
        {
            return Ok(await obterAnosAplicacaoPorDreUseCase.Executar(dreId));
        }

        [HttpGet("componentes-curriculares-dre/{dreId}/{anoAplicacao}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoFiltroDto<int>>), 200)]
        public async Task<IActionResult> ObterComponentesCurricularesPorDreAnoUseCase(long dreId, int anoAplicacao, [FromServices] IObterComponentesCurricularesPorDreAnoUseCase obterComponentesCurricularesPorDreAnoUseCase)
        {
            return Ok(await obterComponentesCurricularesPorDreAnoUseCase.Executar(dreId, anoAplicacao));
        }

        [HttpGet("anos-escolares-dre/{dreId}/{anoAplicacao}/{disciplinaId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoFiltroDto<int>>), 200)]
        public async Task<IActionResult> ObterAnosEscolaresPorDreAnoAplicacao(long dreId, int anoAplicacao, int disciplinaId, [FromServices] IObterAnosEscolaresPorDreAnoAplicacaoUseCase obterAnosEscolaresPorDreAnoAplicacaoUseCase)
        {
            return Ok(await obterAnosEscolaresPorDreAnoAplicacaoUseCase.Executar(dreId, anoAplicacao, disciplinaId));
        }

        [HttpGet("ues-comparacao-por-dre/{dreId}/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(IEnumerable<UePorDreDto>), 200)]
        public async Task<IActionResult> ObterUesComparacaoPorDre(long dreId, int anoAplicacao, int disciplinaId, int anoEscolar,
            [FromServices] IObterUesComparacaoPorDreUseCase useCase)
        {
            var resultado = await useCase.Executar(dreId, anoAplicacao, disciplinaId, anoEscolar);
            return Ok(resultado);
        }

        [HttpGet("comparativo-ue/{dreId}/{disciplinaId}/{anoLetivo}/{anoEscolar}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ProficienciaComparativoUeDto), 200)]
        public async Task<IActionResult> ObterProficienciaComparativoUe(
            int dreId,
            int disciplinaId,
            int anoLetivo,
            int anoEscolar,
            [FromQuery] int? ueId,
            [FromQuery] List<int>? tiposVariacao,
            [FromQuery] string? nomeUe,
            [FromQuery] int? pagina,
            [FromQuery] int? itensPorPagina,
            [FromServices] IObterProficienciaComparativoUeUseCase obterProficienciaComparativoUeUseCase)
        {
            var result = await obterProficienciaComparativoUeUseCase.Executar(dreId, disciplinaId, anoLetivo, anoEscolar, ueId, tiposVariacao, nomeUe, pagina, itensPorPagina);
            return Ok(result);
        }

        [HttpGet("dres-comparacao-por-dre/{dreId}/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(TabelaComparativaDrePspPsaDto), 200)]
        public async Task<IActionResult> ObterTabelaComparativaPorDre(int dreId, int anoAplicacao, int disciplinaId, int anoEscolar,
            [FromServices] IObterProficienciaComparativoDreUseCase useCase)
        {
            var resultado = await useCase.Executar(dreId, anoAplicacao, disciplinaId, anoEscolar);
            return Ok(resultado);
        }

        [HttpGet("anos-aplicacao-sme")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<int>), 200)]
        public async Task<IActionResult> ObterAnosAplicacaoPorSme([FromServices] IObterAnosAplicacaoPorSmeUseCase obterAnosAplicacaoPorSmeUseCase)
        {
            return Ok(await obterAnosAplicacaoPorSmeUseCase.Executar());
        }
        [HttpGet("componentes-curriculares-sme/{anoAplicacao}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoFiltroDto<int>>), 200)]
        public async Task<IActionResult> ObterComponentesCurricularesSmePorAno(int anoAplicacao, [FromServices] IObterComponentesCurricularesSmePorAnoUseCase obterComponentesCurricularesSmePorAnoUseCase)
        {
            return Ok(await obterComponentesCurricularesSmePorAnoUseCase.Executar(anoAplicacao));
        }

        [HttpGet("anos-escolares-sme/{anoAplicacao}/{disciplinaId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoFiltroDto<int>>), 200)]
        public async Task<IActionResult> ObterAnosEscolaresPorSmeAnoAplicacao(int anoAplicacao, int disciplinaId, [FromServices] IObterAnosEscolaresPorSmeAnoAplicacaoUseCase obterAnosEscolaresPorSmeAnoAplicacaoUseCase)
        {
            return Ok(await obterAnosEscolaresPorSmeAnoAplicacaoUseCase.Executar(anoAplicacao, disciplinaId));
        }

        [HttpGet("comparacao-grafico-sme/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(TabelaComparativaDrePspPsaDto), 200)]
        public async Task<IActionResult> ObterGraficoComparativoPorSme(int anoAplicacao, int disciplinaId, int anoEscolar,
         [FromServices] IObterGraficoComparativoProficienciaSmeUseCase useCase)
        {
            var resultado = await useCase.Executar(anoAplicacao, disciplinaId, anoEscolar);
            if(resultado is not null)
                 return Ok(resultado);

            return NoContent();

        }
        [HttpGet("dados-tabela-comparativa-sme/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(TabelaComparativaSmePspPsaDto), 200)]
        public async Task<IActionResult> ObterTabelaComparativaSme(int anoAplicacao, int disciplinaId, int anoEscolar,
            [FromServices] IObterProficienciaComparativoSmeUseCase useCase)
        {
            var resultado = await useCase.Executar(anoAplicacao, disciplinaId, anoEscolar);
            return Ok(resultado);
        }


        [HttpGet("cards-comparativo-sme-por-dre/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(TabelaComparativaDrePspPsaDto), 200)]
        public async Task<IActionResult> ObterCardsComparativoSMEPorDreAnoAplicacao( int anoAplicacao, int disciplinaId, int anoEscolar, 
            [FromQuery] int? dreId,
            [FromQuery] int? pagina,
            [FromQuery] int? itensPorPagina,
            [FromServices] IObterCardComparativoProficienciasSme useCase)
        {
            var resultado = await useCase.Executar(anoAplicacao, disciplinaId, anoEscolar, dreId, pagina, itensPorPagina);
            return Ok(resultado);
        }


        [HttpGet("dres-comparativo-sme/{anoAplicacao}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<DreDto>), 200)]
        public async Task<IActionResult> ObterDresComparativoSme(int anoAplicacao, int disciplinaId, int anoEscolar, [FromServices] IObterDresComparativoSmeUseCase useCase)
        {
            return Ok(await useCase.Executar(anoAplicacao, disciplinaId, anoEscolar));
        }
    }
}