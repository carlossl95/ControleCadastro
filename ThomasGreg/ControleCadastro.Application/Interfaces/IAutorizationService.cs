using ControleCadastro.Application.DTOs;
using ControleCadastro.Application.DTOs.AuthorizationDTO;
using ControleCadastro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.Interfaces
{
    public interface IAutorizationService
    {
        Task<IActionResult> Incluir(AuthorizationDTO autorization);
        Task<IActionResult> Alterar(AuthorizationDTO autorization);
        Task<IActionResult> Excluir(int id);
        Task<IActionResult> ConsultarClientId(string clientId);
        Task<IActionResult> ListarTodos();
    }
}
