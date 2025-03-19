using AutoMapper;
using ControleCadastro.Application.DTOs.ClienteDTO;
using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ControleCadastro.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteService(IClienteRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _clienteRepository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Incluir(ClienteDTOInsert clienteDTO)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);

            if(admin)
            clienteDTO.AutorizationId = clienteDTO.AutorizationId == 0 ? autid : clienteDTO.AutorizationId;

            clienteDTO.AutorizationId = autid;

            var clienteExistente = await _clienteRepository.ConsultarPorEmail(clienteDTO.Email);
            if (clienteExistente != null)
                return RetornoHelper.GerarRetorno(400, "Já existe um cliente com este e-mail.");

            Cliente cliente = new Cliente().CriarUsusario(clienteDTO.AutorizationId, clienteDTO.Nome, clienteDTO.Email, clienteDTO.SenhaHash);

            if (await _clienteRepository.Incluir(cliente))
                return RetornoHelper.GerarRetorno(200, "Cliente inserido com sucesso");

            return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao Incluir o Cliente");
        }

        public async Task<IActionResult> Alterar(ClienteDTO clienteDTO)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);

            Cliente clienteBuscado = await _clienteRepository.ConsultarId(clienteDTO.Id);
            if (clienteBuscado == null)
                return RetornoHelper.GerarRetorno(404, "Cliente não encontrado");

            if(clienteDTO.AutorizationId == 0)
                clienteDTO.AutorizationId = clienteBuscado.AutorizationId;

            if (admin || clienteBuscado.AutorizationId == autid)
            {
                var cliente = await _clienteRepository.Alterar(_mapper.Map<Cliente>(clienteDTO));
                return RetornoHelper.GerarRetorno(200, "Cliente alterado com sucesso");
            }
            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para alterar este cliente");
        }
        public async Task<IActionResult> Excluir(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);


            Cliente clienteBuscado = await _clienteRepository.ConsultarId(id);
            if (clienteBuscado == null)
                return RetornoHelper.GerarRetorno(404, "Cliente não encontrado");

            if (admin || clienteBuscado.AutorizationId == autid)
            {
                if (await _clienteRepository.Excluir(id))
                    return RetornoHelper.GerarRetorno(200, "Cliente excluído com sucesso");

                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao excluir o Cliente");
            }

            return RetornoHelper.GerarRetorno(403, "Você não tem permissão para alterar este cliente");
        }

        public async Task<IActionResult> ConsultarId(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);


            Cliente clienteBuscado = await _clienteRepository.ConsultarId(id);
            if (clienteBuscado == null)
                return RetornoHelper.GerarRetorno(404, "Cliente não encontrado");

            if (admin || clienteBuscado.AutorizationId == autid)
            {
                var clienteDTO = _mapper.Map<ClienteDTOFront>(clienteBuscado);
                return new OkObjectResult(clienteDTO);
            }
            return RetornoHelper.GerarRetorno(403, "Você não tem permissão para acessar este cliente");
        }

        public async Task<IActionResult> ListarTodos()
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);

            if (admin)
            {
                var clientesAdmin = await _clienteRepository.ListarTodos();
                return new OkObjectResult(_mapper.Map<IEnumerable<ClienteDTOFront>>(clientesAdmin));
            }

            var clientes = await _clienteRepository.ListarTodosAutorized(autid);
            return new OkObjectResult(_mapper.Map<IEnumerable<ClienteDTOFront>>(clientes));
        }

        public async Task<IActionResult> ListarTodosCompleto()
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);

            if (admin)
            {
                var clientesAdmin = await _clienteRepository.ListarTodos();
                return new OkObjectResult(_mapper.Map<IEnumerable<ClienteDTOFrontCompleto>>(clientesAdmin));
            }

            var clientes = await _clienteRepository.ListarTodosAutorized(autid);
            return new OkObjectResult(_mapper.Map<IEnumerable<ClienteDTOFrontCompleto>>(clientes));
        }

        public async Task<IActionResult> AtualizaDados(ClienteDTOAtualiza clientDTO)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);

            Cliente clienteBuscado = await _clienteRepository.ConsultarId(clientId);
            if (clienteBuscado == null)
                return RetornoHelper.GerarRetorno(404, "Cliente não encontrado");

            if (clientDTO.Logotipo?.Length>0)
            {
                clienteBuscado.Logotipo = clientDTO.Logotipo;

                if (await _clienteRepository.AtualizaImagem(clienteBuscado))
                    return RetornoHelper.GerarRetorno(200, "Logo atualizada com sucesso");

                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao atualizar o Logo");
            }
            else if (clientDTO.SenhaHash.Length > 0)
            {
                clienteBuscado.Senha = clienteBuscado.AtualizaSenha(clientDTO.SenhaHash);


                if (await _clienteRepository.AtualizaSenha(clienteBuscado))
                    return RetornoHelper.GerarRetorno(200, "Senha atualizada com sucesso");
                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao atualizar a senha");
            }           

            return RetornoHelper.GerarRetorno(400, "Erro durante o processamento");
        }
        public async Task<IActionResult> Perfil()
        {
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.Id")?.Value);

            Cliente clienteBuscado = await _clienteRepository.ConsultarId(clientId);
            if (clienteBuscado == null)
                return RetornoHelper.GerarRetorno(404, "Cliente não encontrado");

            var clienteDTO = _mapper.Map<ClienteDTOFront>(clienteBuscado);
            return new OkObjectResult(clienteDTO);
        }
    }
}
