using ControleCadastro.Application.DTOs;
using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<IActionResult> Incluir(EnderecoDTOInsert enderecoDTO);
        Task<IActionResult> Alterar(EnderecoDTO endereco);
        Task<IActionResult> Excluir(int id);
        Task<IActionResult> ConsultarPorClienteId(int id);
        Task<IActionResult> ConsultarPorId(int id);
        Task<IActionResult> ListarTodos();
    }
}
