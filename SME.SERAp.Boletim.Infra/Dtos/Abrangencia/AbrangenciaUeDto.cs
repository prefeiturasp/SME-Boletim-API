using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Boletim.Infra.Dtos.Abrangencia
{
    public class AbrangenciaUeDto : DtoBase
    {
        public long DreId { get; set; }

        public long UeId { get; set; }

        public string DreAbreviacao { get; set; }

        public string UeNome { get; set; }

        public TipoEscola UeTipo { get; set; }

        public string Descricao => ConstruirDescricao();

        private string ConstruirDescricao()
        {
            var dreAbreviacao = ObterDreAbreviacao();
            var tipoEscola = ObterTipoEscolaAbreviado();

            var descricao = !string.IsNullOrWhiteSpace(dreAbreviacao)
                ? $"{dreAbreviacao} - "
                : string.Empty;

            descricao += !string.IsNullOrWhiteSpace(tipoEscola)
                ? $"{tipoEscola} {UeNome}"
                : UeNome;

            return descricao;
        }

        private string ObterDreAbreviacao()
        {
            var partesDre = DreAbreviacao?.Split('-');

            if (partesDre?.Length > 1)
            {
                return $"{partesDre[0].Trim()} {partesDre[1].Trim()}";
            }

            return partesDre?.FirstOrDefault()?.Trim() ?? string.Empty;
        }

        private string ObterTipoEscolaAbreviado()
        {
            if (UeTipo == TipoEscola.Nenhum) return string.Empty;

            var atributoTipoEscola = UeTipo.GetAttribute<DisplayAttribute>();
            return atributoTipoEscola?.ShortName ?? string.Empty;
        }
    }
}
