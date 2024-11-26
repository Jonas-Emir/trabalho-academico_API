using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Application.Mappers
{
    public class EstoqueMapper : Profile
    {
        public EstoqueMapper()
        {
            CreateMap<Estoque, EstoqueResponseDto>();
            CreateMap<EstoqueResponseDto, Estoque>();
            CreateMap<EstoqueRequestDto, Estoque>();
            CreateMap<Estoque, EstoqueRequestDto>();
        }
    }
}
