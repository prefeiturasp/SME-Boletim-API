using Microsoft.AspNetCore.Mvc.Filters;

namespace SME.SERAp.Boletim.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ChaveAutenticacaoApiAttribute : Attribute, IAsyncActionFilter
    {
        private const string ChaveHeader = "chave-api";
        private const string ChaveEnvironmentVariableName = "ChaveSerapProvaApi";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
#if !DEBUG
            string chaveApi = Environment.GetEnvironmentVariable(ChaveEnvironmentVariableName);
            if (!context.HttpContext.Request.Headers.TryGetValue(ChaveHeader, out var chaveRecebida) || !chaveRecebida.Equals(chaveApi))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }
#endif
            await next();
        }
    }
}
