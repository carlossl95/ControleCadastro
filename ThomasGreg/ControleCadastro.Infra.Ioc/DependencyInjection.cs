using ControleCadastro.Application.Mappings;
using ControleCadastro.Infra.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Infra.Data.Repositories;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ControleCadastro.Domain.Token;
using ControleCadastro.Infra.Data.Identity;
using Microsoft.AspNetCore.Http;
using ControleCadastro.Infra.Data.Procedure;

namespace ControleCadastro.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = System.TimeSpan.Zero

                };
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                // Desabilitar a conversão para camelCase
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });


            services.AddAutoMapper(typeof(EntitiesDTOProfile));


            //Repositories
            services.AddScoped<IAutorizationRepository, AutorizationRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();



            //Services
            services.AddScoped<IAutorizationService, AutorizationService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<ITokenGenerateService, TokenGenereteService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();





            return services;
        }
    }
}
