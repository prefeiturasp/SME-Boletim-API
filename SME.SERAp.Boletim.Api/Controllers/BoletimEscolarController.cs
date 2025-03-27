using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
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
        [HttpGet("{ueId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarDto), 200)]
        public async Task<IActionResult> ObterBoletimPorUe(long ueId,
            [FromServices] IObterBoletimEscolarPorUeUseCase obterBoletimEscolarPorUeUseCase, [FromQuery] FiltroBoletimDto filtros)
        {
            return Ok(await obterBoletimEscolarPorUeUseCase.Executar(ueId, filtros));
        }

        [HttpGet("{ueId}/turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarPorTurmaDto), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(long ueId, [FromQuery] FiltroBoletimDto filtros,
            [FromServices] IObterBoletimEscolarTurmaPorUeUseCase obterBoletimEscolarTurmaPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarTurmaPorUeUseCase.Executar(ueId, filtros));
        }

        [HttpGet("download/{ueId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(long ueId,
          [FromServices] IObterDownloadBoletimProvaEscolarUseCase obterDownloadBoletimProvaEscolarUseCase)
        {
            var file = await obterDownloadBoletimProvaEscolarUseCase.Executar(ueId);

            return File(file, "application/vnd.ms-excel", "relatorio.xls", enableRangeProcessing: true);
        }

        [HttpGet("{ueId}/estudantes")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarComDisciplinasDto), 200)]
        public async Task<IActionResult> ObterAbaEstudanteBoletimEscolarPorUeId(long ueId, [FromQuery] FiltroBoletimEstudantePaginadoDto filtros,
            [FromServices] IObterAbaEstudanteBoletimEscolarPorUeIdUseCase obterAbaEstudanteBoletimEscolarPorUeIdUseCase)
        {
            var resultado = await obterAbaEstudanteBoletimEscolarPorUeIdUseCase.Executar(ueId, filtros);
            return Ok(resultado);
        }

        [HttpGet("{ueId}/filtros")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(BoletimEscolarOpcoesFiltrosDto), 200)]
        public async Task<IActionResult> ObterOpcoesFiltrosBoletimEscolarPorUe(long ueId,
            [FromServices] IObterBoletimEscolarOpcoesFiltrosPorUeUseCase obterBoletimEscolarOpcoesFiltrosPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarOpcoesFiltrosPorUeUseCase.Executar(ueId));
        }

        [HttpGet("{ueId}/estudantes-grafico")]
        [ProducesResponseType(typeof(IEnumerable<AbaEstudanteGraficoDto>), 200)]
        public async Task<IActionResult> ObterAbaEstudanteGraficoPorUeId(long ueId, [FromQuery] FiltroBoletimEstudanteDto filtros,
            [FromServices] IObterAbaEstudanteGraficoPorUeIdUseCase obterAbaEstudanteGraficoPorUeIdUseCase)
        {
            var resultado = await obterAbaEstudanteGraficoPorUeIdUseCase.Executar(ueId, filtros);
            return Ok(resultado);
        }

        [HttpGet("nome-aplicacao")]
        [ProducesResponseType(typeof(IEnumerable<LoteProvaAtivoDto>), 200)]
        public async Task<IActionResult> ObterBoletimNomeAplicacaoProva([FromServices] IObterBoletimNomeAplicacaoProvaUseCase obterLoteProvaAtivo)
        {
            var resultado = await obterLoteProvaAtivo.Executar();
            return Ok(resultado);
        }

        [HttpGet("{ueId}/{disciplinaId}/{anoEscolar}/resultado-probabilidade")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ResultadoProbabilidadeAgrupadoDto>), 200)]
        public async Task<IActionResult> ObterResultadoProbabilidadePorUeAsync(long ueId, long disciplinaId, int anoEscolar,
        [FromQuery] FiltroBoletimResultadoProbabilidadeDto filtros,
        [FromServices] IObterResultadoProbabilidadePorUeUseCase obterResultadoProbabilidadePorUeUseCase)
        {
            if (filtros.Pagina < 1 || filtros.TamanhoPagina < 1)
                return BadRequest("Página e tamanho da página devem ser maiores que zero.");

            var resultado = await obterResultadoProbabilidadePorUeUseCase.Executar(ueId, disciplinaId, anoEscolar, filtros);
            return Ok(resultado);
        }

        [HttpGet("download-probabilidade/{ueId}/{disciplinaId}/{anoEscolar}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterDownloadResultadoProbabilidade(long ueId, long disciplinaId, int anoEscolar,
        [FromServices] IObterDownloadResultadoProbabilidadeUseCase useCase)
        {
            var file = await useCase.Executar(ueId, disciplinaId, anoEscolar);
            return File(file, "application/vnd.ms-excel", $"resultado_probabilidade_{DateTime.Now:dd-MM-yyyy}.xls", enableRangeProcessing: true);
        }
    }
}