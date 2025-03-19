using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Token;
using ControleCadastro.Infra.Data.Contexto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Infra.Data.Identity
{
    public class TokenGenereteService : ITokenGenerateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TokenGenereteService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GerarTokenCadastro(string clientId, string clientSecret)
        {
            Autorization aut = BuscarClientAuthorization(clientId, clientSecret);

            var claims = new[]
            {
                new Claim("aut.Id", aut.Id.ToString()),
                new Claim("aut.ClientId", aut.ClientId.ToString()),
                new Claim("Admin", aut.IsAdmin.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var privatyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(privatyKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GerarToken(Login login)
        {
            Autorization aut = BuscarClientAuthorization(login.ClientId, login.ClientSecret);
            Cliente cliente = BuscarClient(login.Email, aut.Id);

            if (!cliente.ValidarSenha(login.Senha))
                throw new Exception("Senha incorreta");

            var claims = new[]
            {
                new Claim("aut.Id", aut.Id.ToString()),
                new Claim("aut.ClientId", aut.ClientId.ToString()),
                new Claim("Admin", aut.IsAdmin.ToString()),
                new Claim("cliente.id",cliente.Id.ToString()),                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var privatyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(privatyKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Autorization BuscarClientAuthorization(string clientId, string clientSecret)
        {
            var autorizationBuscado = _context.Autorization.Where(x => x.ClientId == clientId && x.ClientSecret == clientSecret).FirstOrDefault();
            if (autorizationBuscado == null)
            {
                throw new Exception("ClientId não autorizado");
            }
            return autorizationBuscado;
        }

        private Cliente BuscarClient(string email, int id)
        {
            var clientBuscado = _context.Cliente.Where(x => x.Email.ToLower() == email.ToLower() && x.AutorizationId == id).FirstOrDefault();
            if (clientBuscado == null)
            {
                throw new Exception("Cliente não Cadastrado");
            }

            return clientBuscado;
        }

        
    }
}
