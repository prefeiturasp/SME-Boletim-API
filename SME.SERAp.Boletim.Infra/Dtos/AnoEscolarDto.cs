using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AnoEscolarDto
    {
        public int Ano { get; set; }

        public Modalidade Modalidade { get; set; }

        public string Descricao
        {
            get
            {
                if (Modalidade == Modalidade.Medio || Modalidade == Modalidade.ETEC)
                {
                    return $"{Ano}ª Serie";
                }

                return $"{Ano}º Ano";
            }
        }
    }
}
