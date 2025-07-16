using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;

namespace SME.SERAp.Boletim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AbrangenciaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeDto>), 200)]
        public async Task<IActionResult> ObterUesAbrangenciaUsuarioLogado(
            [FromServices] IObterUesAbrangenciaUsuarioLogadoUseCase obterUesAbrangenciaUsuarioLogadoUseCase)
        {
            return Ok(await obterUesAbrangenciaUsuarioLogadoUseCase.Executar());
        }

        [HttpGet("dres")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<DreAbragenciaDetalheDto>), 200)]
        public async Task<IActionResult> ObterDresAbrangenciaUsuarioLogado(
            [FromServices] IObterDresAbrangenciaUsuarioLogadoUseCase obterDresAbrangenciaUsuarioLogadoUseCase)
        {
            return Ok(await obterDresAbrangenciaUsuarioLogadoUseCase.Executar());
        }
    }
}
