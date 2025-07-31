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
            return UeNome?.ObterUeDescricao(TipoEscola, DreNome, DreNomeAbreviado) ?? string.Empty;
        }
    }
}