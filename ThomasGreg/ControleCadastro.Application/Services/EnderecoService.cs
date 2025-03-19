using AutoMapper;
using ControleCadastro.Application.DTOs;
using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Application.Interfaces;
using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleCadastro.Application.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository, IClienteRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _clienteRepository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _enderecoRepository = enderecoRepository;
        }
        public async Task<IActionResult> Incluir(EnderecoDTOInsert enderecoDTO)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Admin")?.Value);
            int autid = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("aut.Id")?.Value);
            int clienteId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);

            enderecoDTO.ClienteId = clienteId;

            var clienteExiste = await _clienteRepository.ConsultarId(enderecoDTO.ClienteId);

            if (clienteExiste == null)
                return RetornoHelper.GerarRetorno(400, "Cliente não encontrado");

            if (admin || clienteExiste.Id == enderecoDTO.ClienteId && clienteExiste.AutorizationId == autid)
            {
                if (await _enderecoRepository.Incluir(_mapper.Map<Endereco>(enderecoDTO)))
                    return RetornoHelper.GerarRetorno(200, "Endereco incluído com sucesso ao cliente");

                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao incluir o endereço ao cliente");
            }
            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para incluir um endereço a este cliente");
        }

        public async Task<IActionResult> Alterar(EnderecoDTO enderecoDTO)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);

            enderecoDTO.ClienteId = clientId;

            var enderecoExiste = await _enderecoRepository.ConsultarPorId(enderecoDTO.Id);
            if (enderecoExiste == null)
                return RetornoHelper.GerarRetorno(404, "Endereço não encontrado");

            if (admin || enderecoExiste.ClienteId == clientId)

            {
                if (await _enderecoRepository.Alterar(_mapper.Map<Endereco>(enderecoDTO)))
                    return RetornoHelper.GerarRetorno(200, "Endereço alterado com sucesso");

                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao editar o endereço");
            }

            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para alterar este endereço");
        }

        public async Task<IActionResult> Excluir(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);

            var enderecoExiste = await _enderecoRepository.ConsultarPorId(id);
            if (enderecoExiste == null)
                return RetornoHelper.GerarRetorno(404, "Endereço não encontrado");

            if (admin || enderecoExiste.ClienteId == clientId)
            {
                if (await _enderecoRepository.Excluir(id))
                    return RetornoHelper.GerarRetorno(200, "Endereço excluído com sucesso");

                return RetornoHelper.GerarRetorno(400, "Ocorreu um erro ao excluir o endereço");
            }

            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para alterar este endereço");
        }

        public async Task<IActionResult> ConsultarPorId(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);

            var enderecoExiste = await _enderecoRepository.ConsultarPorId(id);
            if (enderecoExiste == null)
                return RetornoHelper.GerarRetorno(404, "Endereço não encontrado");

            if (admin || enderecoExiste.ClienteId == clientId)
                return new OkObjectResult(_mapper.Map<EnderecoDTOFront>(enderecoExiste));

            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para consultar este endereço");
        }

        public async Task<IActionResult> ConsultarPorClienteId(int id)
        {
            bool admin = Convert.ToBoolean(_httpContextAccessor.HttpContext?.User?.FindFirst("Admin")?.Value);
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.id")?.Value);


            if (admin || clientId == id)
            {
                var enderecoAdm = await _enderecoRepository.ConsultarPorClienteId(id);
                return new OkObjectResult(_mapper.Map<IEnumerable<EnderecoDTOFront>>(enderecoAdm));
            }
            return RetornoHelper.GerarRetorno(401, "Você não tem permissão para consultar este endereço");
        }

        public async Task<IActionResult> ListarTodos()
        {           
            int clientId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("cliente.Id")?.Value);

            var enderecos = await _enderecoRepository.ConsultarPorClienteId(clientId);
            return new OkObjectResult(_mapper.Map<IEnumerable<EnderecoDTOFront>>(enderecos));

        }
    }
}
