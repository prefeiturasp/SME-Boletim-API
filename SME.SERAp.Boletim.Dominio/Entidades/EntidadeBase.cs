using MessagePack;

namespace SME.SERAp.Boletim.Dominio.Entidades
{
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class EntidadeBase
    {
        public long Id { get; set; }
    }
}