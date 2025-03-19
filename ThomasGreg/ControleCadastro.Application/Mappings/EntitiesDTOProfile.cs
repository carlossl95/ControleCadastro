using AutoMapper;
using ControleCadastro.Application.DTOs.AuthorizationDTO;
using ControleCadastro.Application.DTOs.ClienteDTO;
using ControleCadastro.Application.DTOs.EnderecoDTO;
using ControleCadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Application.Mappings
{
    public class EntitiesDTOProfile : Profile
    {
        public EntitiesDTOProfile()
        {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTOInsert>().ReverseMap();
            CreateMap<Cliente, ClienteDTOFront>().ReverseMap();
            CreateMap<Cliente, ClienteDTOFrontCompleto>().ReverseMap();
            CreateMap<Endereco, EnderecoDTO>().ReverseMap();
            CreateMap<Endereco, EnderecoDTOInsert>().ReverseMap();
            CreateMap<Endereco, EnderecoDTOFront>().ReverseMap();
            CreateMap<Autorization, AuthorizationDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTOAtualiza>().ReverseMap();
        }
    }
}
