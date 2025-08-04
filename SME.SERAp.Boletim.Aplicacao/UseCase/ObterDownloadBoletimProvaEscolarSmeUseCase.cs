using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Text;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadBoletimProvaEscolarSmeUseCase : IObterDownloadBoletimProvaEscolarSmeUseCase
    {
        private readonly IMediator mediator;
        public ObterDownloadBoletimProvaEscolarSmeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<MemoryStream> Executar(long loteId)
        {
            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if (tipoPerfilUsuarioLogado is null || tipoPerfilUsuarioLogado.Value != TipoPerfil.Administrador)
                throw new NaoAutorizadoException("Usuário sem permissão.");

            var resultados = await mediator.Send(new ObterDownloadProvasBoletimEscolarSmeQuery(loteId));
            return await BuildCSVForExcel(resultados);
        }

        private static async Task<MemoryStream> BuildCSVForExcel(IEnumerable<DownloadProvasBoletimEscolarPorDreDto> dados)
        {
            var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);

            await writer.WriteLineAsync("<html><head>");
            await writer.WriteLineAsync("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            await writer.WriteLineAsync("<style>");
            await writer.WriteLineAsync("th { font-weight: bold; }");
            await writer.WriteLineAsync(".numero { text-align: center; }");
            await writer.WriteLineAsync("</style>");
            await writer.WriteLineAsync("</head><body>");
            await writer.WriteLineAsync("<table border='1'>");
            await writer.WriteLineAsync("<tr><th>Nome DRE</th><th>Codigo UE</th><th>Nome UE</th><th>Ano Escola</th><th>Turma</th><th>Aluno RA</th><th>Nome Aluno</th><th>Componente</th><th>Proficiência</th><th>Nível</th></tr>");

            foreach (var item in dados)
            {
                await writer.WriteLineAsync("<tr>" +
                    $"<td>{item.NomeDreAbreviacao}</td>" +
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
