using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoletimEscolarController : ControllerBase
    {
        [HttpGet("{codigoUe}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimPorUe(long codigoUe,
            [FromServices] IObterBoletimEscolarPorUeUseCase obterBoletimEscolarPorUeUseCase, [FromQuery] FiltroBoletimDto filtros)
        {
            return Ok(await obterBoletimEscolarPorUeUseCase.Executar(codigoUe, filtros));
        }

        [HttpGet("{codigoUe}/turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(long codigoUe,
            [FromServices] IObterBoletimEscolarTurmaPorUeUseCase obterBoletimEscolarTurmaPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarTurmaPorUeUseCase.Executar(codigoUe));
        }

        [HttpGet("download/{codigoUe}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterBoletimEscolarTurmaPorUe(string codigoUe,
          [FromServices] IObterDownloadBoletimProvaEscolarUseCase obterDownloadBoletimProvaEscolarUseCase)
        {
            var file = await obterDownloadBoletimProvaEscolarUseCase.Executar(codigoUe);

            return File(file, "application/vnd.ms-excel", "relatorio.xls", enableRangeProcessing: true);
        }

        [HttpGet("{ueId}/estudantes")]
        [ProducesResponseType(typeof(BoletimEscolarComDisciplinasDto), 200)]
        public async Task<IActionResult> ObterAbaEstudanteBoletimEscolarPorUeId(long ueId,
        [FromServices] IObterAbaEstudanteBoletimEscolarPorUeIdUseCase obterAbaEstudanteBoletimEscolarPorUeIdUseCase,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var resultado = await obterAbaEstudanteBoletimEscolarPorUeIdUseCase.Executar(ueId, pageNumber, pageSize);
            return Ok(resultado);
        }

        [HttpGet("{codigoUe}/filtros")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterOpcoesFiltrosBoletimEscolarPorUe(long codigoUe,
            [FromServices] IObterBoletimEscolarOpcoesFiltrosPorUeUseCase obterBoletimEscolarOpcoesFiltrosPorUeUseCase)
        {
            return Ok(await obterBoletimEscolarOpcoesFiltrosPorUeUseCase.Executar(codigoUe));
        }
    }
}