namespace ControleCadastro.Front.Models
{
    public class Cadastro
    {
        public string Nome { get; set; }  
        public string Email { get; set; } 
        public string SenhaHash { get; set; }

        public Cadastro()
        { }
    }
}