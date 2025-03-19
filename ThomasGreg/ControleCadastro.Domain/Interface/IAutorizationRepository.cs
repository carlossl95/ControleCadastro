using ControleCadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Interface
{
    public interface IAutorizationRepository
    {
        Task<bool> Incluir(Autorization autorization);
        Task<bool> Alterar(Autorization autorization);
        Task<bool> Excluir(int id);
        Task<Autorization> ConsultarId(int id);
        Task<Autorization> ConsultarClientId(string clientId);
        Task<IEnumerable<Autorization>> ListarTodos();

    }
}
