namespace ControleCadastro.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; set; }
        public int ClienteId { get; private set; }
        public string Logadouro { get; private set; } 
        public string? Numero { get; private set; }
        public string Cep { get; private set; } 
        public string? Complemento { get; private set; }
        public string Cidade { get; private set; } 
        public string Estado { get; private set; } 
        public bool IsPrincipal { get; private set; }
        public virtual Cliente Cliente { get; private set; }

        public Endereco() { }

        public Endereco(int id, int clientId, string logadouro, string numero, string cep, string complemento, string cidade, string estado, bool isPrincipal)
        { 
            Id = id;
            ClienteId = id;
            Logadouro = logadouro;
            Numero = numero;
            Cep = cep;
            Complemento = complemento;
            Estado = estado;
            Cidade = cidade;
            Estado = estado;
            IsPrincipal = isPrincipal;

        }
    }
}
