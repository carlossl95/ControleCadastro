using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.DTOs.ClienteDTO
{
    public class ClienteDTOFrontCompleto
    {
        [Key]
        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Id não pode ser nulo.\"}")]
        [Column("Id")]
        public int Id { get; set; }       

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Nome é obrigatório.\"}")]
        [StringLength(255, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Nome deve ter no máximo 255 caracteres.\"}")]
        [Column("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Email é obrigatório.\"}")]
        [EmailAddress(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Email deve ser um endereço de e-mail válido.\"}")]
        [StringLength(255, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Email deve ter no máximo 255 caracteres.\"}")]
        [Column("Email")]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Column("Logotipo")]
        public byte[]? Logotipo { get; set; }

        public ICollection<EnderecoDTOFront> Enderecos { get; set; }
    }
}
