using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Dominio.Constraints
{
    public static class TipoEscolaBoletim
    {
        private static readonly int[] _tiposPermitidos =
        {
            (int)TipoEscola.EMEF,      // 1
            (int)TipoEscola.EMEFM,     // 3
            (int)TipoEscola.EMEBS,     // 4
            (int)TipoEscola.CEUEMEF,   // 16
            (int)TipoEscola.EMEFPFOM   // 32
        };

        public static int[] ObterTipos() => (int[])_tiposPermitidos.Clone();

        public static string ObterTiposTexto() => string.Join(", ", _tiposPermitidos);

        public static bool EhPermitido(int tipoEscola) => _tiposPermitidos.Contains(tipoEscola);

        public static bool EhPermitido(int? tipoEscola) => tipoEscola.HasValue && EhPermitido(tipoEscola.Value);
    }
}