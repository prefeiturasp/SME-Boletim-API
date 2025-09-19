using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Boletim.Infra.Extensions
{
    public static class BoletimExtensions
    {
        public static string ObterUeDescricao(this string ueNome, TipoEscola tipoEscola, string dreNome, string dreAbreviacao)
        {
            var dreNomeAbv = dreAbreviacao?.Replace(" - ", " ").Trim();

            var tipoEscolaAbrev = ObterTipoEscolaAbreviado(tipoEscola);

            return $"{dreNomeAbv} - {tipoEscolaAbrev} {ueNome}";
        }

        public static string ObterNomeDreAbreviado(this string dreNome)
        {
            var dreNomeAbreviado = dreNome?.Replace("DIRETORIA REGIONAL DE EDUCACAO", "DRE");

            return $"{dreNomeAbreviado}";
        }

        private static string ObterTipoEscolaAbreviado(TipoEscola tipoEscola)
        {
            if (tipoEscola == TipoEscola.Nenhum) return string.Empty;

            var atributoTipoEscola = tipoEscola.GetAttribute<DisplayAttribute>();
            return atributoTipoEscola?.ShortName ?? string.Empty;
        }

        public static double CalcularPercentual(this decimal valorFinal, decimal valorInicial)
        {
            if (valorInicial == 0)
            {
                return 0.0;
            }

            var variacao = ((valorFinal - valorInicial) / valorInicial) * 100;
            return (double)Math.Round(variacao, 2);
        }
    }
}