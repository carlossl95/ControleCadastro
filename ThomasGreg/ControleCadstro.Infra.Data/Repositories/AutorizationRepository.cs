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
    public class AutorizationRepository : IAutorizationRepository
    {
        private readonly ApplicationDbContext _context;

        public AutorizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Incluir(Autorization autorization)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC InserirAutorization @ClientId = {0}, @ClientSecret = {1}, @IsAdmin = {2}",
                autorization.ClientId, autorization.ClientSecret, autorization.IsAdmin);

            return result > 0;
        }

        public async Task<bool> Alterar(Autorization autorization)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC EditarAutorization @Id = {0}, @ClientId = {1}, @ClientSecret = {2}, @IsAdmin = {3}",
                autorization.Id, autorization.ClientId, autorization.ClientSecret, autorization.IsAdmin);

            return result > 0;
        }
        public async Task<bool> Excluir(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC ExcluirAutorization @Id = {0}",
                id);

            return result > 0;
        }

        public async Task<Autorization> ConsultarClientId(string clientId)
        {
            return await _context.Autorization.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Autorization> ConsultarId(int id)
        {
            return await _context.Autorization.FindAsync(id);
        } 

        public async Task<IEnumerable<Autorization>> ListarTodos()
        {
            return await _context.Autorization.ToListAsync();
        }
    }
}
