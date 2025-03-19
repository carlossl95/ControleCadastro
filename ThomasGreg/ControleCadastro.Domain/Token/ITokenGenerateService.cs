using ControleCadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Token
{
    public interface ITokenGenerateService
    {       
        public string GerarToken(Login login);
        public string GerarTokenCadastro(string clientId, string clientSecret);
    }
}
