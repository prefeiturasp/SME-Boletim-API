using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadProvasBoletimEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Text;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadBoletimProvaEscolarUseCase : IObterDownloadBoletimProvaEscolarUseCase
    {
        private readonly IMediator mediator;
        public ObterDownloadBoletimProvaEscolarUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<MemoryStream> Executar(string ueId)
        {
            var abrangenciasUsuarioLogado = await mediator
                 .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == Convert.ToInt64(ueId)) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var provasBoletimEscola = await mediator.Send(new ObterDownloadProvasBoletimEscolarQuery(ueId));

            return await BuildCSVForExcel(provasBoletimEscola);

        }

        private static async Task<MemoryStream> BuildCSVForExcel(IEnumerable<DownloadProvasBoletimEscolarDto> downloadProvasBoletimEscolarDtos)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                await writer.WriteLineAsync("<html><head>");
                await writer.WriteLineAsync("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                await writer.WriteLineAsync("<style>");
                await writer.WriteLineAsync("th { font-weight: bold; }");
                await writer.WriteLineAsync(".numero { text-align: center; }");
                await writer.WriteLineAsync("</style>");
                await writer.WriteLineAsync("</head><body>");
                await writer.WriteLineAsync("<table border='1'>");
                await writer.WriteLineAsync("<tr><th>Codigo UE</th><th>Nome UE</th><th>Ano Escola</th><th>Turma</th><th>Aluno RA</th><th>Nome Aluno</th><th>Componente</th><th>Proficiência</th><th>Nível</th></tr>");

                foreach (var item in downloadProvasBoletimEscolarDtos)
                {
                    await writer.WriteLineAsync("<tr>" +
                        $"<td class=\"numero\">{item.CodigoUE}</td>" +
                        $"<td>{item.NomeUE}</td>" +
                        $"<td class=\"numero\">{item.AnoEscola}</td>" +
                        $"<td>{item.Turma}</td>" +
                        $"<td class=\"numero\">{item.AlunoRA}</td>" +
                        $"<td>{item.NomeAluno}</td>" +
                        $"<td>{item.Componente}</td>" +
                        $"<td class=\"numero\">{item.Proficiencia}</td>" +
                        $"<td>{item.Nivel}</td>" +
                        "</tr>");
                }

                await writer.WriteLineAsync("</table></body></html>");
                await writer.FlushAsync();
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
