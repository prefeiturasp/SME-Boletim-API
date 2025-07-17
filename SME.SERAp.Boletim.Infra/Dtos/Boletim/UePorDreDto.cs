using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class UePorDreDto
    {
        public long UeId { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long DreId { get; set; }
        public string DreNomeAbreviado { get; set; }
        public string DreNome { get; set; }

        public string Descricao => ConstruirDescricao();

        private string ConstruirDescricao()
        {
            var dreSiglas = DreNome?.Replace("DIRETORIA REGIONAL DE EDUCACAO", "DRE").Trim();

            var dreNomeAbv = DreNomeAbreviado?.Replace(" - ", " ").Trim();

            var tipoEscolaAbrev = ObterTipoEscolaAbreviado();

            //return $"{dreSiglas}   {tipoEscolaAbrev} {UeNome}";
            return $"{dreNomeAbv} - {tipoEscolaAbrev} {UeNome}";
        }

        private string ObterTipoEscolaAbreviado()
        {
            if (TipoEscola == TipoEscola.Nenhum) return string.Empty;

            var atributoTipoEscola = TipoEscola.GetAttribute<DisplayAttribute>();
            return atributoTipoEscola?.ShortName ?? string.Empty;
        }
    }
}