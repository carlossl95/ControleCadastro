using System.Text.Json.Serialization;

namespace ControleCadastro.Front.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Logadouro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public bool IsPrincipal { get; set; }

        public Endereco() { }
    }
}
