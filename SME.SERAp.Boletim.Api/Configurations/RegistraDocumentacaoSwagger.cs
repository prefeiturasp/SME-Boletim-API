﻿using Microsoft.OpenApi.Models;

namespace SME.SERAp.Boletim.Api.Configurations
{
    public static class RegistraDocumentacaoSwagger
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SERAp Boletim API", Version = "1.0" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Para autenticação, incluir 'Bearer' seguido do token JWT. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);
            });
        }
    }
}
