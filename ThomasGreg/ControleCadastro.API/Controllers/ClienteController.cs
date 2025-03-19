using ControleCadastro.Application.DTOs.ClienteDTO;
using ControleCadastro.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCadastro.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;


        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("inserir")]
        public async Task<IActionResult> Incluir(ClienteDTOInsert clienteDTO)
        {
            try
            {
                return await _clienteService.Incluir(clienteDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Alterar(ClienteDTO clienteDTO)
        {
            try
            {
                return await _clienteService.Alterar(clienteDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        [HttpDelete("deletarPorId/{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                return await _clienteService.Excluir(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        [HttpGet("consultarPorId/{id}")]
        public async Task<IActionResult> ConsultarId(int id)
        {
            try
            {
                return await _clienteService.ConsultarId(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }

        [HttpPut("atualizarDados")]
        public async Task<IActionResult> AtualizaDados(ClienteDTOAtualiza clientDTO)
        {
            try
            {
                return await _clienteService.AtualizaDados(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }
        [HttpGet("listarClientes")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                return await _clienteService.ListarTodos();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Resposta(500, ex.Message));
            }
        }       

        [HttpGet("Perfil")]
        public async Task<IActionResult> Perfil()
        {
            try
            {
                return await _clienteService.Perfil();
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
