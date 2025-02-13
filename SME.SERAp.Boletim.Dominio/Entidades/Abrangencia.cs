namespace SME.SERAp.Boletim.Dominio.Entidades
{
    public class Abrangencia : EntidadeBase
    {
        public Abrangencia() { }

        public long UsuarioId { get; set; }

        public long GrupoId { get; set; }

        public long DreId { get; set; }

        public long UeId { get; set; }

        public long TurmaId { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fim {  get; set; }
    }
}
