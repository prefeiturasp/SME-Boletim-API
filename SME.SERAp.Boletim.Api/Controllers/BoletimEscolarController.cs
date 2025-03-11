using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;

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
    }
}