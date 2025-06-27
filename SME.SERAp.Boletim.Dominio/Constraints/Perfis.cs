using SME.SERAp.Boletim.Dominio.Enumerados;

namespace SME.SERAp.Boletim.Dominio.Constraints
{
    public static class Perfis
    {
        public readonly static Guid PERFIL_ADMINISTRADOR = Guid.Parse("AAD9D772-41A3-E411-922D-782BCB3D218E");
        public readonly static Guid PERFIL_ADMINISTRADOR_NTA = Guid.Parse("22366A3E-9E4C-E711-9541-782BCB3D218E");

        public readonly static Guid PERFIL_ADMINISTRADOR_COPED_LEITURA = Guid.Parse("A8CB8D7B-F333-E711-9541-782BCB3D218E");

        public readonly static Guid PERFIL_ADMINISTRADOR_DRE = Guid.Parse("104F0759-87E8-E611-9541-782BCB3D218E");
        public readonly static Guid PERFIL_ADMINISTRADOR_NA_DRE = Guid.Parse("4318D329-17DC-4C48-8E59-7D80557F7E77");

        public readonly static Guid PERFIL_DIRETOR_ESCOLAR = Guid.Parse("75DCAB30-2C1E-E811-B259-782BCB3D2D76");
        public readonly static Guid PERFIL_ASSISTENTE_DIRETOR_UE = Guid.Parse("ECF7A20D-1A1E-E811-B259-782BCB3D2D76");

        public readonly static Guid PERFIL_COORDENADOR_PEDAGOGICO = Guid.Parse("D4026F2C-1A1E-E811-B259-782BCB3D2D76");

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

        public static TipoPerfil ObterTipoPerfil(Guid perfil)
        {
            if(PerfilEhAdministrador(perfil) || perfil == PERFIL_ADMINISTRADOR_COPED_LEITURA)
                return TipoPerfil.Administrador;

            if(perfil == PERFIL_ADMINISTRADOR_DRE || perfil == PERFIL_ADMINISTRADOR_NA_DRE)
                return TipoPerfil.Administrador_DRE;

            if(perfil == PERFIL_DIRETOR_ESCOLAR || perfil == PERFIL_ASSISTENTE_DIRETOR_UE)
                return TipoPerfil.Diretor;

            if (perfil == PERFIL_COORDENADOR_PEDAGOGICO)
                return TipoPerfil.Coordenador;

            return TipoPerfil.Professor;
        }
    }
}
