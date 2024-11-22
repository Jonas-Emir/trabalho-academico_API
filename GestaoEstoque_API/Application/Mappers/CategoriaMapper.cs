using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Application.Mappers
{
    public class CategoriaMapper : Profile
    {
        public CategoriaMapper()
        {
            CreateMap<Produto, ProdutoResponseDto>();
            CreateMap<CategoriaResponseDto, Categoria>();
            CreateMap<ProdutoResponseDto, Produto>();
        }
    }
}
