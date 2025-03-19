using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Infra.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Infra.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Alterar(Cliente cliente)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC EditarClientePorId @Id = {0}, @AutorizationId = {1}, @Nome = {2}, @Email = {3}, @Logotipo = {4}",
                cliente.Id, cliente.AutorizationId, cliente.Nome, cliente.Email, cliente.Logotipo);

            return result > 0;
        }

        public async Task<bool> AtualizaSenha(Cliente clientDTO)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
         "EXEC AlterarSenhaClientePorId @Id = {0}, @SenhaHash = {1}",
         clientDTO.Id, clientDTO.SenhaHash);

            return result > 0;
        }

        public async Task<Cliente> ConsultarId(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task<Cliente> ConsultarPorEmail(string email)
        {
            return await _context.Cliente.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> Excluir(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC ExcluirClientePorId @Id = {0}", id);

            return result > 0;
        }

        public async Task<bool> Incluir(Cliente cliente)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC InserirCliente @AutorizationId = {0}, @Nome = {1}, @Email = {2}, @SenhaHash = {3}",
                cliente.AutorizationId, cliente.Nome, cliente.Email, cliente.SenhaHash);

            return result > 0;
        }

        public async Task<IEnumerable<Cliente>> ListarTodos()
        {
            return await _context.Cliente.Include(c => c.Enderecos).ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> ListarTodosAutorized(int id)
        {
            return await _context.Cliente.Where(c => c.AutorizationId == id).Include(c => c.Enderecos).ToListAsync();
        }

        public async Task<bool> AtualizaImagem(Cliente cliente)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                
                "EXEC AtualizarLogotipoPorId @Id = {0}, @Logotipo = {1}",
                
                cliente.Id, cliente.Logotipo);

            return result > 0;
        }
    }
}
