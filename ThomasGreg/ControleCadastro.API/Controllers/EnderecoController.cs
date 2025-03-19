using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCadastro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnderecoController : Controller
    {
        private readonly IEnderecoService _enderecoService;


        public EnderecoController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }
        [HttpPost("inserir")]
        public async Task<IActionResult> Incluir(EnderecoDTOInsert endereco)
        {
            try
            {
                return await _enderecoService.Incluir(endereco);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpPut("editar")]
        public async Task<IActionResult> Alterar(EnderecoDTO endereco)
        {
            try
            {
                return await _enderecoService.Alterar(endereco);
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
                return await _enderecoService.Excluir(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpGet("consultarPorId{id}")]
        public async Task<IActionResult> ConsultarPorId(int id)
        {
            try
            {
                return await _enderecoService.ConsultarPorId(id);
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
                return await _enderecoService.ListarTodos();
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
