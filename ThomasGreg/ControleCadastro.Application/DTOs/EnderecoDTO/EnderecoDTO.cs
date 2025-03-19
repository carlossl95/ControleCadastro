using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.DTOs.EnderecoDTO
{
    public class EnderecoDTO
    {
        [Key]
        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Id não pode ser nulo.\"}")]
        [Column("Id")]
        public int Id { get; set; }

        
        [Column("ClienteId")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Logradouro é obrigatório.\"}")]
        [StringLength(255, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Logradouro deve ter no máximo 255 caracteres.\"}")]
        [Column("Logadouro")]
        public string Logadouro { get; set; }

        [StringLength(15, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Número deve ter no máximo 15 caracteres.\"}")]
        [Column("Numero")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Cep é obrigatório.\"}")]
        [StringLength(15, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Cep deve ter no máximo 15 caracteres.\"}")]
        [Column("Cep")]
        public string Cep { get; set; }

        [StringLength(20, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Complemento deve ter no máximo 20 caracteres.\"}")]
        [Column("Complemento")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Cidade é obrigatório.\"}")]
        [StringLength(50, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Cidade deve ter no máximo 50 caracteres.\"}")]
        [Column("Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Estado é obrigatório.\"}")]
        [StringLength(2, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Estado deve ter no máximo 2 caracteres.\"}")]
        [Column("Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo IsPrincipal é obrigatório.\"}")]
        [Column("IsPrincipal")]
        public bool IsPrincipal { get; set; }
    }
}

