using ControleCadastro.Domain.Exception;
using System;
using System.Collections.Generic;

namespace ControleCadastro.Domain.Entities
{
    public class Autorization
    {
        public int Id { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public bool IsAdmin { get; private set; }
        
    }
}
