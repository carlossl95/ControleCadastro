using ControleCadastro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Utilities
{
    public static class RetornoHelper
    {
        public static IActionResult GerarRetorno(int codigo, string mensagem)
        {
            var retorno = new
            {
                Codigo = codigo,
                Mensagem = mensagem
            };

            return new ObjectResult(retorno) { StatusCode = codigo };
        }
    }
}
