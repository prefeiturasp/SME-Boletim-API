namespace SME.SERAp.Boletim.Infra.Extensions
{
    public static class DreExtensions
    {
        private static string _drePrefixo => "DIRETORIA REGIONAL DE EDUCACAO";
        private static string _dreAbreviacaoPrefixo => "DRE";

        public static string AbreviarPrefixoDre(this string dreNome)
        {
            return dreNome?.Replace(_drePrefixo, _dreAbreviacaoPrefixo).Trim();
        }

        public static string RemoverPrefixoDre(this string dreNome)
        {
            return dreNome?.Replace(_drePrefixo, string.Empty).Trim();
        }
    }
}
