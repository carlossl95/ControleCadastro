namespace ControleCadastro.Front.Models
{
    public class Cadastro
    {
        public string Nome { get; set; }  // Adicionei o 'set'
        public string Email { get; set; } // Adicionei o 'set'
        public string SenhaHash { get; set; } // Adicionei o 'set'

        public Cadastro()
        { }
    }
}