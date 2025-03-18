using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Boletim.Dominio.Enumerados
{
    public enum TipoNivelProficiencia
    {
        [Display(Name = "Abaixo do básico")]
        AbaixoBasico = 1,

        [Display(Name = "Básico")]
        Basico = 2,

        [Display(Name = "Adequado")]
        Adequado = 3,

        [Display(Name = "Avançado")]
        Avancado = 4
    }
}
