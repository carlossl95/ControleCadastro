using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Entities
{
    public class Retorno
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }

        public Retorno()
        {
        }        
    }
}
