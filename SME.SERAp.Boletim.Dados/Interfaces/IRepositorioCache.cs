﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioCache
    {
        Task SalvarRedisAsync(string nomeChave, object valor, int minutosParaExpirar = 720);
        Task RemoverRedisAsync(string nomeChave);
        Task<T> ObterRedisAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720);
        Task<T> ObterRedisAsync<T>(string nomeChave);
        Task<bool> ExisteChaveAsync(string nomeChave);
        Task<string> ObterRedisToJsonAsync(string nomeChave, Func<Task<string>> buscarDados, int minutosParaExpirar = 720);
        Task<string> ObterRedisToJsonAsync(string nomeChave);
        Task SalvarRedisToJsonAsync(string nomeChave, string json, int minutosParaExpirar = 720);
    }
}