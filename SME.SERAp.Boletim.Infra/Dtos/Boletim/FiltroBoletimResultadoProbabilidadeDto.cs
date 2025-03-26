namespace SME.SERAp.Boletim.Infra.Dtos.Boletim
{
    public class FiltroBoletimResultadoProbabilidadeDto
    {
        public List<string> Turma { get; set; } = new();

        public string Habilidade { get; set; }

        public int Pagina { get; set;} = 1;

        public int TamanhoPagina { get; set;} = 10;
    }
}
