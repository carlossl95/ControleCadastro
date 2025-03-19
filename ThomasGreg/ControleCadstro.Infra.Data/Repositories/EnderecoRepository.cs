using ControleCadastro.Domain.Entities;
using ControleCadastro.Domain.Interface;
using ControleCadastro.Infra.Data.Contexto;
using Microsoft.EntityFrameworkCore;

namespace ControleCadastro.Infra.Data.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly ApplicationDbContext _context;

        public EnderecoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Alterar(Endereco endereco)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC AtualizarEndereco @Id = {0}, @ClienteId = {1}, @Logadouro = {2}, @Numero = {3}, @Cep = {4}, @Complemento = {5}, @Cidade = {6}, @Estado = {7}, @IsPrincipal = {8}",
                endereco.Id, endereco.ClienteId, endereco.Logadouro, endereco.Numero, endereco.Cep, endereco.Complemento, endereco.Cidade, endereco.Estado, endereco.IsPrincipal);

            return result > 0;
        }

        public async Task<IEnumerable<Endereco>> ConsultarPorClienteId(int id)
        {
            return await _context.Endereco.Where(e => e.ClienteId == id).ToListAsync();
        }

        public async Task<Endereco> ConsultarPorId(int id)
        {
            return await _context.Endereco.FindAsync(id);            
        }

        public async Task<bool> Excluir(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC ExcluirEndereco @Id = {0}",
                id);

            return result > 0;
        }

        public async Task<bool> Incluir(Endereco endereco)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC InserirEndereco @ClienteId = {0}, @Logadouro = {1}, @Numero = {2}, @Cep = {3}, @Complemento = {4}, @Cidade = {5}, @Estado = {6}, @IsPrincipal = {7}",
                endereco.ClienteId, endereco.Logadouro, endereco.Numero, endereco.Cep, endereco.Complemento, endereco.Cidade, endereco.Estado, endereco.IsPrincipal);

            return result > 0;
        }

        public async Task<IEnumerable<Endereco>> ListarTodos()
        {
            return await _context.Endereco.ToListAsync();
        }
    }
}
