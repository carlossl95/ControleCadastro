using AutoMapper;
using ControleCadastro.Application.DTOs;
using ControleCadastro.Application.DTOs.AuthorizationDTO;
using ControleCadastro.Application.DTOs.ClienteDTO;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Exception;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ControleCadastro.Application.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly IAutorizationRepository _autorizationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public AutorizationService(IAutorizationRepository autorization, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _autorizationRepository = autorization;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IActionResult> Incluir(AuthorizationDTO autorization)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            if (admin)
                RetornoHelper.GerarRetorno(400, "Somente administradores do sistema tem acesso a esta função");

            var clientIdExiste = await _autorizationRepository.ConsultarClientId(autorization.ClientId);
            if (clientIdExiste != null)
                RetornoHelper.GerarRetorno(400, "Já existe um cadastro com este ClientId");

            if (await _autorizationRepository.Incluir(_mapper.Map<Autorization>(autorization)))
                return RetornoHelper.GerarRetorno(200, "Autorização inserida com sucesso");

            return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao Incluir a Autorização");
        }

        public async Task<IActionResult> Alterar(AuthorizationDTO autorization)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            if (admin)
                RetornoHelper.GerarRetorno(400, "Somente administradores do sistema tem acesso a esta função");

            var clientIdExiste = await _autorizationRepository.ConsultarClientId(autorization.ClientId);
            if (clientIdExiste != null && clientIdExiste.Id != autorization.Id)
                RetornoHelper.GerarRetorno(400, "Já existe um cadastro com este ClientId");

            if (await _autorizationRepository.Alterar(_mapper.Map<Autorization>(autorization)))
                return RetornoHelper.GerarRetorno(200, "Autorização Atualizada com sucesso");

            return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao editar a Autorização");
        }

        public async Task<IActionResult> Excluir(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            if (admin)
                RetornoHelper.GerarRetorno(400, "Somente administradores do sistema tem acesso a esta função");

            var clientIdExiste = await _autorizationRepository.ConsultarId(id);
            if (clientIdExiste == null)
                RetornoHelper.GerarRetorno(400, "Autorização não encontrada");

            if (await _autorizationRepository.Excluir(id))
                return RetornoHelper.GerarRetorno(200, "Autorização excluida com sucesso");

            return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao excluir a Autorização");

        }

        public async Task<IActionResult> ListarTodos()
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            if (admin)
                RetornoHelper.GerarRetorno(400, "Somente administradores do sistema tem acesso a esta função");

            var autorizations = await _autorizationRepository.ListarTodos();

            return new OkObjectResult(_mapper.Map<IEnumerable<AuthorizationDTO>>(autorizations));
        }

        public async Task<IActionResult> ConsultarClientId(string clientId)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            if (admin)
                RetornoHelper.GerarRetorno(400, "Somente administradores do sistema tem acesso a esta função");

            var clientIdExiste = await _autorizationRepository.ConsultarClientId(clientId);
            if (clientIdExiste == null)
                RetornoHelper.GerarRetorno(400, "Autorização não encontrada");

            return new OkObjectResult(_mapper.Map<AuthorizationDTO>(clientIdExiste));
        }
    }
}
