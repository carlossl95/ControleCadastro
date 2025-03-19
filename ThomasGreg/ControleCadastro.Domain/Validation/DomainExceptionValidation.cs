using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Domain.Exception
{
    public class DomainExceptionValidation : IOException
    {
        public DomainExceptionValidation(string error) : base(error) {}

        public static void When(bool condicao, string error)
        {
            if (condicao)
            {
                throw new DomainExceptionValidation(error);
            }
        }
    }
}
