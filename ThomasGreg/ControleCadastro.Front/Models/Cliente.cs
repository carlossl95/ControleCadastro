using System.IdentityModel.Tokens.Jwt;
using System;

namespace ControleCadastro.Front.Models
{
    public class Cliente
    {
        public int Id { get; private set; }
        public Autorization? AutorizationId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public byte[]? Logotipo { get;  set; }

        public Cliente()
        { }

        public Cliente(string nome, string email, byte[] logo)
        {
            Nome = nome;
            Email = email;
            Logotipo = logo;
        }

        public static Dictionary<string, string> ClaimsJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
                throw new ArgumentException("Token JWT inválido");

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtToken.Claims
                .ToDictionary(c => c.Type, c => c.Value);

            return claims;
        }
    }
}
