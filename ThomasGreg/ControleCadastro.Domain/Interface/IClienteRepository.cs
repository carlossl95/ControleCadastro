using ControleCadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Interface
{
    public interface IClienteRepository
    {
        Task<bool> Incluir(Cliente cliente);
        Task<bool> Alterar(Cliente cliente);
        Task<bool> Excluir(int id);
        Task<Cliente> ConsultarId(int id);
        Task<IEnumerable<Cliente>> ListarTodos();
        Task<IEnumerable<Cliente>> ListarTodosAutorized(int id);
        Task<Cliente> ConsultarPorEmail(string email);
        Task<bool> AtualizaImagem(Cliente clientDTO);
        Task<bool> AtualizaSenha(Cliente clientDTO);
    }
}
