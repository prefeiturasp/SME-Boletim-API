﻿using MediatR;
using Microsoft.IdentityModel.Tokens;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTokenJwt
{
    public class ObterTokenJwtQueryHandler : IRequestHandler<ObterTokenJwtQuery, AutenticacaoRetornoDto>
    {
        private readonly JwtOptions jwtOptions;

        public ObterTokenJwtQueryHandler(JwtOptions jwtOptions)
        {
            this.jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public Task<AutenticacaoRetornoDto> Handle(ObterTokenJwtQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var claims = new List<Claim>
            {
                new Claim("LOGIN", request.Abrangencias.FirstOrDefault().Login),
                new Claim("USUARIOID", request.Abrangencias.FirstOrDefault().UsuarioId.ToString()),
                new Claim("USUARIO", request.Abrangencias.FirstOrDefault().Usuario),
                new Claim("GRUPOID", request.Abrangencias.FirstOrDefault().GrupoId.ToString()),
                new Claim("GRUPO", request.Abrangencias.FirstOrDefault().Grupo),
                //new Claim("PERMITECONSULTAR", request.Abrangencias.FirstOrDefault().PermiteConsultar.ToString()),
                //new Claim("PERMITEALTERAR", request.Abrangencias.FirstOrDefault().PermiteAlterar.ToString())
            };

            foreach (var abrangencia in request.Abrangencias)
                claims.Add(new Claim("DRE-UE-TURMA", $"{abrangencia.DreId}-{abrangencia.UeId}-{abrangencia.TurmaId}"));

            var dataHoraExpiracao = now.AddMinutes(double.Parse(jwtOptions.ExpiresInMinutes));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                notBefore: now,
                claims: claims,
                expires: dataHoraExpiracao,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha256)
                );

            var tokenGerado = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(new AutenticacaoRetornoDto(tokenGerado, dataHoraExpiracao));
        }
    }
}
