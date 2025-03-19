using ControleCadastro.Domain.Exception;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public void Validar(int id, int clientId, string logadouro, string numero, string cep, string complemento, string cidade, string estado, bool isPrincipal)
        {
            DomainExceptionValidation.When(id < 1, "Id não pode ser menos que 1");
            DomainExceptionValidation.When(clientId < 1, "O Id do cliente deve ser informado");
            DomainExceptionValidation.When(logadouro.ToString().Length > 255, "Logadouro não pode ser maior que 255 caracteres");
            DomainExceptionValidation.When(logadouro.ToString().Length < 5, "Logadouro não pode ser menor que 5 caracteres");
            DomainExceptionValidation.When(numero.ToString().Length > 15, "Numero não pode ser maior que 15 caracteres");
            DomainExceptionValidation.When(cep.ToString().Length > 15, "Cep não pode ser maior que 15 caracteres");
            DomainExceptionValidation.When(!string.IsNullOrEmpty(cep), "Cep deve ser preenchido");
            DomainExceptionValidation.When(complemento.ToString().Length > 20, "Complemento não pode ser maior que 20 caracteres");
            DomainExceptionValidation.When(!string.IsNullOrEmpty(cidade), "Cidade deve ser preenchida");
            DomainExceptionValidation.When(cidade.ToString().Length > 50, "Cidade não pode ser maior que 50 caracteres");
            DomainExceptionValidation.When(!string.IsNullOrEmpty(estado), "Estado deve ser preenchido");
            DomainExceptionValidation.When(complemento.ToString().Length > 2, "Estado deve ser preenchido por suas siglas com apenas 2 caracteres");
            DomainExceptionValidation.When(isPrincipal == null, "is Principal deve ser prenchido");
        }


    }
}
