using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterDownloadDreResultadoProbabilidade;
using SME.SERAp.Boletim.Dominio.Constraints;
using SME.SERAp.Boletim.Dominio.Enumerados;
using SME.SERAp.Boletim.Infra.Dtos.BoletimEscolar;
using SME.SERAp.Boletim.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterDownloadDreResultadoProbabilidadeUseCase : IObterDownloadDreResultadoProbabilidadeUseCase
    {
        private readonly IMediator mediator;
        public ObterDownloadDreResultadoProbabilidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<MemoryStream> Executar(long loteId, int dreId)
        {
            var dresAbrangenciaUsuarioLogado = await mediator
                .Send(new ObterDresAbrangenciaUsuarioLogadoQuery());

            var tipoPerfilUsuarioLogado = await mediator
                .Send(new ObterTipoPerfilUsuarioLogadoQuery());

            if ((!dresAbrangenciaUsuarioLogado?.Any(x => x.Id == dreId) ?? true) || tipoPerfilUsuarioLogado is null || !Perfis.PodeVisualizarDre(tipoPerfilUsuarioLogado.Value))
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa DRE.");

            var resultados = await mediator.Send(new ObterDownloadDreResultadoProbabilidadeQuery(loteId, dreId));
            return await BuildCSVForExcel(resultados);
        }

        private static async Task<MemoryStream> BuildCSVForExcel(IEnumerable<DownloadResultadoProbabilidadeDto> resultados)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            {
                await writer.WriteLineAsync("<html><head>");
                await writer.WriteLineAsync("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                await writer.WriteLineAsync("<style>th { font-weight: bold; }</style>");
                await writer.WriteLineAsync("</head><body>");
                await writer.WriteLineAsync("<table border='1'>");
                await writer.WriteLineAsync("<tr><th>Nome DRE</th><th>Codigo UE</th><th>Nome UE</th><th>Componente</th><th>Código Habilidade</th><th>Habilidade</th><th>Turma</th><th>Abaixo do Básico</th><th>Básico</th><th>Adequado</th><th>Avançado</th></tr>");

                foreach (var item in resultados)
                {
                    await writer.WriteLineAsync("<tr>" +
                         
                        $"<td>{item.NomeDreAbreviacao}</td>" +
                        $"<td class=\"numero\">{item.CodigoUe}</td>" +
                        $"<td>{item.NomeUe}</td>" +
                        $"<td>{item.Componente}</td>" +
                        $"<td>{item.CodigoHabilidade}</td>" +
                        $"<td>{item.HabilidadeDescricao}</td>" +
                        $"<td>{item.TurmaDescricao}</td>" +
                        $"<td>{item.AbaixoDoBasico}</td>" +
                        $"<td>{item.Basico}</td>" +
                        $"<td>{item.Adequado}</td>" +
                        $"<td>{item.Avancado}</td>" +
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