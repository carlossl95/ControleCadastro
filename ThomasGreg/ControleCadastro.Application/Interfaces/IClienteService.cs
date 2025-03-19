using ControleCadastro.Application.DTOs.ClienteDTO;
using Microsoft.AspNetCore.Mvc;

namespace ControleCadastro.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IActionResult> Incluir(ClienteDTOInsert clienteDTO);
        Task<IActionResult> Alterar(ClienteDTO clienteDTO);
        Task<IActionResult> Excluir(int id);
        Task<IActionResult> ConsultarId(int id);
        Task<IActionResult> ListarTodos();
        Task<IActionResult> ListarTodosCompleto();
        Task<IActionResult> AtualizaDados(ClienteDTOAtualiza clientDTO);
        Task<IActionResult> Perfil();
    }
}
