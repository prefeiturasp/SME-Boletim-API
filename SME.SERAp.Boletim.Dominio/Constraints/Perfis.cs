namespace SME.SERAp.Boletim.Dominio.Constraints
{
    public static class Perfis
    {
        public readonly static Guid PERFIL_ADMINISTRADOR = Guid.Parse("AAD9D772-41A3-E411-922D-782BCB3D218E");
        public readonly static Guid PERFIL_ADMINISTRADOR_NTA = Guid.Parse("22366A3E-9E4C-E711-9541-782BCB3D218E");

        public readonly static Guid PERFIL_PROFESSOR = Guid.Parse("E77E81B1-191E-E811-B259-782BCB3D2D76");
        public readonly static Guid PERFIL_PROFESSOR_OLD = Guid.Parse("067D9B21-A1FF-E611-9541-782BCB3D218E");

        public static bool PerfilEhValido(Guid perfil)
        {
            return
                perfil == Perfis.PERFIL_ADMINISTRADOR ||
                perfil == Perfis.PERFIL_ADMINISTRADOR_NTA ||
                perfil == Perfis.PERFIL_PROFESSOR ||
                perfil == Perfis.PERFIL_PROFESSOR_OLD;
        }

        public static bool PerfilEhAdministrador(Guid perfil)
        {
            return
                PerfilEhValido(perfil) &&
                (perfil == Perfis.PERFIL_ADMINISTRADOR || perfil == Perfis.PERFIL_ADMINISTRADOR_NTA);
        }

        public static bool PerfilEhProfessor(Guid perfil)
        {
            return PerfilEhValido(perfil) &&
                (perfil == Perfis.PERFIL_PROFESSOR || perfil == Perfis.PERFIL_PROFESSOR_OLD);
        }
    }
}
