using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Dominio.Entidades;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AlunoController : ControllerBase
    {
        [HttpGet("{alunoRA}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SalvarPreferencias(long alunoRA,
            [FromServices] IObterAlunoPorRaUseCase obterAlunoPorRaUseCase)
        {
            return Ok(await obterAlunoPorRaUseCase.Executar(alunoRA));
        }
    }
}