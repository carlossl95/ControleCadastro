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
    public class ClienteDTOAtualiza
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("SenhaHash")]
        public string? SenhaHash { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Column("Logotipo")]
        public byte[]? Logotipo { get; set; }


    }
}
