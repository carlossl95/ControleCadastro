using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleCadastro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private ITokenGenerateService _token;

        public TokenController(ITokenGenerateService token)
        {
            _token = token;
        }

        [HttpPost("geral")]
        [Authorize]
        public async Task<IActionResult> GerarToken(Login login)
        {
            try
            {
                var tokenGenerate = _token.GerarToken(login);
                return Ok(tokenGenerate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        [HttpGet("cadastro/{clientId}/{clientSecret}")]
        public async Task<IActionResult> GerarTokenCadastro(string clientId, string clientSecret)
        {
            try
            {
                var tokenGenerate = _token.GerarTokenCadastro(clientId,  clientSecret);
                return Ok(tokenGenerate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        public class Resposta
        {
            public Resposta(int status, string mensagem)
            {
                this.Status = status;
                this.Mensagem = mensagem;

            }
            public int Status { get; set; }
            public string Mensagem { get; set; }

        }
    }
}
