using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        [HttpPost("autenticacao")]
        //[ChaveAutenticacaoApi]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AutenticacaoValidarDto), 200)]
        public async Task<IActionResult> Autenticar([FromServices] IAutenticacaoUseCase autenticarUseCase,
            [FromBody] AutenticacaoDto autenticacaoDto)
        {
            return Ok(await autenticarUseCase.Executar(autenticacaoDto));
        }

        [HttpPost("validar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AutenticacaoRetornoDto), 200)]
        public async Task<IActionResult> Validar([FromServices] IAutenticacaoValidarUseCase autenticacaoValidarUseCase,
           [FromBody] AutenticacaoValidarDto autenticacaoValidarDto)
        {
            return Ok(await autenticacaoValidarUseCase.Executar(autenticacaoValidarDto));
        }
    }
}
