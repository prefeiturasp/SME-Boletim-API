using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.UseCase;
using SME.SERAp.Boletim.Dados.Cache;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dados.Repositorios.Serap;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;
using SME.SERAp.Boletim.IoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();

            RegistrarRepositorios(services);
            RegistrarServicos(services);
            RegistrarCasosDeUso(services);
            RegistraMapeamentos.Registrar();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScoped<IRepositorioCache, RepositorioCache>();
            services.TryAddScoped<IRepositorioAluno, RepositorioAluno>();
            services.TryAddScoped<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.TryAddScoped<IRepositorioBoletimEscolar, RepositorioBoletimEscolar>();
            services.TryAddScoped<IRepositorioBoletimProvaAluno, RepositorioBoletimProvaAluno>();
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScoped<IServicoTelemetria, ServicoTelemetria>();
            services.TryAddScoped<IServicoLog, ServicoLog>();

        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.TryAddScoped<IObterAlunoPorRaUseCase, ObterAlunoPorRaUseCase>();
            services.TryAddScoped<IObterUesAbrangenciaUsuarioLogadoUseCase, ObterUesAbrangenciaUsuarioLogadoUseCase>();
            services.TryAddScoped<IAutenticacaoUseCase, AutenticacaoUseCase>();
            services.TryAddScoped<IAutenticacaoValidarUseCase, AutenticacaoValidarUseCase>();
            services.TryAddScoped<IObterBoletimEscolarPorUeUseCase, ObterBoletimEscolarPorUeUseCase>();
            services.TryAddScoped<IObterBoletimEscolarTurmaPorUeUseCase, ObterBoletimEscolarTurmaPorUeUseCase>();
            services.TryAddScoped<IObterAbaEstudanteBoletimEscolarPorUeIdUseCase, ObterAbaEstudanteBoletimEscolarPorUeIdUseCase>();
            services.TryAddScoped<IObterBoletimEscolarOpcoesFiltrosPorUeUseCase, ObterBoletimEscolarOpcoesFiltrosPorUeUseCase>();
            services.TryAddScoped<IObterAbaEstudanteGraficoPorUeIdUseCase, ObterAbaEstudanteGraficoPorUeIdUseCase>();
        }
    }
}