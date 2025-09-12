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
            services.TryAddScoped<IRepositorioLoteProva, RepositorioLoteProva>();
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
            services.TryAddScoped<IObterDownloadBoletimProvaEscolarUseCase, ObterDownloadBoletimProvaEscolarUseCase>();
            services.TryAddScoped<IObterAbaEstudanteBoletimEscolarPorUeIdUseCase, ObterAbaEstudanteBoletimEscolarPorUeIdUseCase>();
            services.TryAddScoped<IObterBoletimEscolarOpcoesFiltrosPorUeUseCase, ObterBoletimEscolarOpcoesFiltrosPorUeUseCase>();
            services.TryAddScoped<IObterAbaEstudanteGraficoPorUeIdUseCase, ObterAbaEstudanteGraficoPorUeIdUseCase>();
            services.TryAddScoped<IObterBoletimAplicacoesProvaUseCase, ObterBoletimAplicacoesProvaUseCase>();
            services.TryAddScoped<IObterResultadoProbabilidadePorUeUseCase, ObterResultadoProbabilidadePorUeUseCase>();
            services.TryAddScoped<IObterResultadoProbabilidadePorUeListaUseCase, ObterResultadoProbabilidadePorUeListaUseCase>();
            services.TryAddScoped<IObterAnosEscolaresPorLoteIdUseCase, ObterAnosEscolaresPorLoteIdUseCase>();
            services.TryAddScoped<IObterDresAbrangenciaUsuarioLogadoUseCase, ObterDresAbrangenciaUsuarioLogadoUseCase>();
            services.TryAddScoped<IObterUesPorNivelProficienciaDisciplinaPorDreUseCase, ObterUesPorNivelProficienciaDisciplinaPorDreUseCase>();
            services.TryAddScoped<IObterDownloadResultadoProbabilidadeUseCase, ObterDownloadResultadoProbabilidadeUseCase>();
            services.TryAddScoped<IObterBoletimEscolarResumoDreUseCase, ObterBoletimEscolarResumoDreUseCase>();
            services.TryAddScoped<IObterUesPorDreUseCase, ObterUesPorDreUseCase>();
            services.TryAddScoped<IObterDownloadBoletimProvaEscolarPorDreUseCase, ObterDownloadBoletimProvaEscolarPorDreUseCase>();
            services.TryAddScoped<IObterBoletimDadosUesPorDreUseCase, ObterBoletimDadosUesPorDreUseCase>();
            services.TryAddScoped<IObterBoletimEscolarResumoSmeUseCase, ObterBoletimEscolarResumoSmeUseCase>();
            services.TryAddScoped<IObterDresPorNivelProficienciaDisciplinaUseCase, ObterDresPorNivelProficienciaDisciplinaUseCase>();
            services.TryAddScoped<IObterBoletimEscolarDresMediaProficienciaUseCase, ObterBoletimEscolarDresMediaProficienciaUseCase>();
            services.TryAddScoped<IObterDownloadBoletimProvaEscolarSmeUseCase, ObterDownloadBoletimProvaEscolarSmeUseCase>();
            services.TryAddScoped<IObterDownloadSmeResultadoProbabilidadeUseCase, ObterDownloadSmeResultadoProbabilidadeUseCase>();
            services.TryAddScoped<IObterDresUseCase, ObterDresUseCase>();
            services.TryAddScoped<IObterProficienciaDreUseCase, ObterProficienciaDreUseCase>();
            services.TryAddScoped<IObterDownloadDreResultadoProbabilidadeUseCase, ObterDownloadDreResultadoProbabilidadeUseCase>();
            services.TryAddScoped<IObterProficienciaComparativoProvaSPUseCase, ObterProficienciaComparativoProvaSPUseCase>();
            services.TryAddScoped<IObterTurmasUeAnoUseCase, ObterTurmasUeAnoUseCase>();
        }
    }
}