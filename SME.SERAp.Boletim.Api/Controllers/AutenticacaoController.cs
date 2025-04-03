using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Api.Filters;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Abrangencia;
using SME.SERAp.Boletim.Infra.Dtos.LoteProva;

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

        [HttpGet("validacao-api")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ValidacaoController()
        {
            return Ok(true);
        }

        [HttpGet("validacao-banco")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(LoteProvaAtivoDto), 200)]
        public async Task<IActionResult> ValidacaoBancoController([FromServices] IObterBoletimNomeAplicacaoProvaUseCase obterLoteProvaAtivo)
        {
            try
            {
                var resultado = await obterLoteProvaAtivo.Executar();
                return Ok(resultado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
