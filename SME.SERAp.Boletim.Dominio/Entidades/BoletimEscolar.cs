using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class BoletimEscolar : EntidadeBase
    {
        public long UeId { get; set; }
        public long ProvaId { get; set; }
        public string ComponenteCurricular { get; set; }
        public decimal AbaixoBasico { get; set; }
        public decimal AbaixoBasicoPorcentagem { get; set; }
        public decimal Basico { get; set; }
        public decimal BasicoPorcentagem { get; set; }
        public decimal Adequado { get; set; }
        public decimal AdequadoPorcentagem { get; set; }
        public decimal Avancado { get; set; }
        public decimal AvancadoPorcentagem { get; set; }
        public decimal Total { get; set; }
        public decimal MediaProficiencia { get; set; }
    }
}