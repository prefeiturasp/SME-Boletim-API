using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class BoletimEscolarMap : DommelEntityMap<BoletimEscolar>
    {
        public BoletimEscolarMap()
        {
            ToTable("boletim_escolar");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.ComponenteCurricular).ToColumn("componente_curricular");
            Map(c => c.AbaixoBasico).ToColumn("abaixo_basico");
            Map(c => c.AbaixoBasicoPorcentagem).ToColumn("abaixo_basico_porcentagem");
            Map(c => c.Basico).ToColumn("basico");
            Map(c => c.BasicoPorcentagem).ToColumn("basico_porcentagem");
            Map(c => c.Adequado).ToColumn("adequado");
            Map(c => c.AdequadoPorcentagem).ToColumn("adequado_porcentagem");
            Map(c => c.Avancado).ToColumn("avancado");
            Map(c => c.AvancadoPorcentagem).ToColumn("avancado_porcentagem");
            Map(c => c.Total).ToColumn("total");
            Map(c => c.MediaProficiencia).ToColumn("media_proficiencia");
        }
    }
}