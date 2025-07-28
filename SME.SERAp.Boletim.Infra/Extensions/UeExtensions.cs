using SME.SERAp.Boletim.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Boletim.Infra.Extensions
{
    public static class UeExtensions
    {
        public static string ObterUeDescricao(this string ueNome, TipoEscola tipoEscola, string dreNome, string dreAbreviacao)
        {
            var dreSiglas = dreNome?.Replace("DIRETORIA REGIONAL DE EDUCACAO", "DRE").Trim();

            var dreNomeAbv = dreAbreviacao?.Replace(" - ", " ").Trim();

            var tipoEscolaAbrev = ObterTipoEscolaAbreviado(tipoEscola);

            return $"{dreNomeAbv} - {tipoEscolaAbrev} {ueNome}";
        }

        private static string ObterTipoEscolaAbreviado(TipoEscola tipoEscola)
        {
            if (tipoEscola == TipoEscola.Nenhum) return string.Empty;

            var atributoTipoEscola = tipoEscola.GetAttribute<DisplayAttribute>();
            return atributoTipoEscola?.ShortName ?? string.Empty;
        }
    }
}
