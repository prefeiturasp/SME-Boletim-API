﻿using MediatR;
using SME.SERAp.Boletim.Aplicacao.Interfaces.UseCase;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimEscolarPorUe;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUesAbrangenciaUsuarioLogado;
using SME.SERAp.Boletim.Infra.Dtos.Boletim;
using SME.SERAp.Boletim.Infra.Exceptions;
using System.Text.RegularExpressions;

namespace SME.SERAp.Boletim.Aplicacao.UseCase
{
    public class ObterBoletimEscolarPorUeUseCase : IObterBoletimEscolarPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterBoletimEscolarPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<BoletimEscolarDto>> Executar(long loteId, long ueId, FiltroBoletimDto filtros)
        {
            var abrangenciasUsuarioLogado = await mediator
                .Send(new ObterUesAbrangenciaUsuarioLogadoQuery());

            if (!abrangenciasUsuarioLogado?.Any(x => x.UeId == ueId) ?? true)
                throw new NaoAutorizadoException("Usuário não possui abrangências para essa UE.");

            var boletins = await mediator.Send(new ObterBoletimEscolarPorUeQuery(loteId, ueId, filtros));
            if (boletins != null)
            {

                var listBoletimDto = new List<BoletimEscolarDto>();
                foreach (var boletim in boletins)
                {
                    var boletimDto = new BoletimEscolarDto();

                    boletimDto.ProvaId = boletim.ProvaId;
                    boletimDto.UeId = boletim.UeId;
                    boletimDto.AbaixoBasico = $"{boletim.AbaixoBasico} ({Math.Round(boletim.AbaixoBasicoPorcentagem)}%)";
                    boletimDto.Basico = $"{boletim.Basico} ({Math.Round(boletim.BasicoPorcentagem)}%)";
                    boletimDto.Adequado = $"{boletim.Adequado} ({Math.Round(boletim.AdequadoPorcentagem)}%)";
                    boletimDto.Avancado = $"{boletim.Avancado} ({Math.Round(boletim.AvancadoPorcentagem)}%)";
                    boletimDto.Total = boletim.Total;
                    boletimDto.MediaProficiencia = boletim.MediaProficiencia;
                    boletimDto.ComponenteCurricular = boletim.ComponenteCurricular;

                    int indiceParenteses = boletim.ComponenteCurricular.IndexOf('(');

                    boletimDto.DisciplinaDescricao = boletim.ComponenteCurricular.Substring(0, indiceParenteses).Trim();
                    boletimDto.AnoEscolar = Convert.ToInt32(Regex.Match(boletim.ComponenteCurricular, @"\d+").Value);
                    boletimDto.AnoEscolarDescricao = $"{boletimDto.AnoEscolar}º Ano";
                    boletimDto.DisciplinaId = boletimDto.DisciplinaDescricao == "Matemática" ? 2 : 5;

                    listBoletimDto.Add(boletimDto);
                    ;
                }
                return listBoletimDto;

            }

            throw new NegocioException($"Não foi possível localizar boletins para a UE {ueId}");

        }
    }
}