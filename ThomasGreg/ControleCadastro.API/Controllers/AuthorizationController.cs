using ControleCadastro.Application.DTOs.AuthorizationDTO;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ControleCadastro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AutorizationController : Controller
    {
        private readonly IAutorizationService _autorizationService;


        public AutorizationController(IAutorizationService autorization)
        {
            _autorizationService = autorization;
        }

        [HttpPost("inserir")]
        public async Task<IActionResult> Incluir(AuthorizationDTO autorization)
        {
            try
            {
                return Ok(await _autorizationService.Incluir(autorization));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpPut("editar")]
        public async Task<IActionResult> Alterar(AuthorizationDTO autorization)
        {
            try
            {
                return Ok(await _autorizationService.Alterar(autorization));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                return Ok(await _autorizationService.Excluir(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpGet("consultarClientId{clientId}")]
        public async Task<IActionResult> ConsultarId(string clientId)
        {
            try
            {
                return Ok(await _autorizationService.ConsultarClientId(clientId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }

        }
        [HttpGet("listarTodos")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                return Ok(await _autorizationService.ListarTodos());
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
