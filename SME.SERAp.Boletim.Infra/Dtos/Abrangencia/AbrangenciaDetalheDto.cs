namespace SME.SERAp.Boletim.Infra.Dtos.Abrangencia
{
    public class AbrangenciaDetalheDto : DtoBase
    {
        public long Id { get; set; }

        public long UsuarioId { get; set; }

        public string Login { get; set; }

        public string Usuario { get; set; }

        public long GrupoId { get; set; }

        public Guid Pefil { get; set; }

        public string Grupo {  get; set; }

        public long DreId { get; set; }

        public long? UeId { get; set; }

        public long TurmaId { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fim {  get; set; }

        public AbrangenciaDetalheDto()
        {
            
        }

        public AbrangenciaDetalheDto(string login, Guid perfil): this()
        {
            Login = login;
            Pefil = perfil;
            DreId = 0;
            UeId = 0;
            TurmaId = 0;
        }
    }
}
