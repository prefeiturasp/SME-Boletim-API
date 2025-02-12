using Elastic.Apm.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SERAp.Boletim.Aplicacao.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.IoC.Extensions
{
    public static class RegistraMediatr
    {
        public static void AdicionarMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("SME.SERAp.Boletim.Aplicacao");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));
        }
    }
}