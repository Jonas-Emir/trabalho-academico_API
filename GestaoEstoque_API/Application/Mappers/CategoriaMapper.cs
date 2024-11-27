using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Application.Mappers
{
    public class CategoriaMapper : Profile
    {
        public CategoriaMapper()
        {
            CreateMap<Categoria, CategoriaResponseDto>();
            CreateMap<CategoriaResponseDto, Categoria>();
            CreateMap<CategoriaRequestDto, Categoria>();
            CreateMap<Categoria, CategoriaRequestDto>();
        }
    }
}
