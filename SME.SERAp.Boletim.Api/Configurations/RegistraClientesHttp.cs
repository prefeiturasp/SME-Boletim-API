﻿using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Api.Configurations
{
    public static class RegistraClientesHttp
    {
        public static void Registrar(IServiceCollection services, GithubOptions githubOptions)
        {
            var policy = ObterPolicyBaseHttp();

            services.AddHttpClient(name: "githubApi", c =>
            {
                c.BaseAddress = new Uri(githubOptions.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "SGP");

            }).AddPolicyHandler(policy);
        }

        static IAsyncPolicy<HttpResponseMessage> ObterPolicyBaseHttp()
        {
            return HttpPolicyExtensions
                 .HandleTransientHttpError()
                 .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                             retryAttempt)));
        }
    }
}