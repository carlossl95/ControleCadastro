using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.DTOs.AuthorizationDTO
{
    public class AuthorizationDTO
    {
        [Key]
        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo Id não pode ser nulo.\"}")]
        [Column("Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo ClientId é obrigatório.\"}")]
        [StringLength(255, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo ClientId deve ter no máximo 255 caracteres.\"}")]
        public string ClientId { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo ClientSecret é obrigatório.\"}")]
        [StringLength(255, ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo ClientSecret deve ter no máximo 255 caracteres.\"}")]
        public string ClientSecret { get; set; }

        [Required(ErrorMessage = "{\"Codigo\":400,\"Mensagem\":\"O campo IsAdmin é obrigatório.\"}")]
        public bool IsAdmin { get; set; }
    }
}
