using ControleCadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Interface
{
    public interface IEnderecoRepository
    {

        Task<bool> Incluir(Endereco endereco);
        Task<bool> Alterar(Endereco endereco);
        Task<bool> Excluir(int id);
        Task<Endereco> ConsultarPorId(int id);
        Task<IEnumerable<Endereco>> ConsultarPorClienteId(int id);
        Task<IEnumerable<Endereco>> ListarTodos();

    }
}
